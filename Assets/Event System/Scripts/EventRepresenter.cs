using UnityEngine;
using System.Collections;
using Event = Orb.Event;

public class EventRepresenter : MonoBehaviour {
	private Vector2 targetPos;
	private float speed = 0.1f;
	private Event ev;
	private bool isSelfDestructing = false;
	
	private OrbEventEnumerator.Event evType;

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
	public void includeEvent(OrbEventEnumerator.Event _event){
		evType = _event;
	}

	public OrbEventEnumerator.Event getEventType(){
		return evType;
	}
}
