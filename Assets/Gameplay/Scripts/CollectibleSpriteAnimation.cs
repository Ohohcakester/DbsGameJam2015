using UnityEngine;
using System.Collections;

public class CollectibleSpriteAnimation : MonoBehaviour {
	private float maxSpeed = 5.0f;
	private float currSpeed = 0f;
	private bool isChasing = false;
	private GameObject player;

	public void chasePlayer(GameObject plyr){
		isChasing = true;
		currSpeed = 0;
		player = plyr;
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 10.0f);
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isChasing) {
			Vector2 playerPos = player.transform.position;
			Vector2 currPos = this.transform.position;
			Vector2 posDiff = playerPos - currPos;
			if (posDiff.magnitude > 0.3f) {
				this.transform.position = Vector2.Lerp (currPos, playerPos, Time.timeScale * currSpeed);
				currSpeed = Mathf.Lerp (currSpeed, maxSpeed, Time.timeScale * 0.005f);
			} else {
				this.gameObject.SetActive (false);
			}
		}
	}

	public void stopChasing(){
		isChasing = false;
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0f);

	}

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
