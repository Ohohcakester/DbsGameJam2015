using UnityEngine;
using System.Collections;

public class EventRepresenter : MonoBehaviour {
	private Vector2 targetPos;
	private float speed = 0.1f;

	// Update is called once per frame
	void Update () {
		Vector2 currPos = this.transform.localPosition;
		this.transform.localPosition = Vector2.Lerp (currPos, targetPos, speed);
	}

	public void setPos(Vector2 pos){
		targetPos = pos;
	}
}
