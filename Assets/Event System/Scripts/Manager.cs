using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
//	public GameObject eventObjRep;
	public GameObject eventScreenPrefab;
	private GameObject evScreen;
	public GameObject currentOrbScoreScreenPrefab;
	private GameObject currOrbScoreScreen;
	public GameObject totalScoreScreenPrefab;
	private GameObject totalScoreScreen;

	public float newEventBaseCooldown = 3.0f;
	private float lastNewEventAddTime = 0;
	private float timeToNextEvent = 0;

	// Use this for initialization
	void Start () {
		evScreen = Instantiate (eventScreenPrefab) as GameObject;
		evScreen.GetComponent<EventScreen>().initialize();
		evScreen.transform.parent = GameObject.Find ("Main Camera").gameObject.transform;

		currOrbScoreScreen = Instantiate (currentOrbScoreScreenPrefab) as GameObject;
		totalScoreScreen = Instantiate (totalScoreScreenPrefab) as GameObject;

//		Camera.main.gameObject.AddComponent<GameController> ();
		Camera.main.gameObject.GetComponent<GameController> ().setOrbScoreScript (currOrbScoreScreen.GetComponent<ScoreDisplay> ());
		Camera.main.gameObject.GetComponent<GameController> ().setTotalScoreScript (totalScoreScreen.GetComponent<ScoreDisplay> ());


		/*
		for (int i = 0; i <= 5; i++) {
			//Event testEvent = new Event("Instance " + i);
			Event testEvent = new Event();
			evScreen.GetComponent<EventScreen>().addEventRepToScreen(testEvent);
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.F1)) {
			Event testEvent = new Event();
			evScreen.GetComponent<EventScreen>().addAndPop(testEvent);
		}

		if (Time.time - lastNewEventAddTime >= timeToNextEvent) {
			float range = newEventBaseCooldown / 2;
			timeToNextEvent = Random.Range (newEventBaseCooldown + range, newEventBaseCooldown - range);
			lastNewEventAddTime = Time.time;

			addNewEvent ();
		}

		if (Input.GetKeyDown (KeyCode.F2)) {
			Camera.main.gameObject.GetComponent<GameController> ().transferOrbScoreToTotal ();
		}

		if (Input.GetKeyDown (KeyCode.F3)) {
			Camera.main.gameObject.GetComponent<GameController> ().resetOrbScore ();
		}
	}

	void addNewEvent(){
		Event newEv = new Event ();
		evScreen.GetComponent<EventScreen>().addAndPop(newEv);
	}


}
