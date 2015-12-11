using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	ScoreDisplay orbScoreScript;
	ScoreDisplay totalScoreScript;

	int currentOrbScore = 0;
	int currentTotalScore = 0;

	public void setOrbScoreScript (ScoreDisplay script){
		orbScoreScript = script;
//		Debug.Log (orbScoreScript.ToString ());
	}

	public void setTotalScoreScript (ScoreDisplay script){
		totalScoreScript = script;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addOrbScore(int score) {
		currentOrbScore += score;
		updateOrbScoreText ();
	}

	void updateOrbScoreText() {
		orbScoreScript.updateScore (currentOrbScore);
	}

	void updateTotalScoreText() {
		totalScoreScript.updateScore (currentTotalScore);
	}

	public void transferOrbScoreToTotal() {
		currentTotalScore += currentOrbScore;
		currentOrbScore = 0;
		updateOrbScoreText ();
		updateTotalScoreText ();
	}

	public void resetOrbScore() {
		currentOrbScore = 0;
		updateOrbScoreText ();
	}
}
