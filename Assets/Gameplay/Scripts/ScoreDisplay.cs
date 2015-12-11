using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {
	Text scoreText;
	int currScore = 0;

	// Use this for initialization
	void Start () {
		scoreText = this.transform.Find ("TextCanvas").GetComponentInChildren<Text> ();
//		Debug.Log (scoreText.ToString ());
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
}
