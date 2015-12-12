using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner
{
    private GameVariables gameVariables;
    private MazeManager mazeManager;
    private List<FlyingEnemyContainer> flyingEnemies;
    private float safeModificationRange = 30f;

    public EnemySpawner()
    {
        mazeManager = Camera.main.GetComponent<MazeManager>();
        flyingEnemies = new List<FlyingEnemyContainer>();
    }

    public void Update()
    {
        if (flyingEnemies.Count < gameVariables.nEnemies)
        {
            TrySpawnEnemy();
        } else if (flyingEnemies.Count > gameVariables.nEnemies)
        {
            TryDeleteRandomEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        var emptyTile = mazeManager.gridGraph.RandomEmptyBlock();
        if (!TileWithinSafeModificationRange(emptyTile)) return;
        var flyingEnemy = mazeManager.InstantiateFlyingEnemy(emptyTile.x, emptyTile.y);
        flyingEnemies.Add(new FlyingEnemyContainer(flyingEnemy));
    }

    private void TryDeleteRandomEnemy()
    {
        int choice = Random.Range(0, flyingEnemies.Count);
        var flyingEnemy = flyingEnemies[choice];
        float distance = OhVec.Distance2D(flyingEnemy.Position(), mazeManager.PlayerPosition());
        if (distance >= safeModificationRange)
        {
            flyingEnemy.Despawn();
        }
        RemoveDespawnedEnemies();
    }

    private void RemoveDespawnedEnemies()
    {
        flyingEnemies.RemoveAll((fe) => fe.despawned);
    }

    private bool TileWithinSafeModificationRange(Point emptyTile)
    {
        float actualX, actualY;
        mazeManager.ToActual(emptyTile.x, emptyTile.y, out actualX, out actualY);
        var playerPos = mazeManager.PlayerPosition();
        float dx = actualX - playerPos.x;
        float dy = actualY - playerPos.y;
        float distance = Mathf.Sqrt(dx * dx + dy * dy);
        return distance >= safeModificationRange;
    }
}

internal class FlyingEnemyContainer
{
    public readonly FlyingEnemy flyingEnemy;
    public bool despawned { get; private set; }

    public FlyingEnemyContainer(FlyingEnemy flyingEnemy)
    {
        this.flyingEnemy = flyingEnemy;
    }

    public Vector2 Position()
    {
        return OhVec.toVector2(flyingEnemy.transform.position);
    }

    public void Despawn()
    {
        if (despawned) return;
        MonoBehaviour.Destroy(flyingEnemy);
        despawned = true;
    }
}