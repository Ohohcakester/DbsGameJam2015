using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using Event = Orb.Event;

class SequenceManager
{
    private GameController gameController;
    public GameVariables gameVariables { get; private set; }

    private float startTime;
    private float nextEventHappenTime;
    private float luck;

    private float previousFrameTime;

    private List<ActiveEvent> activeEvents;

    public SequenceManager()
    {
        gameController = Camera.main.GetComponent<GameController>();
        gameVariables = new GameVariables();
    }

    public void Update()
    {
        float dTime = Time.time - previousFrameTime;
        previousFrameTime = Time.time;
        RunEvents();

        if (HasMaxEvents())
        {
            nextEventHappenTime += dTime;
        }
        else
        {
            if (Time.time > nextEventHappenTime)
            {
                Event ev = null; // pop from queue
                if (WillEventHappenRandom(ev.getEventType()))
                {
                    StartEvent(ev);
                    AdjustLuck(ev.getEventType());
                }
                else
                {
                    // Don't Start Event
                }

            }
        }
    }

    private bool HasMaxEvents()
    {
        return activeEvents.Count >= 3;
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
        if (SigmoidRandom(luck))
        {
            // Negative Things
            
        }
        else
        {
            // Positive Things

        }
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
