using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	private GameObject player;
	private Vector2 targetPos;
	private Vector2 offset = new Vector2 (+.9f, 0);
	public float panSpeed = 0.1f;

	public void setPlayer (GameObject plyr){
        this.player = plyr;
	}

	void Start(){
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (player != null) {
			Vector2 playerPos = player.transform.position;
			targetPos = playerPos + offset;
			this.transform.position = Vector3.Lerp(this.transform.position, OhVec.toVector3 (targetPos, -10f), panSpeed);
		}
	}
}
