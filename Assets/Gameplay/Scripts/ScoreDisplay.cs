using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
	Text scoreText;
	int currScore = 0;
	MazeManager mazeMngr;
	Vector2 targetPos;
	private float speed = 0.5f;
	private bool isOrb = false;
	Vector2 displacement = new Vector2 (0, 0.6f);
	GameObject orbScoreObj;

	// Use this for initialization
    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        if (scoreText != null) return;

        scoreText = this.transform.Find ("TextCanvas").GetComponentInChildren<Text> ();
	//	Debug.Log (scoreText.ToString ());
		mazeMngr = Camera.main.GetComponent<MazeManager>();
		if (this.gameObject.name.Equals ("CurrentOrbScoreScreen(Clone)")) {
			isOrb = true;
		}
	}

	public void updateScore(int newScore)
	{
	    Initialise();
//		scoreText = this.transform.Find ("TextCanvas").GetComponentInChildren<Text> ();
//		Debug.Log (scoreText.ToString ());
		scoreText.text = newScore.ToString();
	}

	public void addScore(int score){
		currScore += score;
//		Debug.Log (currScore);
		scoreText.text = currScore.ToString();

	}

	public void setFinalScore(int newScore){
		scoreText = this.transform.Find ("TextCanvas").GetComponentInChildren<Text> ();
		scoreText.text = newScore.ToString();
	}

	void FixedUpdate(){
		if (isOrb) {
			targetPos = mazeMngr.PlayerPosition () + displacement;
			Vector2 currPos = this.transform.position;
			this.transform.position = Vector2.Lerp (currPos, targetPos, speed);
		}
	}

	public void updateTime(float time){
		int seconds = (int)time;
		int minutes = seconds / 60;
		seconds = seconds % 60;

	    if (time < 0)
	    {
	        seconds = 0;
	        minutes = 0;
	    }

	    string strSeconds = "" + seconds;
        while (strSeconds.Length <= 1) strSeconds = "0" + strSeconds;

		string strRep = minutes + "." + strSeconds;
	//	Debug.Log (strRep);
		scoreText.text = strRep;
	}
}
