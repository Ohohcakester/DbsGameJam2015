using UnityEngine;
using System.Collections;

public class DynamicBackground : MonoBehaviour {
	Vector2 playerStartPos;
    private MazeManager mazeManager;
	public Player player;
    // Use this for initialization

    void Start () {
		//player = GameObject.Find ("Player(Clone)").gameObject;
		playerStartPos = new Vector2 (0,0);
        //playerStartPos = player.transform.position;
        mazeManager = Camera.main.GetComponent<MazeManager>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (mazeManager == null) return;
	    //var currPlayerPos = mazeManager.PlayerPosition();
		Vector2 currPlayerPos = Camera.main.transform.position;
		float yDisp = (currPlayerPos.y - playerStartPos.y) * 0.01f;
		float xDisp = (currPlayerPos.x - playerStartPos.x) * 0.1f;

		Vector2 newPos = new Vector2 ((currPlayerPos.x - xDisp), (currPlayerPos.y + yDisp));

		this.transform.position = newPos;
	}
}
