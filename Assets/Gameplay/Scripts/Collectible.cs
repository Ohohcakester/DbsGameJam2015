using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    private GameVariables gameVariables;
	public GameObject collectibleSpriteObjPrefab;
	private bool isCollected = false;
	private GameObject collectibleSpriteObj;
	private float nextRespawnTime;
	public int lightAmount = 1;

    private Vector3 pos1;
    private Vector3 pos2;
    private bool flicker;

	private void OnTriggerEnter2D(Collider2D col){
		if (!isCollected && col.tag == "Player") {
		    nextRespawnTime = Time.time + RespawnTime();
			isCollected = true;
			collectibleSpriteObj.GetComponent<CollectibleSpriteAnimation>().chasePlayer(col.gameObject, lightAmount);

//			Debug.Log (col.ToString ());
			Player plyr = col.GetComponent<Player> ();
			if (plyr != null) {
				plyr.GrowOrb (lightAmount);
			}

		//	Camera.main.gameObject.GetComponent<GameController> ().addOrbScore (1);
	//		GameObject.Find ("TotalScoreScreen(Clone)").GetComponent<ScoreDisplay> ().addScore (1);
		}
	}

	public void initialize()
	{
	    if (collectibleSpriteObj != null) return;

		collectibleSpriteObj = Instantiate (collectibleSpriteObjPrefab, this.transform.position, this.transform.rotation) as GameObject;
		collectibleSpriteObj.GetComponent<CollectibleSpriteAnimation> ().stopChasing ();
	    gameVariables = Camera.main.GetComponent<GameController>().GetGameVariables();

	    pos1 = this.transform.position;
	    pos2 = pos1 + new Vector3(0, 10000f, 0);
	}

	void Start(){
		initialize ();
	}

    private void Update()
    {
        if (isCollected && Time.time >= nextRespawnTime)
        {
            collectibleSpriteObj.transform.position = pos1;
            collectibleSpriteObj.SetActive(true);
            isCollected = false;
            collectibleSpriteObj.GetComponent<CollectibleSpriteAnimation>().stopChasing();
        }
    }

    void FixedUpdate() {
        if (!isCollected)       
        {
            if (flicker) transform.position = pos1;
            else transform.position = pos2;
            flicker = !flicker;
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
}
