using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
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
