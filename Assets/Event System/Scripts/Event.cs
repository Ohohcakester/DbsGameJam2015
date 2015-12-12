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
	        return ev.ToString();

	        /*switch (ev)
	        {

                case OrbEventEnumerator.Event.BonusStarlight: return 2.9;
                case OrbEventEnumerator.Event.Multiplier2: return 2;
                case OrbEventEnumerator.Event.Multiplier3: return 3;
                case OrbEventEnumerator.Event.LessJellyfish: return 3.7;
                case OrbEventEnumerator.Event.Multiplier5: return 5;
                case OrbEventEnumerator.Event.EnemiesRun: return 7.9;
                case OrbEventEnumerator.Event.LessCrabs: return 2.1;
                case OrbEventEnumerator.Event.MoreStarlight: return 2.4;

                case OrbEventEnumerator.Event.SquidInk: return -1.7;
                case OrbEventEnumerator.Event.FastJellyfish: return -4.6;
                case OrbEventEnumerator.Event.MoreJellyfish: return -5.3;
                case OrbEventEnumerator.Event.FastCrabs: return -1.1;
                case OrbEventEnumerator.Event.MoreCrabs: return -2.1;
                case OrbEventEnumerator.Event.UndercurrentLeft: return -1.4;
                case OrbEventEnumerator.Event.UndercurrentRight: return -1.4;
                case OrbEventEnumerator.Event.AggressiveJellyfish: return -5.1;
                case OrbEventEnumerator.Event.Multiplier0_2: return -5;
                case OrbEventEnumerator.Event.Multiplier0_5: return -3;
                case OrbEventEnumerator.Event.LessStarlight: return -1.9;
                case OrbEventEnumerator.Event.Multiplier0_8: return -2;
	        }*/
	    }

    }
}
