using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    private GameVariables gameVariables;
	public GameObject collectibleSpriteObjPrefab;
	private bool isCollected = false;
	private GameObject collectibleSpriteObj;
    private CollectibleSpriteAnimation collectibleSpriteAnimation;
	private float nextRespawnTime;
	public int lightAmount = 1;

	private void OnTriggerEnter2D(Collider2D col)
	{
	    initialize();

		//Debug.Log (col.ToString ());
		if (!isCollected && col.gameObject.tag == "Player") {
			nextRespawnTime = Time.time + RespawnTime ();
			isCollected = true;
            collectibleSpriteAnimation.chasePlayer(col.gameObject, lightAmount);

			Player plyr = col.GetComponent<Player> ();
			if (plyr != null) {
				plyr.GrowOrb (lightAmount);
			} else {
				//			Debug.Log ("Cant retrieve plyr");
				plyr = col.transform.parent.GetComponent<Player> ();
				//			Debug.Log ("Successfully retrieved" + plyr.ToString ());
				plyr.GrowOrb (lightAmount);
			}

			//	Camera.main.gameObject.GetComponent<GameController> ().addOrbScore (1);
			//		GameObject.Find ("TotalScoreScreen(Clone)").GetComponent<ScoreDisplay> ().addScore (1);
		} else {
			if (!isCollected) {
				//Debug.Log (col.gameObject.tag);
			}
		}
	}

	public void initialize()
	{
	    if (collectibleSpriteObj != null) return;

		collectibleSpriteObj = Instantiate (collectibleSpriteObjPrefab, this.transform.position, this.transform.rotation) as GameObject;
	    collectibleSpriteAnimation = collectibleSpriteObj.GetComponent<CollectibleSpriteAnimation>();
        collectibleSpriteAnimation.stopChasing();
	    gameVariables = Camera.main.GetComponent<GameController>().GetGameVariables();
	}

	void Start(){
		initialize ();
	}

	void Update(){
		if (isCollected && Time.time >= nextRespawnTime) {
			collectibleSpriteObj.transform.position = this.transform.position;
			collectibleSpriteObj.SetActive (true);
			isCollected = false;
            collectibleSpriteAnimation.stopChasing();
		}
	}

    private float RespawnTime()
    {
        return gameVariables.itemRespawnTime + Random.Range(-3f, 3f);
    }

    public void InstantRespawn()
    {
        if (isCollected) nextRespawnTime = Time.time;
    }

    public void Vanish()
    {
        nextRespawnTime = Time.time + RespawnTime();
        isCollected = true;
        collectibleSpriteAnimation.Disable();
    }
}
