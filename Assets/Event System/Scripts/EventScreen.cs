using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventScreen : MonoBehaviour {
	private int MAX_EVENT_NUM = 6;

	private Vector2[] locArray = new Vector2[6];
	public GameObject eventObjRep;
	private int currentEventIndex = 0;
	private List<GameObject> eventObjs = new List<GameObject>();


	// Use this for initialization
	public void initialize () {
		for (int i = 0; i < locArray.Length; i++) {
			string posObjName = "Loc" + i;
			locArray [i] = this.transform.Find (posObjName).transform.localPosition;
			//Debug.Log (this.transform.Find (posObjName).transform.localPosition.ToString ());
		}

	}

	public void addEventToScreen(Event ev){
		int currIndex = eventObjs.Count;
		GameObject testEventObj = Instantiate (eventObjRep) as GameObject;
		ev.configureUIEventObject (testEventObj);
		testEventObj.transform.parent = this.transform;
		testEventObj.GetComponent<EventRepresenter> ().setPos (locArray [currIndex]);
		eventObjs.Insert (0, testEventObj);
	}

	/*
	public OrbEventEnumerator.Event addAndPop(Event ev){
		GameObject testEventObj = Instantiate (eventObjRep, locArray [0], this.transform.rotation) as GameObject;
		ev.configureUIEventObject (testEventObj);
		testEventObj.transform.parent = this.transform;
		testEventObj.GetComponent<EventRepresenter> ().setPos (locArray [0]);


		for (int i = 0; i < eventObjs.Count; i++) {
			if (i != MAX_EVENT_NUM - 1) {
				eventObjs [i].GetComponent<EventRepresenter> ().setPos (locArray [i + 1]);
			}			
		}


		eventObjs.Insert (0, testEventObj);

		testEventObj.transform.localPosition = locArray [0];

		//	if (eventObjs.Count == MAX_EVENT_NUM + 1) {

		int lastIndex = eventObjs.Count;
		OrbEventEnumerator.Event returnEvent = eventObjs [lastIndex ].GetComponent<EventRepresenter> ().getEvent ();
		Destroy (eventObjs [lastIndex ]);
		eventObjs.RemoveAt (lastIndex );
		return returnEvent;
	//	}
	}
*/
	public OrbEventEnumerator.Event pop(){
		int lastIndex = eventObjs.Count;
		OrbEventEnumerator.Event returnEvent = eventObjs [lastIndex ].GetComponent<EventRepresenter> ().getEvent ();
		Destroy (eventObjs [lastIndex ]);
		eventObjs.RemoveAt (lastIndex );
		return returnEvent;
	}

	public void removeLast(){
		int currSize = eventObjs.Count;
		Destroy (eventObjs [currSize]);
		eventObjs.RemoveAt (currSize);

		for (int i = 0; i < eventObjs.Count; i++) {
			if (i != MAX_EVENT_NUM - 1) {
				eventObjs [i].GetComponent<EventRepresenter> ().setPos (locArray [i + 1]);
			}			
		}

	}

	public int getCurrentSize()	{
		return eventObjs.Count;
	}


}
