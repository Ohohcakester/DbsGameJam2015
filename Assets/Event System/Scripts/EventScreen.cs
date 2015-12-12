using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Event = Orb.Event;

public class EventScreen : MonoBehaviour {
    public const int MAX_EVENT_NUM = 6;
    public const int MAX_ACTIVE_EVENT_NUM = 3;

	private Vector2[] locArray = new Vector2[6];
	private Vector2[] buffLocArray = new Vector2[4];
	private bool[] isActiveSpaceFree = { true, true, true }; 
	public GameObject eventObjRep;
	private int currentEventIndex = 0;
	private List<GameObject> eventObjs = new List<GameObject>();

	private List<GameObject> activeEventObjs = new List<GameObject> ();


	// Use this for initialization
	public void initialize () {
		for (int i = 0; i < locArray.Length; i++) {
			string posObjName = "Loc" + i;
			locArray [i] = this.transform.Find (posObjName).transform.localPosition;
			//Debug.Log (this.transform.Find (posObjName).transform.localPosition.ToString ());
		}

		for (int i = 0; i < buffLocArray.Length; i++) {
			string posObjName = "BuffLoc" + i;
			buffLocArray [i] = this.transform.Find (posObjName).transform.localPosition;
			//Debug.Log (this.transform.Find (posObjName).transform.localPosition.ToString ());
		}
	}

	void Update(){
		for (int i=0; i<activeEventObjs.Count; i++){
			if (activeEventObjs[i] != null && activeEventObjs [i].GetComponent<EventRepresenter> ().getEvent ().hasExpired) {
				activeEventObjs [i].GetComponent<EventRepresenter> ().moveTowardsThenDestroySelf (buffLocArray [3]);
				isActiveSpaceFree [i] = true;
			}
		}

		if (Input.GetKeyDown (KeyCode.F1)) {
			Event ev = new Event ();
			if (getCurrentSize () < 6) {
				addEventToScreen (ev);
			} else {
				popSuccess ();
				addEventToScreen (ev);
			}
		}
	}

	public void addEventToScreen(Event ev){
//		Debug.Log ("Adding new Event " + ev.ToString ());
		int currIndex = eventObjs.Count;
		for (int i = 0; i < currIndex; i++) {
			eventObjs [i].GetComponent<EventRepresenter> ().setPos (locArray [i+1]);
		}

		Vector2 currPos = this.transform.position;
		Vector2 offset = new Vector2 (0, -1f);
		Vector2 spawnPos = currPos + offset;
		GameObject testEventObj = Instantiate (eventObjRep, spawnPos, this.transform.rotation) as GameObject;
		ev.configureUIEventObject (testEventObj);
		testEventObj.transform.parent = this.transform;
		testEventObj.GetComponent<EventRepresenter> ().setPos (locArray [0]);
		testEventObj.GetComponent<EventRepresenter> ().includeEvent(ev);
		eventObjs.Insert (0, testEventObj);
	}
    
	public Event popSuccess(){
		Debug.Log ("Pop success");
		int lastIndex = eventObjs.Count-1;
		Event returnEvent = eventObjs [lastIndex].GetComponent<EventRepresenter> ().getEvent ();

		setLatestEventToActive ();
		return returnEvent;
	}

    public Event peekNextMaybeEvent()
    {
        int lastIndex = eventObjs.Count - 1;
        return eventObjs[lastIndex].GetComponent<EventRepresenter>().getEvent();
    }

	public void setLatestEventToActive(){
		int index = -1;
		for (int i = 0; i < 3; i++) {
			if (isActiveSpaceFree [i]) {
				index = i;
				isActiveSpaceFree [i] = false;
				break;
			}
		}

		int lastIndex = eventObjs.Count - 1;
		if (index != -1) {
			//	Debug.Log (index);
			eventObjs [lastIndex].GetComponent<EventRepresenter> ().setPos (buffLocArray [index]);
		} else {
			Debug.Log ("Too many active buffs!");
		}

//		eventObjs [lastIndex].GetComponent<EventRepresenter> ().setPos (buffLocArray [0]);
		activeEventObjs.Add (eventObjs [lastIndex]);
		eventObjs.RemoveAt (lastIndex );

	}

	public void shiftActiveEvents()
	{
		for (int i = 0; i < activeEventObjs.Count; i++) {
			activeEventObjs [i].GetComponent<EventRepresenter> ().setPos (buffLocArray [i + 1]);
		}
	}

	public void removeLastActive(){
		Debug.Log (activeEventObjs.Count);
		GameObject lastActive = activeEventObjs [activeEventObjs.Count - 1];
//		Debug.Log (lastActive.ToString ());
		activeEventObjs.Remove (lastActive);
		lastActive.GetComponent<EventRepresenter> ().moveTowardsThenDestroySelf (buffLocArray [3]);
	//	Destroy (lastActive);
	}

	public void removeLast(){
		Debug.Log ("Remove last");
		int currSize = eventObjs.Count;
		Destroy (eventObjs [currSize-1]);
		eventObjs.RemoveAt (currSize-1);

		for (int i = 0; i < eventObjs.Count; i++) {
			if (i != MAX_EVENT_NUM - 1) {
				eventObjs [i].GetComponent<EventRepresenter> ().setPos (locArray [i + 1]);
			}			
		}

	}

	public int getCurrentSize()	{
//		Debug.Log ("Size is " + eventObjs.Count);
		return eventObjs.Count;
	}

    public int getNumActiveEvents()
    {
        return activeEventObjs.Count;
    }

}
