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

	public void addEventRepToScreen(Event ev){
		GameObject testEventObj = Instantiate (eventObjRep) as GameObject;
		ev.configureUIEventObject (testEventObj);
		testEventObj.transform.parent = this.transform;
		testEventObj.transform.localPosition = locArray [currentEventIndex];
		eventObjs.Add (testEventObj);
		currentEventIndex++;
	}

	public void addAndPop(Event ev){
		GameObject testEventObj = Instantiate (eventObjRep, locArray [0], this.transform.rotation) as GameObject;
//		Debug.Log (testEventObj.transform.localPosition.ToString ());
		ev.configureUIEventObject (testEventObj);
		testEventObj.transform.parent = this.transform;
		testEventObj.GetComponent<EventRepresenter> ().setPos (locArray [0]);


		for (int i = 0; i < eventObjs.Count; i++) {
			if (i != MAX_EVENT_NUM - 1) {
				//eventObjs [i].transform.localPosition = locArray [i + 1];
		//		eventObjs [i].transform.localPosition = locArray [i + 1];
				eventObjs [i].GetComponent<EventRepresenter> ().setPos (locArray [i + 1]);
			}			
		}

		if (eventObjs.Count == MAX_EVENT_NUM) {
			Destroy (eventObjs [5]);
			eventObjs.RemoveAt (5);
		}
		eventObjs.Insert (0, testEventObj);

		testEventObj.transform.localPosition = locArray [0];
	}


}
