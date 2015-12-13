using UnityEngine;
using System.Collections;
using Random = OhRandom;

public class GameController : MonoBehaviour
{
    [SerializeField] public GameObject prefab_inksplat;
	[SerializeField] public GameObject prefab_current;
	[SerializeField] public GameObject endGameScreen;
    public bool gameOver { get; private set; }


    ScoreDisplay orbScoreScript;
	ScoreDisplay totalScoreScript;
	ScoreDisplay timeLeftScript;
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
        if (Time.timeScale < 0.1f) Time.timeScale = 1;
        sequenceManager = new SequenceManager();
        gameVariables = sequenceManager.gameVariables;
    }

    private void Update()
    {
        sequenceManager.Update();
        ScoreIncreaseUpdate();
        updateOrbScoreText();
		checkForTime ();
		checkForEscape ();
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

	public void setTimeLeftScript(ScoreDisplay script){
		timeLeftScript = script;
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

	void checkForTime(){
        timeLeftScript.updateTime(sequenceManager.RemainingTime);

		if (!gameOver && sequenceManager.ActualRemainingTime <= 0) {
			endGame ();
		}
	}

	void endGame()
	{
	    gameOver = true;
		Time.timeScale = 0;
		GameObject screen = Instantiate (endGameScreen, this.transform.position, this.transform.rotation) as GameObject;
		screen.GetComponent<ScoreDisplay> ().setFinalScore (currentTotalScore);
		screen.transform.parent = this.transform;
	}

	void checkForEscape(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			UnityEngine.SceneManagement.SceneManager.LoadScene ("startscreen");
		}
	}
}
