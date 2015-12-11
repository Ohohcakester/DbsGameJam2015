using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	public GameObject collectibleSpriteObjPrefab;
	private bool isCollected = false;
	private GameObject collectibleSpriteObj;
	private float lastCollectedTime;
	private float baseRespawnTime = 10.0f;
	private float nextRespawnTime;
	private float lastRespawnTime;

	private void OnTriggerEnter2D(Collider2D col){
		if (!isCollected && col.tag == "Player") {
			lastCollectedTime = Time.time;
			nextRespawnTime = Time.time + Random.Range (baseRespawnTime - (baseRespawnTime * 0.3f), baseRespawnTime + (baseRespawnTime * 0.3f));
			isCollected = true;
		//	collectibleSpriteObj.SetActive (false);
			collectibleSpriteObj.GetComponent<CollectibleSpriteAnimation>().chasePlayer(col.gameObject);

			Camera.main.gameObject.GetComponent<GameController> ().addOrbScore (1);
	//		GameObject.Find ("TotalScoreScreen(Clone)").GetComponent<ScoreDisplay> ().addScore (1);
		}
	}

	public void initialize(){
		collectibleSpriteObj = Instantiate (collectibleSpriteObjPrefab, this.transform.position, this.transform.rotation) as GameObject;
		collectibleSpriteObj.GetComponent<CollectibleSpriteAnimation> ().stopChasing ();
	}

	void Start(){
		initialize ();
	}

	void Update(){
		if (isCollected && Time.time >= nextRespawnTime) {
			collectibleSpriteObj.transform.position = this.transform.position;
			collectibleSpriteObj.SetActive (true);
			isCollected = false;
			collectibleSpriteObj.GetComponent<CollectibleSpriteAnimation> ().stopChasing ();
		}
	}
}
