using UnityEngine;
using System.Collections;

public class DynamicBackground : MonoBehaviour {
	Vector2 playerStartPos;
	public Player player;
    // Use this for initialization

    void Start () {
		//player = GameObject.Find ("Player(Clone)").gameObject;
        //playerStartPos = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
        {
            //player = GameObject.Find("Player(Clone)").gameObject;
            player = Camera.main.GetComponent<MazeManager>().player;
            playerStartPos = player.transform.position;
        }
        else
        {
            Vector2 currPlayerPos = player.transform.position;
            Vector2 displacement = playerStartPos - currPlayerPos;
            displacement *= 0.5f;
            this.transform.position = displacement - new Vector2(10f, 10f);
        }
//		targetPos = Vector2.Lerp (currPos, currPlayerPos);
	//	this.transform.position 
	}
}
