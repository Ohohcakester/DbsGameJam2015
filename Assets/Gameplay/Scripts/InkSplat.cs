using UnityEngine;
using System.Collections;

public class InkSplat : MonoBehaviour {

	private bool isFading = false;

	private float startOpacity = 1f;
	private float currOpacity;

	private float fadeSpeed = 0.01f;
	private float rCol, gCol, bCol;

	private SpriteRenderer[] sRenderers;

	public void startFading(){
		isFading = true;
		currOpacity = startOpacity;

	}

	// Use this for initialization
	void Start () {
		Vector2 camPos = Camera.main.transform.position;
		this.transform.position = OhVec.toVector3 (camPos, 0);
		this.transform.parent = Camera.main.transform;

		sRenderers = this.transform.GetComponentsInChildren<SpriteRenderer> ();
		rCol = sRenderers [0].color.r;
		gCol = sRenderers [0].color.g;
		bCol = sRenderers [0].color.b;

		startFading ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		currOpacity -= fadeSpeed;
		Color newCol = new Color (rCol, gCol, bCol, currOpacity);
		for (int i = 0; i < sRenderers.Length; i++) {
			sRenderers [i].color = newCol;
		}

		if (currOpacity <= 0.05f) {
		//	Debug.Log ("Destroy!");
			Destroy (this.gameObject);
		}
	}
}
