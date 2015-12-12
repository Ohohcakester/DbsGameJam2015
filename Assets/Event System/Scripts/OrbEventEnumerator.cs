using UnityEngine;
using System.Collections;

public class OrbEventEnumerator
{

    public enum Event
    {
        None,

        // Buff
        Multiplier2,
        Multiplier3,
        Multiplier5,
        Multiplier0_2,
        Multiplier0_5,
        Multiplier0_8,
        MoreJellyfish,
        MoreCrabs,
        LessJellyfish,
        LessCrabs,
        UndercurrentLeft,
        UndercurrentRight,
        EnemiesRun,
        LessStarlight,
        MoreStarlight,
        AggressiveJellyfish,
        FastJellyfish,
        FastCrabs,

        // Trigger
        SquidInk,
        BonusStarlight,
    };
}
