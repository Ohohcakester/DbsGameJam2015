﻿using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using Event = Orb.Event;

class SequenceManager
{
    private GameController gameController;
    private EventScreen eventScreen;
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
    }

    private void UpdateEventPop()
    {
        if (Time.time <= nextEventHappenTime) return;
        if (eventScreen.getCurrentSize() <= 0) return;
        Event ev = eventScreen.peekNextMaybeEvent(); // pop from queue
        if (ev == null) return;

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
            }
        }
    }

    private void StartEvent(Event ev)
    {
        ActiveEvent activeEvent = new ActiveEvent(ev, EventDuration(ev));
        activeEvents.Add(activeEvent);
    }

    private float EventDuration(Event ev)
    {
        return 10f;
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

    private bool WillEventHappenRandom(OrbEventEnumerator.Event ev)
    {
        return Random.Range(0, 1f) < 0.5f;
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
        eventScreen.addEventToScreen(new Event(evType));
    }

    public void AdjustLuck(OrbEventEnumerator.Event ev)
    {
        switch (ev)
        {
        }
        luck += 0;
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
