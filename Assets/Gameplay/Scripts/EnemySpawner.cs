using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner
{
    private GameVariables gameVariables;
    private List<FlyingEnemyContainer> flyingEnemies; 

    public EnemySpawner()
    {
        
    }

    public void Update()
    {
        if (flyingEnemies.Count < gameVariables.nEnemies)
        {
            
        }
    }
}

internal class FlyingEnemyContainer
{
    public readonly FlyingEnemy flyingEnemy;
    public bool despawned { get; private set; }

    public void Despawn()
    {
        if (despawned) return;
        MonoBehaviour.Destroy(flyingEnemy);
        despawned = true;
    }
}