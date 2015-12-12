using UnityEngine;
using System.Collections.Generic;
using Event = Orb.Event;

class SequenceManager
{
    private EventStatus eventStatus;
    private GameController gameController;
    public GameVariables gameVariables { get; private set; }

    private float startTime;
    private float nextEventHappenTime;
    private float luck;

    public SequenceManager()
    {
        eventStatus = new EventStatus();
        gameController = Camera.main.GetComponent<GameController>();
        gameVariables = new GameVariables();
    }

    public void Update()
    {
        /*if (Time.time > nextEventHappenTime)
        {
            Event ev;
        }*/
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

    private void ResetGameBuffs()
    {
        
    }

    private void ApplyBuffs()
    {
        ResetGameBuffs();
        foreach (var buff in eventStatus.events)
        {
            if (eventStatus.status[buff])
            {
                Apply(buff);
            }
        }
    }

    private void Apply(OrbEventEnumerator.Event buff)
    {
        
    }

}
