using UnityEngine;
using System.Collections;

public class CurrentScript : MonoBehaviour {
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = transform.GetComponent<Animator>();
	}

	public void playLeftCurrent(){
//		Debug.Log ("left curr");
		anim.Play ("LeftwardsCurrent");
	}
	public void playRightCurrent(){
//		Debug.Log ("right curr");
		anim.Play ("RightwardsCurrent");
	}
}
