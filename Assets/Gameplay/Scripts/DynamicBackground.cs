using UnityEngine;
using System.Collections;

public class DynamicBackground : MonoBehaviour {
	Vector2 playerStartPos;
    private MazeManager mazeManager;
	public Player player;
    // Use this for initialization

    void Start () {
		//player = GameObject.Find ("Player(Clone)").gameObject;
        //playerStartPos = player.transform.position;
        mazeManager = Camera.main.GetComponent<MazeManager>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (mazeManager == null) return;
	    var currPlayerPos = mazeManager.PlayerPosition();
        Vector2 displacement = playerStartPos - currPlayerPos;
        displacement *= 0.5f;
	    this.transform.position = -displacement;
	}
}
