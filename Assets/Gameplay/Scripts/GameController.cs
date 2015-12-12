using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	ScoreDisplay orbScoreScript;
	ScoreDisplay totalScoreScript;
	Animator orbScoreAnimator;
	GameObject orbScoreOrb;
    private SequenceManager sequenceManager;
		
	int currentOrbScore = 0;
	int currentTotalScore = 0;

    private void Start()
    {
        sequenceManager = new SequenceManager();
    }

    private void Update()
    {
        sequenceManager.Update();
    }

    public void setOrbScoreScript (ScoreDisplay script){
		orbScoreScript = script;
//		Debug.Log (orbScoreScript.ToString ());
	}

	public void setOrbScoreAnimator(Animator anim){
		orbScoreAnimator = anim;
	}

	public void setTotalScoreScript (ScoreDisplay script){
		totalScoreScript = script;
	}


	public void setOrbScoreOrb(GameObject orb){
		orbScoreOrb = orb;
	}

	public void addOrbScore(int score) {
		currentOrbScore += score;
		updateOrbScoreText ();
		orbScoreAnimator.Play ("OrbCircleActivate");
	}

	void updateOrbScoreText() {
		orbScoreScript.updateScore (currentOrbScore);
		setOrbScoreSize ();
	}

	void updateTotalScoreText() {
		totalScoreScript.updateScore (currentTotalScore);
	}

	void setOrbScoreSize(){
		float ratio = currentOrbScore/100.0f + 1;
		float size = Mathf.Clamp (ratio, 1, 1.5f);
		orbScoreOrb.transform.localScale = new Vector2 (size, size);
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
