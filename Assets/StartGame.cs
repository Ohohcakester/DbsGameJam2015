using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Use this for initialization
    void Start()
    {
        if (Time.timeScale < 0.1f) Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GameScene()
    {
        Application.LoadLevel("testscene");
    }
    public void CreditsScene()
    {
        Application.LoadLevel("credits");
    }
}
