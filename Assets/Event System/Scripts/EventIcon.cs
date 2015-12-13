using UnityEngine;
using System.Collections;
using Type = OrbEventEnumerator.Event;

public class EventIcon : MonoBehaviour
{
    [SerializeField] private Sprite[] iconSprites;


    public void SetSprite(Type ev)
    {
        transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = iconSprites[spriteIndex(ev)];
    }

    private int spriteIndex(Type ev)
    {
        switch (ev)
        {
            case OrbEventEnumerator.Event.Multiplier2: return 0;
            case OrbEventEnumerator.Event.Multiplier3: return 1;
            case OrbEventEnumerator.Event.Multiplier5: return 2;
            case OrbEventEnumerator.Event.BonusStarlight: return 3;
            case OrbEventEnumerator.Event.LessJellyfish: return 4;
            case OrbEventEnumerator.Event.EnemiesRun: return 5;
            case OrbEventEnumerator.Event.LessCrabs: return 6;
            case OrbEventEnumerator.Event.MoreStarlight: return 7;

            case OrbEventEnumerator.Event.SquidInk: return 8;
            case OrbEventEnumerator.Event.FastJellyfish: return 9;
            case OrbEventEnumerator.Event.MoreJellyfish: return 10;
            case OrbEventEnumerator.Event.FastCrabs: return 11;
            case OrbEventEnumerator.Event.MoreCrabs: return 12;
            case OrbEventEnumerator.Event.UndercurrentLeft: return 13;
            case OrbEventEnumerator.Event.UndercurrentRight: return 14;
            case OrbEventEnumerator.Event.AggressiveJellyfish: return 15;
            case OrbEventEnumerator.Event.LessStarlight: return 16;
            case OrbEventEnumerator.Event.Multiplier0_2: return 17;
            case OrbEventEnumerator.Event.Multiplier0_5: return 18;
            case OrbEventEnumerator.Event.Multiplier0_8: return 19;
        }
        return -1;
    }

}
