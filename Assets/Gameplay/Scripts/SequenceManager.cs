using System;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using Event = Orb.Event;
using Random = UnityEngine.Random;
using Type = OrbEventEnumerator.Event;

class SequenceManager
{
    private GameController gameController;
    private EventScreen eventScreen;
    private EnemySpawner enemySpawner;
    private SoundManager soundManager;
    public GameVariables gameVariables { get; private set; }
    

    private float startTime;
    private float endTime;
    private float stopEventsTime;
    private float nextEventHappenTime;
    private float luck;

    private float previousFrameTime;

    private List<ActiveEvent> activeEvents;

    public SequenceManager()
    {
        gameController = Camera.main.GetComponent<GameController>();
        gameVariables = new GameVariables();
        soundManager = Camera.main.GetComponent<SoundManager>();
        enemySpawner = new EnemySpawner(gameVariables);
        activeEvents = new List<ActiveEvent>();
        eventScreen = Camera.main.GetComponent<UISummoner>().getEventScreen();

        startTime = Time.time;
        endTime = Time.time + 300;
        stopEventsTime = Time.time + 220;
    }

    public void Update()
    {
        float dTime = Time.time - previousFrameTime;
        previousFrameTime = Time.time;
        RunEvents();

        if (eventScreen.getNumActiveEvents() >= EventScreen.MAX_ACTIVE_EVENT_NUM || HasMaxActiveEvents())
        {
            nextEventHappenTime += dTime;
        }
        else
        {
            UpdateEventPop();
        }

        UpdateAddEvents();

        enemySpawner.Update();
    }

    private void UpdateEventPop()
    {
        if (Time.time <= nextEventHappenTime) return;
        if (eventScreen.getCurrentSize() <= 0) return;
        Event ev = eventScreen.peekNextMaybeEvent(); // pop from queue
        if (ev == null) return;

        //Debug.Log("Run ==> " + ev.getEventType());
        if (WillEventHappenRandom(ev))
        {
            StartEvent(ev);
            eventScreen.popSuccess();
            AdjustLuck(ev.getEventType(), 1);
            if (Event.eventAlignment(ev.getEventType()) == Orb.Alignment.BAD)
            {
                soundManager.BadEventSound();
            } else
            {
                soundManager.GoodEventSound();
            }

        }
        else
        {
            // Don't Start Event
            eventScreen.removeLast();
            AdjustLuck(ev.getEventType(), -0.4f);
            soundManager.EventCancelledSound();
        }

        UpdateNextEventHappenTime();
    }


    private void UpdateNextEventHappenTime()
    {
        if (eventScreen.getCurrentSize() <= 0) return;
        Event ev = eventScreen.peekNextMaybeEvent(); // pop from queue
        nextEventHappenTime = Time.time + EventHappenDelay(ev.getEventType());
    }

    private void UpdateAddEvents()
    {
        if (eventScreen.getCurrentSize() >= EventScreen.MAX_EVENT_NUM) return;
        if (Time.time >= stopEventsTime) return;
        AddNewEvent();
    }

    private bool HasMaxActiveEvents()
    {
        return activeEvents.Count >= EventScreen.MAX_ACTIVE_EVENT_NUM;
    }

    private void RunEvents()
    {
        for (int i = 0; i < activeEvents.Count; ++i)
        {
            var activeEvent = activeEvents[i];
            if (Time.time > activeEvent.expireTime)
            {
                activeEvent.Expire();
                activeEvent.ev.expire();
            }
        }
        ClearInactiveEvents();
        ApplyBuffs();
    }

    private void ClearInactiveEvents()
    {
        activeEvents.RemoveAll(ev => ev.isExpired);
    }

    /// <summary>
    /// -infty: 0
    /// infty: 1
    /// 0: 1/2
    /// </summary>
    private float Sigmoid(float value)
    {
        return 1/(1 + Mathf.Exp(-value));
    }

    /// <summary>
    /// -infty: probability 0 of being true.
    /// infty: probability 1 of being true
    /// </summary>
    private bool SigmoidRandom(float value)
    {
        return Random.Range(0, 1f) < Sigmoid(value);
    }

