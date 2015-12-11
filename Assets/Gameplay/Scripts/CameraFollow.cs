using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	private GameObject player;
	private Vector2 targetPos;
	public float panSpeed = 0.1f;

	public void setPlayer (GameObject plyr){
		player = plyr;
	}

	void Start(){
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (player != null) {
			targetPos = player.transform.position;
			this.transform.position = Vector3.Lerp(this.transform.position, OhVec.toVector3 (targetPos, -10f), Time.timeScale*panSpeed);
		}
	}
}
