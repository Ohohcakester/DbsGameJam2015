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
        float baseDetectionRange = 10;
        float baseDetectionDistance = 10;
        int nEnemies = 5;
        int nCrabs = 8;
        float multiplier = 1f;
        float undercurrent = 0f;
        float itemRespawnTime = 25f;

        float turnSpeed = 7.7f;
        float moveSpeed = 5.2f;
        float baseAggressiveness = 0.1f;
        float decisionTime = 0f;
        float decisionTimeExtra = 0.5f;

        float walkingEnemySpeed = 1.4f;
    }

    public void ApplyDifficultyChange(float orbSize)
    {

    }
}
