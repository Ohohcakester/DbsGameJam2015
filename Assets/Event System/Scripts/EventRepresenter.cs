using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Event = Orb.Event;

public class EventRepresenter : MonoBehaviour {
	private Vector2 targetPos;
	private float speed = 15f;
	private Event ev;
	private bool isSelfDestructing = false;
	Animator anim;

	float ay = -0.004f;
	float vy;

	private bool isLerping = true;
	
	private OrbEventEnumerator.Event evType;

	// Update is called once per frame
	void Update () {
		Vector2 currPos = this.transform.localPosition;

		if (isLerping) {
			this.transform.localPosition = Vector2.Lerp (currPos, targetPos, speed*Time.deltaTime);

		} else {
//			Debug.Log ("Not lerping : " + currPos.y);
			this.transform.localPosition = new Vector3 (currPos.x,  (currPos.y + vy), -3f);
			vy += ay;
		}

		if (isSelfDestructing) {
			if ((currPos - targetPos).magnitude <= 0.3f) {
				Destroy (this.gameObject);
			}
		}
//		Debug.Log (this.transform.localPosition.z);
	}

	void Start(){
		anim = this.transform.GetComponent<Animator> ();
	}

	public void playSuccessAnim(){
		anim.Play ("EventSuccess");
	}

	public void setProbText(string text){
		this.transform.Find ("ProbabilityText").GetComponentInChildren<Text> ().text = text;
	}

	public void playFailureAnim(){
		anim.Play ("EventFail");
		setProbText("X");
		Vector2 currPos = this.transform.localPosition;
		Vector2 offset = new Vector2 (0, -10f);
		Vector2 despawnPos = currPos + offset;
		moveTowardsThenDestroySelf (despawnPos);
		isLerping = false;
		vy = -0.01f;
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
