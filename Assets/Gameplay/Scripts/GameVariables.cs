using UnityEngine;
using System.Collections.Generic;

public class GameVariables
{
    public const bool DEBUG_MODE = true;

    public float baseDetectionRange;
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
        nEnemies = 1;
        nCrabs = 2;
        multiplier = 1f;
        undercurrent = 0f;
        itemRespawnTime = 100f;

        turnSpeed = 7.7f;
        moveSpeed = 3f;
        baseAggressiveness = 0.05f;
        decisionTime = 0f;
        decisionTimeExtra = 0.5f;

        walkingEnemySpeed = 0.9f;
    }

    public void ApplyDifficultyChange(int orbScore)
    {
        baseDetectionRange += orbScore/200f;
        float increaseAggressiveness = Mathf.Max(orbScore/20000f,0.4f);
        baseAggressiveness += increaseAggressiveness;
    }

    public void ApplyBuff(OrbEventEnumerator.Event buff)
    {
        switch (buff)
        {
            case OrbEventEnumerator.Event.Multiplier2:
                multiplier *= 2;
                break;
            case OrbEventEnumerator.Event.Multiplier3:
                multiplier *= 3;
                break;
            case OrbEventEnumerator.Event.Multiplier5:
                multiplier *= 5;
                break;
            case OrbEventEnumerator.Event.Multiplier0_2:
                multiplier *= 0.2001f;
                break;
            case OrbEventEnumerator.Event.Multiplier0_5:
                multiplier *= 0.5001f;
                break;
            case OrbEventEnumerator.Event.Multiplier0_8:
                multiplier *= 0.8001f;
                break;
            case OrbEventEnumerator.Event.MoreJellyfish:
                nEnemies += 3;
                break;
            case OrbEventEnumerator.Event.MoreCrabs:
                nCrabs += 8;
                break;
            case OrbEventEnumerator.Event.LessJellyfish:
                nEnemies -= 4;
                break;
            case OrbEventEnumerator.Event.LessCrabs:
                nCrabs -= 10;
                break;
            case OrbEventEnumerator.Event.UndercurrentLeft:
                undercurrent -= 1.1f;
                break;
            case OrbEventEnumerator.Event.UndercurrentRight:
                undercurrent += 1.1f;
                break;
            case OrbEventEnumerator.Event.EnemiesRun:
                moveSpeed *= -1;
                break;
            case OrbEventEnumerator.Event.LessStarlight:
                itemRespawnTime += 50f;
                break;
            case OrbEventEnumerator.Event.MoreStarlight:
                itemRespawnTime -= 90f;
                break;
            case OrbEventEnumerator.Event.AggressiveJellyfish:
                baseAggressiveness = 0.3f;
                break;
            case OrbEventEnumerator.Event.FastJellyfish:
                if (moveSpeed > 0) moveSpeed = 3.7f;
                if (moveSpeed < 0) moveSpeed = -3.7f;
                break;
            case OrbEventEnumerator.Event.FastCrabs:
                walkingEnemySpeed = 1.9f;
                break;
        }
    }
}
