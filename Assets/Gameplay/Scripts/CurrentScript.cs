using UnityEngine;
using System.Collections;

public class CurrentScript : MonoBehaviour {
	Animator anim;

	// Use this for initialization
	void Initialise ()
	{
	    if (anim != null) return;
		anim = transform.GetComponent<Animator>();
	}

	public void playLeftCurrent()
	{
	    Initialise();
//		Debug.Log ("left curr");
		anim.Play ("LeftwardsCurrent");
	}

    public void playRightCurrent()
    {
        Initialise();
//		Debug.Log ("right curr");
        anim.Play("RightwardsCurrent");
    }
}
