﻿using UnityEngine;
using System.Collections;
using Random = OhRandom;

public class GameController : MonoBehaviour
{
    [SerializeField] public GameObject prefab_inksplat;
    [SerializeField] public GameObject prefab_current;

    ScoreDisplay orbScoreScript;
	ScoreDisplay totalScoreScript;
	Animator orbScoreAnimator;
	GameObject orbScoreOrb;
    private SequenceManager sequenceManager;

    private int orbScoreTextValue = int.MinValue;

	int currentOrbScore = 0;
	int currentTotalScore = 0;

    private float lastUpdateTime;
    private float currentValue;
    private float incrementValue = 50f;
    private GameVariables gameVariables;

    private void Start()
    {
        Initialise();
    }

	public int getCurrentOrbScore(){
		return currentOrbScore;
	}

    private void Initialise()
    {
        if (sequenceManager != null) return;
        sequenceManager = new SequenceManager();
        gameVariables = sequenceManager.gameVariables;
    }

    private void Update()
    {
        sequenceManager.Update();
        ScoreIncreaseUpdate();
        updateOrbScoreText();
    }

    private void ScoreIncreaseUpdate()
    {
        float dTime = Time.time - lastUpdateTime;
        lastUpdateTime = Time.time;

        currentValue += dTime * scoreAccumulationRate();

        while (currentValue > incrementValue)
        {
            currentOrbScore++;
            currentValue -= incrementValue;
        }
    }

    private float scoreAccumulationRate()
    {
        // Depends on orb score
        return currentOrbScore * gameVariables.multiplier;
    }

    public GameVariables GetGameVariables()
    {
        Initialise();
        return gameVariables;
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
		orbScoreAnimator.Play ("OrbCircleActivate");
	}

	void updateOrbScoreText()
	{
	    if (orbScoreTextValue == currentOrbScore) return;
	    orbScoreTextValue = currentOrbScore;

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
		updateTotalScoreText ();
	}

	public void resetOrbScore() {
		currentOrbScore = 0;
	}
}
