using UnityEngine;
using System.Collections.Generic;

class SequenceManager
{
    private EventStatus eventStatus;
    private GameController gameController;
    public GameVariables gameVariables { get; private set; }

    public SequenceManager()
    {
        eventStatus = new EventStatus();
        gameController = Camera.main.GetComponent<GameController>();
        gameVariables = new GameVariables();
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
