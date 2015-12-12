using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using Event = Orb.Event;

class SequenceManager
{
    private GameController gameController;
    private EventScreen eventScreen;
    private EnemySpawner enemySpawner;
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
        if (WillEventHappenRandom(ev.getEventType()))
        {
            StartEvent(ev);
            eventScreen.popSuccess();
            AdjustLuck(ev.getEventType());
        }
        else
        {
            // Don't Start Event
            eventScreen.removeLast();
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
    }

    private void StartEvent(Event ev)
    {
        ActiveEvent activeEvent = new ActiveEvent(ev, EventDuration(ev));
        activeEvents.Add(activeEvent);
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
        OrbEventEnumerator.Event evType = OrbEventEnumerator.Event.Test3;

        if (SigmoidRandom(luck))
        {
            // Negative Things
            int choice = Random.Range(0, 2);
            switch (choice)
            {
                case 0: evType = OrbEventEnumerator.Event.Crabby; break;
                case 1: evType = OrbEventEnumerator.Event.Jelly; break;
            }
        }
        else
        {
            // Positive Things
            int choice = Random.Range(0, 2);
            switch (choice)
            {
                case 0: evType = OrbEventEnumerator.Event.Meteor; break;
                case 1: evType = OrbEventEnumerator.Event.Multiplier; break;
            }
        }
        //Debug.Log("Add ==> " + evType + " | size " + eventScreen.getCurrentSize());
        if (eventScreen.getCurrentSize() <= 0)
        {
            nextEventHappenTime = Time.time + EventHappenDelay(evType);
        }
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

    private bool WillEventHappenRandom(OrbEventEnumerator.Event ev)
    {
        return Random.Range(0, 1f) < 0.5f;
    }

    private float EventHappenDelay(OrbEventEnumerator.Event eventType)
    {
        return 1.5f;
    }

    private float EventDuration(Event ev)
    {
        return 3.5f;
    }

    private void AdjustLuck(OrbEventEnumerator.Event ev)
    {
        switch (ev)
        {
        }
        luck += 0;
    }

    private void ApplyEffect(OrbEventEnumerator.Event buff)
    {

    }


    private void StartEffect(OrbEventEnumerator.Event buff)
    {

    }

}

internal class ActiveEvent
{
    public Event ev { get; private set; }
    public float expireTime { get; private set; }
    public bool isExpired { get; private set; }

    public ActiveEvent(Event ev, float duration)
    {
        this.ev = ev;
        this.expireTime = Time.time + duration;
        isExpired = false;
    }

    public void Expire()
    {
        isExpired = true;
    }
}
