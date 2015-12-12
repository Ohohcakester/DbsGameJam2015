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
	void Start () {
		scoreText = this.transform.Find ("TextCanvas").GetComponentInChildren<Text> ();
//		Debug.Log (scoreText.ToString ());
		mazeMngr = Camera.main.GetComponent<MazeManager>();
		if (this.gameObject.name.Equals ("CurrentOrbScoreScreen(Clone)")) {
			isOrb = true;
		}
	}

	public void updateScore(int newScore){
		string score = newScore.ToString ();
		scoreText.text = score;
	}

	public void addScore(int score){
		currScore += score;
		Debug.Log (currScore);
		scoreText.text = currScore.ToString();

	}

	void FixedUpdate(){
		if (isOrb) {
			targetPos = mazeMngr.PlayerPosition () + displacement;
			Vector2 currPos = this.transform.position;
			this.transform.position = Vector2.Lerp (currPos, targetPos, speed);
		}
	}
}
