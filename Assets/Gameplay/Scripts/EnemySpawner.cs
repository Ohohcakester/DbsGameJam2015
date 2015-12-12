using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner
{
    private GameVariables gameVariables;
    private MazeManager mazeManager;
    private List<FlyingEnemyContainer> flyingEnemies;
    private List<WalkingEnemyContainer> walkingEnemies;
    private float safeModificationRange = 30f;

    private float nextWalkingEnemyDelete;
    private float walkingEnemyDeleteInterval = 9f;

    public EnemySpawner(GameVariables gameVariables)
    {
        this.gameVariables = gameVariables;
        mazeManager = Camera.main.GetComponent<MazeManager>();
        flyingEnemies = new List<FlyingEnemyContainer>();
        walkingEnemies = new List<WalkingEnemyContainer>();
        nextWalkingEnemyDelete = Time.time;
    }

    public void Update()
    {
        if (flyingEnemies.Count < gameVariables.nEnemies)
        {
            TrySpawnFlyingEnemy();
        }
        else if (flyingEnemies.Count > gameVariables.nEnemies)
        {
            TryDeleteRandomFlyingEnemy();
        }

        if (walkingEnemies.Count < gameVariables.nCrabs)
        {
            TrySpawnWalkingEnemy();
        }
        else if (walkingEnemies.Count > gameVariables.nCrabs)
        {
            TryDeleteRandomWalkingEnemy();
        }

        if (Time.time > nextWalkingEnemyDelete)
        {
            TryDeleteRandomWalkingEnemy();
            nextWalkingEnemyDelete += walkingEnemyDeleteInterval;
        }
    }

    private void TrySpawnFlyingEnemy()
    {
        var emptyTile = mazeManager.gridGraph.RandomEmptyBlock();
        if (!TileWithinSafeModificationRange(emptyTile)) return;
        var flyingEnemy = mazeManager.InstantiateFlyingEnemy(emptyTile.x, emptyTile.y);
        flyingEnemies.Add(new FlyingEnemyContainer(flyingEnemy));
    }

    private void TryDeleteRandomFlyingEnemy()
    {
        if (flyingEnemies.Count <= 0) return;

        int choice = Random.Range(0, flyingEnemies.Count);
        var flyingEnemy = flyingEnemies[choice];
        float distance = OhVec.Distance2D(flyingEnemy.Position(), mazeManager.PlayerPosition());
        if (distance >= safeModificationRange)
        {
            flyingEnemy.Despawn();
        }
        RemoveDespawnedFlyingEnemies();
    }

    private void TrySpawnWalkingEnemy()
    {
        var emptyTile = mazeManager.gridGraph.RandomEmptyBlock();
        if (!TileWithinSafeModificationRange(emptyTile)) return;
        var walkingEnemy = mazeManager.InstantiateWalkingEnemy(emptyTile.x, emptyTile.y);
        walkingEnemies.Add(new WalkingEnemyContainer(walkingEnemy));
    }

    private void TryDeleteRandomWalkingEnemy()
    {
        if (walkingEnemies.Count <= 0) return;

        int choice = Random.Range(0, walkingEnemies.Count);
        var walkingEnemy = walkingEnemies[choice];
        float distance = OhVec.Distance2D(walkingEnemy.Position(), mazeManager.PlayerPosition());
        if (distance >= safeModificationRange)
        {
            walkingEnemy.Despawn();
        }
        RemoveDespawnedWalkingEnemies();
    }

    private void RemoveDespawnedFlyingEnemies()
    {
        flyingEnemies.RemoveAll((fe) => fe.despawned);
    }

    private void RemoveDespawnedWalkingEnemies()
    {
        walkingEnemies.RemoveAll((fe) => fe.despawned);
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

internal class WalkingEnemyContainer
{
    public readonly WalkingEnemy walkingEnemy;
    public bool despawned { get; private set; }

    public WalkingEnemyContainer(WalkingEnemy walkingEnemy)
    {
        this.walkingEnemy = walkingEnemy;
    }

    public Vector2 Position()
    {
        return OhVec.toVector2(walkingEnemy.transform.position);
    }

    public void Despawn()
    {
        if (despawned) return;
        MonoBehaviour.Destroy(walkingEnemy);
        despawned = true;
    }
}