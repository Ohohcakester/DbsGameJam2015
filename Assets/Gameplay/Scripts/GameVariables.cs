using UnityEngine;
using System.Collections.Generic;

public class GameVariables
{
    public float baseDetectionRange = 10;
    public float baseDetectionDistance = 10;
    public int nEnemies = 5;
    public int nCrabs = 5;
    public float multiplier = 1f;
    public float undercurrent = 0f;
    public float itemRespawnRate;

    public float turnSpeed = 7.7f;
    public float moveSpeed = 6f;
    public float baseAggressiveness = 0.5f;
    public float decisionTime = 0f;
    public float decisionTimeExtra = 0.5f;

    public float walkingEnemySpeed = 1.4f;

    public void ResetGameBuffs()
    {

    }

    public void ApplyDifficultyChange(float timePassed)
    {

    }
}