    public void AddNewEvent()
    {
        OrbEventEnumerator.Event evType = OrbEventEnumerator.Event.None;

        if (SigmoidRandom(luck))
        {
            // Negative Things
            int choice = Random.Range(0, 12);
            switch (choice)
            {
                case 0:
                    evType = Type.SquidInk;
                    break;
                case 1:
                    evType = Type.FastJellyfish;
                    break;
                case 2:
                    evType = Type.MoreJellyfish;
                    break;
                case 3:
                    evType = Type.FastCrabs;
                    break;
                case 4:
                    evType = Type.MoreCrabs;
                    break;
                case 5:
                    evType = Type.UndercurrentLeft;
                    break;
                case 6:
                    evType = Type.UndercurrentRight;
                    break;
                case 7:
                    evType = Type.AggressiveJellyfish;
                    break;
                case 8:
                    evType = Type.Multiplier0_2;
                    break;
                case 9:
                    evType = Type.Multiplier0_5;
                    break;
                case 10:
                    evType = Type.LessStarlight;
                    break;
                case 11:
                    evType = Type.Multiplier0_8;
                    break;
            }

        }
        else
        {
            // Positive Things
            int choice = Random.Range(0, 8);
            switch (choice)
            {
                case 0:
                    evType = Type.BonusStarlight;
                    break;
                case 1:
                    evType = Type.Multiplier2;
                    break;
                case 2:
                    evType = Type.Multiplier3;
                    break;
                case 3:
                    evType = Type.LessJellyfish;
                    break;
                case 4:
                    evType = Type.Multiplier5;
                    break;
                case 5:
                    evType = Type.EnemiesRun;
                    break;
                case 6:
                    evType = Type.LessCrabs;
                    break;
                case 7:
                    evType = Type.MoreStarlight;
                    break;
            }
        }

        //Debug.Log("Add ==> " + evType + " | size " + eventScreen.getCurrentSize());
        if (eventScreen.getCurrentSize() <= 0)
        {
            nextEventHappenTime = Time.time + EventHappenDelay(evType);
        }
        AdjustLuck(evType, 0.4f);
        eventScreen.addEventToScreen(new Event(evType));
        
    }

    private void ApplyBuffs()
    {
        gameVariables.ResetGameBuffs();
        foreach (var activeEvent in activeEvents)
        {
            ApplyEffect(activeEvent.ev.getEventType());
        }

        gameVariables.ApplyDifficultyChange(Time.time - startTime);
    }

    private bool WillEventHappenRandom(Event ev)
    {
        return Random.Range(0, 1f)*100 < ev.probability100;
    }

    private float EventHappenDelay(OrbEventEnumerator.Event eventType)
    {
        return (float) EventHappenDelayValue(eventType) + Random.Range(-10f, 5f);
    }

    private double EventHappenDelayValue(OrbEventEnumerator.Event eventType)
    {
        switch (eventType)
        {
            case Type.BonusStarlight: return 15;
            case Type.Multiplier2: return 15;
            case Type.Multiplier3: return 15;
            case Type.LessJellyfish: return 15;
            case Type.Multiplier5: return 15;
            case Type.EnemiesRun: return 15;
            case Type.LessCrabs: return 15;
            case Type.MoreStarlight: return 15;

            case Type.SquidInk: return 10;
            case Type.FastJellyfish: return 15;
            case Type.MoreJellyfish: return 15;
            case Type.FastCrabs: return 15;
            case Type.MoreCrabs: return 15;
            case Type.UndercurrentLeft: return 10;
            case Type.UndercurrentRight: return 10;
            case Type.AggressiveJellyfish: return 20;
            case Type.Multiplier0_2: return 15;
            case Type.Multiplier0_5: return 15;
            case Type.LessStarlight: return 15;
            case Type.Multiplier0_8: return 15;
        }
        return 0;
    }

    private float EventDuration(Event ev)
    {
        return (float) EventDurationValue(ev.getEventType()) + Random.Range(-2, 2);
    }

    private double EventDurationValue(OrbEventEnumerator.Event eventType)
    {
        switch (eventType)
        {
            case Type.BonusStarlight: return 10;
            case Type.Multiplier2: return 20;
            case Type.Multiplier3: return 20;
            case Type.LessJellyfish: return 50;
            case Type.Multiplier5: return 20;
            case Type.EnemiesRun: return 13;
            case Type.LessCrabs: return 50;
            case Type.MoreStarlight: return 40;

            case Type.SquidInk: return 10;
            case Type.FastJellyfish: return 35;
            case Type.MoreJellyfish: return 35;
            case Type.FastCrabs: return 50;
            case Type.MoreCrabs: return 50;
            case Type.UndercurrentLeft: return 20;
            case Type.UndercurrentRight: return 20;
            case Type.AggressiveJellyfish: return 15;
            case Type.Multiplier0_2: return 20;
            case Type.Multiplier0_5: return 20;
            case Type.LessStarlight: return 50;
            case Type.Multiplier0_8: return 20;
        }
        return 0;
    }

