using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    private GameVariables gameVariables;
    private GameController gameController;
	public GameObject collectibleSpriteObjPrefab;
	private bool isCollected = false;
	private GameObject collectibleSpriteObj;
    private CollectibleSpriteAnimation collectibleSpriteAnimation;
	private float nextRespawnTime;
	private int lightAmount = 10;

	private void OnTriggerEnter2D(Collider2D col)
	{
	    initialize();

		//Debug.Log (col.ToString ());
		if (!isCollected && col.gameObject.tag == "Player") {
			nextRespawnTime = Time.time + RespawnTime ();
			isCollected = true;
            collectibleSpriteAnimation.chasePlayer(col.gameObject);

			Player plyr = col.GetComponent<Player> ();
		    if (plyr != null)
		    {
		        CollectedByPlayer(plyr, lightAmount);
		    }
		    else
		    {
		        //			Debug.Log ("Cant retrieve plyr");
		        plyr = col.transform.parent.GetComponent<Player>();
		        //			Debug.Log ("Successfully retrieved" + plyr.ToString ());
		        CollectedByPlayer(plyr, lightAmount);
		    }

		    //	Camera.main.gameObject.GetComponent<GameController> ().addOrbScore (1);
			//		GameObject.Find ("TotalScoreScreen(Clone)").GetComponent<ScoreDisplay> ().addScore (1);
		} else {
			if (!isCollected) {
				//Debug.Log (col.gameObject.tag);
			}
		}
	}

    private void CollectedByPlayer(Player player, int points)
    {
        player.GrowOrb(points);
        gameController.addOrbScore((int)(points * gameVariables.multiplier));

    }

    public void initialize()
	{
	    if (collectibleSpriteObj != null) return;

		collectibleSpriteObj = Instantiate (collectibleSpriteObjPrefab, this.transform.position, this.transform.rotation) as GameObject;
	    collectibleSpriteAnimation = collectibleSpriteObj.GetComponent<CollectibleSpriteAnimation>();
        collectibleSpriteAnimation.stopChasing();
        gameController = Camera.main.GetComponent<GameController>();
        gameVariables = gameController.GetGameVariables();
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
