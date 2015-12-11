using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Event{
	public string eventName;
	public string eventDescription;

	//Constructor
	public Event (string name){
		eventName = name;
		if (name == "Test") {
			eventDescription = "Test Display Text";
		}
	}

	public void configureUIEventObject(GameObject eventObj){
		eventObj.transform.Find ("TextCanvas").GetComponentInChildren<Text> ().text = eventDescription;
	}


}
