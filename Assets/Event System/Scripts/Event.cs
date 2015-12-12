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
		    int dieRoll = Random.Range (0, 100);
    //		Debug.Log (dieRoll);
		    OrbEventEnumerator.Event type;

		    if (dieRoll <= 30) {
    //			Debug.Log ("Mult");
			    type = OrbEventEnumerator.Event.Multiplier;
		    } else if (dieRoll <= 50) {
    //			Debug.Log ("Meteor");
			    type = OrbEventEnumerator.Event.Meteor;
		    } else if (dieRoll <= 60) {
			    type = OrbEventEnumerator.Event.Crabby;
		    } else if (dieRoll <= 70) {
			    type = OrbEventEnumerator.Event.Jelly;
		    } else if (dieRoll <= 80) {
			    type = OrbEventEnumerator.Event.Tsunami;
		    } else if (dieRoll <= 90) {
			    type = OrbEventEnumerator.Event.Test2;
		    } else {
			    type = OrbEventEnumerator.Event.Test3;
		    }
    //		Debug.Log (dieRoll + " " + type.ToString());
		    return type;
	    }

	    public static string getEventDescription(OrbEventEnumerator.Event ev){
    //		Debug.Log (ev.ToString ());
		    if (ev == OrbEventEnumerator.Event.Multiplier) {
			    return "Multiplier";
		    } else if (ev == OrbEventEnumerator.Event.Meteor) {
			    return "Meteor";
		    } else if (ev == OrbEventEnumerator.Event.Crabby) {
			    return "Crabby";
		    } else if (ev == OrbEventEnumerator.Event.Jelly) {
			    return "Jelly";
		    } else if (ev == OrbEventEnumerator.Event.Tsunami) {
			    return "Tsunami";
		    } else if (ev == OrbEventEnumerator.Event.Test2) {
			    return "Test2";
		    } else if (ev == OrbEventEnumerator.Event.Test3) {
			    return "Test3";
		    } else {
			    return "Unfound";
		    }
	    }

    }
}
