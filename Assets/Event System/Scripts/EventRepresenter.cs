using UnityEngine;
using System.Collections;

public class EventRepresenter : MonoBehaviour {
	private Vector2 targetPos;
	private float speed = 0.1f;
	private Event ev;
	private bool isSelfDestructing = false;

	// Update is called once per frame
	void Update () {
		Vector2 currPos = this.transform.localPosition;
		this.transform.localPosition = Vector2.Lerp (currPos, targetPos, speed);

		if (isSelfDestructing) {
			if ((currPos - targetPos).magnitude <= 0.3f) {
				Destroy (this.gameObject);
			}
		}
	}

	public void setPos(Vector2 pos){
		targetPos = pos;
	}

	public void includeEvent(Event _event){
		ev = _event;
	}

	public void moveTowardsThenDestroySelf(Vector2 pos){
		isSelfDestructing = true;
		targetPos = pos;
	}

	public Event getEvent(){
		return ev;
	}
}
