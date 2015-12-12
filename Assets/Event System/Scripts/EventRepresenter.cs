using UnityEngine;
using System.Collections;

public class EventRepresenter : MonoBehaviour {
	private Vector2 targetPos;
	private float speed = 0.1f;
	private OrbEventEnumerator.Event ev;

	// Update is called once per frame
	void Update () {
		Vector2 currPos = this.transform.localPosition;
		this.transform.localPosition = Vector2.Lerp (currPos, targetPos, speed);
	}

	public void setPos(Vector2 pos){
		targetPos = pos;
	}

	public void includeEvent(OrbEventEnumerator.Event _event){
		ev = _event;
	}

	public OrbEventEnumerator.Event getEvent(){
		return ev;
	}
}