    private void AdjustLuck(OrbEventEnumerator.Event ev, float scale)
    {
        luck += (float)LuckAmount(ev) * scale;
    }

    private double LuckAmount(OrbEventEnumerator.Event ev)
    {
        switch (ev)
        {
            case Type.BonusStarlight: return 2.9;
            case Type.Multiplier2: return 2;
            case Type.Multiplier3: return 3;
            case Type.LessJellyfish: return 3.7;
            case Type.Multiplier5: return 5;
            case Type.EnemiesRun: return 7.9;
            case Type.LessCrabs: return 2.1;
            case Type.MoreStarlight: return 2.4;

            case Type.SquidInk: return -1.7;
            case Type.FastJellyfish: return -4.6;
            case Type.MoreJellyfish: return -5.3;
            case Type.FastCrabs: return -1.1;
            case Type.MoreCrabs: return -2.1;
            case Type.UndercurrentLeft: return -0.2;
            case Type.UndercurrentRight: return -0.2;
            case Type.AggressiveJellyfish: return -5.1;
            case Type.Multiplier0_2: return -5;
            case Type.Multiplier0_5: return -3;
            case Type.LessStarlight: return -1.9;
            case Type.Multiplier0_8: return -2;
        }
        return 0;
    }


    private void ApplyEffect(OrbEventEnumerator.Event buff)
    {
        gameVariables.ApplyBuff(buff);
    }


    private void StartEvent(Event ev)
    {
        ActiveEvent activeEvent = new ActiveEvent(ev, (float)EventDuration(ev));
        activeEvents.Add(activeEvent);

        var buff = activeEvent.ev.getEventType();
        switch (buff)
        {
            case Type.SquidInk:
            {
                var inkSplat = CreateInkSplat();
                activeEvent.SetOnExpireFunction(() => inkSplat.startFading());
                break;
            }
            case Type.BonusStarlight:
                Camera.main.GetComponent<MazeManager>().InstantRespawnAllCollectibles();
                break;
            case Type.LessStarlight:
                Camera.main.GetComponent<MazeManager>().RandomlyDestroyCollectibles();
                break;
            case Type.UndercurrentRight:
            {
                var current = CreateCurrent(true);
                activeEvent.SetOnExpireFunction(() => MonoBehaviour.Destroy(current));
                break;
            }
            case Type.UndercurrentLeft:
            {
                var current = CreateCurrent(false);
                activeEvent.SetOnExpireFunction(() => MonoBehaviour.Destroy(current));
                break;
            }
        }
    }

    private InkSplat CreateInkSplat()
    {
        var inkSplatObject = MonoBehaviour.Instantiate(gameController.prefab_inksplat) as GameObject;
        return inkSplatObject.GetComponent<InkSplat>();
    }

    private GameObject CreateCurrent(bool facingRight)
    {
        var currentObject = MonoBehaviour.Instantiate(gameController.prefab_current) as GameObject;
        var mainCam = Camera.main.transform;

        currentObject.transform.parent = mainCam;
        currentObject.transform.position = OhVec.SetXY(currentObject.transform.position, mainCam.position.x, mainCam.position.y);

        var currentScript = currentObject.GetComponent<CurrentScript>();
        if (facingRight) currentScript.playRightCurrent();
        else currentScript.playLeftCurrent();
        return currentObject;
    }

}

internal class ActiveEvent
{
    public Event ev { get; private set; }
    public float expireTime { get; private set; }
    public bool isExpired { get; private set; }
    private Action onExpire = null;

    public ActiveEvent(Event ev, float duration)
    {
        this.ev = ev;
        this.expireTime = Time.time + duration;
        isExpired = false;
    }

    public void SetOnExpireFunction(Action action)
    {
        onExpire = action;
    }

    public void Expire()
    {
        if (onExpire != null) onExpire();
        isExpired = true;
    }
}
