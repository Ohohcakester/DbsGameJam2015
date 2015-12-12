using UnityEngine;
using System.Collections;
using Event = Orb.Event;

public class UISummoner : MonoBehaviour {

	public GameObject eventScreenPrefab;
	private GameObject evScreen;
	public GameObject currentOrbScoreScreenPrefab;
	private GameObject currOrbScoreScreen;
	public GameObject totalScoreScreenPrefab;
	private GameObject totalScoreScreen;


    private void Start()
    {
        Initialise();
    }
    
    private void Initialise()
    {
        if (evScreen != null) return;

        evScreen = Instantiate (eventScreenPrefab) as GameObject;
		evScreen.GetComponent<EventScreen>().initialize();
		evScreen.transform.parent = GameObject.Find ("Main Camera").gameObject.transform;

		currOrbScoreScreen = Instantiate (currentOrbScoreScreenPrefab) as GameObject;
		currOrbScoreScreen.transform.parent = Camera.main.transform;
		totalScoreScreen = Instantiate (totalScoreScreenPrefab) as GameObject;
		totalScoreScreen.transform.parent = Camera.main.transform;

		Camera.main.gameObject.GetComponent<GameController> ().setOrbScoreScript (currOrbScoreScreen.GetComponent<ScoreDisplay> ());
		Camera.main.gameObject.GetComponent<GameController> ().setOrbScoreAnimator (currOrbScoreScreen.GetComponent<Animator> ());
		Camera.main.gameObject.GetComponent<GameController> ().setOrbScoreOrb (currOrbScoreScreen.transform.Find("CircleSprite").gameObject);
		Camera.main.gameObject.GetComponent<GameController> ().setTotalScoreScript (totalScoreScreen.GetComponent<ScoreDisplay> ());

	}

	public void addNewEvent(Event ev){
		evScreen.GetComponent<EventScreen> ().addEventToScreen (ev);
	}

    public EventScreen getEventScreen()
    {
        Initialise();
        return evScreen.GetComponent<EventScreen>();
    }
}
