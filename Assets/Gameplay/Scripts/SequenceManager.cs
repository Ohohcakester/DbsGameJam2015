using UnityEngine;
using System.Collections.Generic;

class SequenceManager
{
    private EventStatus eventStatus;
    private GameController gameController;


    public SequenceManager()
    {
        if (eventStatus != null) return;
        eventStatus = new EventStatus();
        gameController = Camera.main.GetComponent<GameController>();
    }

    public void Update()
    {

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
