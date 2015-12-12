using UnityEngine;
using System.Collections;

public class DynamicBackground : MonoBehaviour {
	private Vector2 targetPos;
	GameObject player;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 currPlayerPos = player.transform.position;
		Vector2 currPos = this.transform.position;

//		targetPos = Vector2.Lerp (currPos, currPlayerPos);
	//	this.transform.position 
	}
}
