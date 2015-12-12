using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Orb {
    public class Event{
	    public string eventName;
	    public string eventDescription;


	    private OrbEventEnumerator.Event eventType;
	    public bool hasExpired { get; private set; }

	    //Constructor
	    public Event (OrbEventEnumerator.Event ev) {
		    eventType = ev;
		    eventDescription = getEventDescription(eventType);
		    eventName = eventDescription;
		    hasExpired = false;
	    }

	    public void expire(){
		    hasExpired = true;
	    }

	    public Event() {
		    eventType = getRandomEvent ();
    //		int randomEventIndex = Random.Range (0, eventNamesArray.Length);
    //		eventName = eventNamesArray [randomEventIndex];
    //		eventDescription = eventNamesArray [randomEventIndex];
    //		Debug.Log(getEventDescription(eventType));
		    eventDescription = getEventDescription(eventType);
		    eventName = eventDescription;
		    hasExpired = false;
	    }

	    public void configureUIEventObject(GameObject eventObj) {
    //		Debug.Log ("Configuring" + eventDescription);
		    eventObj.transform.Find ("TextCanvas").GetComponentInChildren<Text> ().text = eventDescription;
	    }

    //	public void 

	    public OrbEventEnumerator.Event getEventType() {
		    return this.eventType;
	    }

	    public OrbEventEnumerator.Event getRandomEvent(){
		    return OrbEventEnumerator.Event.None;
	    }

	    public static string getEventDescription(OrbEventEnumerator.Event ev)
	    {
	        //return ev.ToString();

	        switch (ev)
	        {
                case OrbEventEnumerator.Event.BonusStarlight: return "Treasure!";
                case OrbEventEnumerator.Event.Multiplier2: return "2x Orb\nMultiplier";
                case OrbEventEnumerator.Event.Multiplier3: return "3x Orb\nMultiplier";
                case OrbEventEnumerator.Event.LessJellyfish: return "Less\nJellyfish";
                case OrbEventEnumerator.Event.Multiplier5: return "5x Orb\nMultiplier";
                case OrbEventEnumerator.Event.EnemiesRun: return "Terrified\nJellyfish";
                case OrbEventEnumerator.Event.LessCrabs: return "Less\nCrabs";
                case OrbEventEnumerator.Event.MoreStarlight: return "Abundant\nStarlight";

                case OrbEventEnumerator.Event.SquidInk: return "Squid\nSplatter";
                case OrbEventEnumerator.Event.FastJellyfish: return "Speedy\nJellyfish";
                case OrbEventEnumerator.Event.MoreJellyfish: return "Jellyfish\nSeason";
                case OrbEventEnumerator.Event.FastCrabs: return "Speedy\nCrabs";
                case OrbEventEnumerator.Event.MoreCrabs: return "Crab\nSeason";
                case OrbEventEnumerator.Event.UndercurrentLeft: return "Fast\nCurrent";
                case OrbEventEnumerator.Event.UndercurrentRight: return "Fast\nCurrent";
                case OrbEventEnumerator.Event.AggressiveJellyfish: return "They can\nsee you";
                case OrbEventEnumerator.Event.Multiplier0_2: return "0.2x Orb\nMultiplier";
                case OrbEventEnumerator.Event.Multiplier0_5: return "0.5x Orb\nMultiplier";
                case OrbEventEnumerator.Event.LessStarlight: return "Starlight\nScarcity";
                case OrbEventEnumerator.Event.Multiplier0_8: return "0.8x Orb\nMultiplier";
	        }
	        return ev.ToString();
	    }

    }
}
