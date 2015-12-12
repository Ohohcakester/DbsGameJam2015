using System;
using System.Collections.Generic;

public class EventStatus
{
    public Dictionary<OrbEventEnumerator.Event, bool> status { get; private set; }
    public List<OrbEventEnumerator.Event> events;


    public EventStatus()
    {
        status = new Dictionary<OrbEventEnumerator.Event, bool>();
        events = new List<OrbEventEnumerator.Event>();

        foreach (OrbEventEnumerator.Event key in Enum.GetValues(typeof(OrbEventEnumerator.Event)))
        {
            status.Add(key, false);
            events.Add(key);
        }
    }


}
