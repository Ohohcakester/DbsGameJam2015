using UnityEngine;
using System.Collections;

public class EnemyThieving : MonoBehaviour {
	public float stunTime = 2.0f;

	void OnCollisionEnter2D(Collision2D col){
		Debug.Log (col.gameObject.tag);
		if (col.gameObject.tag == "Player") {
			
			Player plyr = col.gameObject.GetComponent<Player> ();
			if (plyr != null) {
				Debug.Log ("Stunning player");
				plyr.stun (2.0f);
				plyr.DestroyOrb ();
			} else {
				Debug.Log ("Collided with non-player");
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		Debug.Log (col.gameObject.tag);
		if (col.gameObject.tag == "Player") {

			Player plyr = col.gameObject.GetComponent<Player> ();
			if (plyr != null) {
				Debug.Log ("Stunning player");
				plyr.stun (2.0f);
				plyr.DestroyOrb ();
			} else {
				Debug.Log ("Collided with non-player");
			}
		}
	}

}
