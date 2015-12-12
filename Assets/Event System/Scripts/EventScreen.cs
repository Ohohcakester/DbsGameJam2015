using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Event = Orb.Event;

public class EventScreen : MonoBehaviour {
    public const int MAX_EVENT_NUM = 6;
    public const int MAX_ACTIVE_EVENT_NUM = 3;

	private Vector2[] locArray = new Vector2[6];
	private Vector2[] buffLocArray = new Vector2[4];
//	private bool[] isActiveSpaceFree = { true, true, true }; 

	public GameObject eventObjRep;
	private List<GameObject> eventObjs = new List<GameObject>();

	private GameObject[] activeEventObjs = new GameObject[3];


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
		
		//Debug.Log ((activeEventObjs [0] == null) + "," + (activeEventObjs [1] == null) + "," + (activeEventObjs [2] == null));
		
		for (int i = 0; i < activeEventObjs.Length; ++i) {
			var activeEvent = activeEventObjs [i];
			if (activeEvent == null) continue;
			if (!activeEvent.GetComponent<EventRepresenter> ().getEvent ().hasExpired) continue;

			activeEvent.GetComponent<EventRepresenter> ().moveTowardsThenDestroySelf (buffLocArray [3]);
			activeEventObjs [i] = null;
		}

		//Debug.Log (getNumActiveEvents ());
		/*foreach (var key in activeEventObjs) {
			if (key != null && key.GetComponent<EventRepresenter> ().getEvent ().hasExpired) {
				key.GetComponent<EventRepresenter> ().moveTowardsThenDestroySelf (buffLocArray [3]);
				Debug.Log ("Destroying an active buff");
				Destroy (key);
			}
		}*/

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
//		Debug.Log (eventObjs.Count);
		int currIndex = eventObjs.Count;
		for (int i = 0; i < currIndex; i++) {
			eventObjs [i].GetComponent<EventRepresenter> ().setPos (locArray [i+1]);
		}

		Vector2 currPos = this.transform.position;
		Vector2 offset = new Vector2 (0, -20f);
		Vector2 spawnPos = currPos + offset;
		GameObject testEventObj = Instantiate (eventObjRep, spawnPos, this.transform.rotation) as GameObject;
		ev.configureUIEventObject (testEventObj);
		testEventObj.transform.parent = this.transform;
		testEventObj.GetComponent<EventRepresenter> ().setPos (locArray [0]);
		testEventObj.GetComponent<EventRepresenter> ().includeEvent(ev);
		eventObjs.Insert (0, testEventObj);
	}
    
	public Event popSuccess(){
		//Debug.Log ("Pop success : " + Time.time);
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
		int lastIndex = eventObjs.Count - 1;
		int index = -1;
		for (int i = 0; i < 3; i++) {
			if (activeEventObjs [i] == null) {
				index = i;
				activeEventObjs [i] = eventObjs[lastIndex];
				break;
			}
		}

		if (index != -1) {
			//	Debug.Log (index);
			eventObjs [lastIndex].GetComponent<EventRepresenter> ().setPos (buffLocArray [index]);
		} else {
			Debug.Log ("Too many active buffs!");
		}

//		eventObjs [lastIndex].GetComponent<EventRepresenter> ().setPos (buffLocArray [0]);
		eventObjs.RemoveAt (lastIndex );

	}

	/*public void shiftActiveEvents()
	{
		for (int i = 0; i < activeEventObjs.Length; i++) {
			activeEventObjs [i].GetComponent<EventRepresenter> ().setPos (buffLocArray [i + 1]);
		}
	}*/

	/*public void removeLastActive(){
		Debug.Log (activeEventObjs.Length);
		GameObject lastActive = activeEventObjs [activeEventObjs.Length - 1];
//		Debug.Log (lastActive.ToString ());
		activeEventObjs.Remove (lastActive);
		lastActive.GetComponent<EventRepresenter> ().moveTowardsThenDestroySelf (buffLocArray [3]);
	//	Destroy (lastActive);
	}*/

	public void removeLast(){
		//Debug.Log ("Removed Last : " + Time.time);
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
		int totalActive = 0;
		for (int i = 0; i < 3; i++) {
			if (activeEventObjs [i] != null) {
				totalActive++;
			}
		}
		return totalActive;
    }

}
