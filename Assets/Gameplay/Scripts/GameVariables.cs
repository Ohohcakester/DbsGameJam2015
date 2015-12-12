using UnityEngine;
using System.Collections.Generic;

public class GameVariables
{
    public float baseDetectionRange;
    public float baseDetectionDistance;
    public int nEnemies;
    public int nCrabs;
    public float multiplier;
    public float undercurrent;
    public float itemRespawnTime;

    public float turnSpeed;
    public float moveSpeed;
    public float baseAggressiveness;
    public float decisionTime;
    public float decisionTimeExtra;

    public float walkingEnemySpeed;

    public GameVariables()
    {
        ResetGameBuffs();
    }

    public void ResetGameBuffs()
    {
        baseDetectionRange = 10;
        baseDetectionDistance = 10;
        nEnemies = 3;
        nCrabs = 5;
        multiplier = 1f;
        undercurrent = 0f;
        itemRespawnTime = 25f;

        turnSpeed = 7.7f;
        moveSpeed = 3.2f;
        baseAggressiveness = 0.1f;
        decisionTime = 0f;
        decisionTimeExtra = 0.5f;

        walkingEnemySpeed = 0.9f;
    }

    public void ApplyDifficultyChange(float orbSize)
    {

    }
}
