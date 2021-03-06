﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const float NOTARGET_TIME = 5f;

    private const float WalkSpeed = 3;
    private const float JumpSpeed = 7;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    private PlayerGroundCollider groundCollider;
	private Animator anim;
    private GameVariables gameVariables;

    private bool noTarget;
    public bool NoTarget
    {
        get { return noTarget || gController.getCurrentOrbScore() <= 0; }
    }

    private float noTargetEndTime;
	public bool isStunned { get; private set;}
	private float stunEndTime;

    private Vector2 colliderWidth1;
    private Vector2 colliderWidth2;
    private bool flicker;

    private Transform playerSprite;

	GameController gController;
	private bool isConsuming = false;
	private bool hasFinishedConsuming = false;
	private float startConsumingTime;
	private float consumeDelay = 2.0f;

    public bool facingRight { get; private set; }
	public bool isOrbDestroyed { get; private set; }
	private OrbController orbCtrl;
    private GameController gameController;
    private int lastUpdatedOrbSize = int.MinValue;

    // Use this for initialization
	void Start ()
	{
	    Initialise();
	}

    private void Initialise()
    {
        if (rigidbody2D != null) return;
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        groundCollider = transform.FindChild("GroundCollider").GetComponent<PlayerGroundCollider>();
		gController = Camera.main.GetComponent<GameController> ();
        playerSprite = transform.FindChild("PlayerSprite");

        colliderWidth1 = boxCollider2D.size;
        colliderWidth2 = OhVec.ScaleX(boxCollider2D.size, 0.985f);
		anim = transform.Find("PlayerSprite").transform.GetComponent<Animator> ();
        orbCtrl = transform.GetComponentInChildren<OrbController>();
        gameController = Camera.main.GetComponent<GameController>();
        gameVariables = gameController.GetGameVariables();

		isStunned = false;
		noTarget = false;
    }

    // Update is called once per frame
	void Update ()
	{
        if (flicker) boxCollider2D.size = colliderWidth1;
        else boxCollider2D.size = colliderWidth2;
        flicker = !flicker;

		if (!isStunned) {
			if (!isConsuming) {
				float vx = gameVariables.undercurrent;

				float horzAxis = Input.GetAxis ("Horizontal");

				if (Input.GetKey (KeyCode.LeftArrow) || horzAxis < 0) {
					vx -= WalkSpeed;
					FaceDirection (true);
				}
				if (Input.GetKey (KeyCode.RightArrow) || horzAxis > 0) {
					vx += WalkSpeed;
					FaceDirection (false);
				}
				if (Input.GetKeyDown (KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton2)) {
					if (IsOnPlatform ()) {
						Jump ();
					}
				}

				if ((Input.GetKeyDown (KeyCode.LeftControl) || Input.GetKey(KeyCode.JoystickButton3)) && IsOnPlatform () && (gController.getCurrentOrbScore () != 0)) {
					isConsuming = true;
					hasFinishedConsuming = false;
					startConsumingTime = Time.time;
					vx = 0;
				}

				if (vx == 0f) {
					if (!isConsuming) {
						anim.Play ("FishIdle");
					} else {
						anim.Play ("FishTryingToEat");
					}
				} else {
					anim.Play ("FishWalk");
				}


				rigidbody2D.velocity = OhVec.SetX (rigidbody2D.velocity, vx);

			} else if (Time.time - startConsumingTime >= consumeDelay && !hasFinishedConsuming) {
				hasFinishedConsuming = true;
				anim.Play ("FishEat");
				gController.transferOrbScoreToTotal ();
			} else if (hasFinishedConsuming && Time.time - startConsumingTime >= consumeDelay + .3f) {
				isConsuming = false;
				hasFinishedConsuming = true;
				RegenerateOrb ();
			}
		} else {
			rigidbody2D.velocity = OhVec.SetX (rigidbody2D.velocity, 0);
			anim.Play ("EnemySteal");
			if (Time.time >= stunEndTime) {
				isStunned = false;
				RegenerateOrb ();
			}
		}

		if (noTarget) {
			if (Time.time >= noTargetEndTime) {
				noTarget = false;
			}
		}

	    UpdateOrbSize();
	}

    private void FaceDirection(bool right)
    {
        facingRight = right;

        if ((playerSprite.localScale.x < 0) != right)
            playerSprite.localScale = OhVec.FlipX(playerSprite.localScale);
    }

    private bool IsOnPlatform()
    {
        return groundCollider.OnGround;
    }

    private void Jump()
    {
        rigidbody2D.velocity = OhVec.SetY(rigidbody2D.velocity, JumpSpeed);
    }

	public void DestroyOrb()
	{
		isConsuming = false;
		gController.resetOrbScore ();
		orbCtrl.Gone ();
	}

	public void RegenerateOrb(){
		orbCtrl.setOriginalSize ();
	}

	private void UpdateOrbSize()
	{
	    if (lastUpdatedOrbSize == gameController.getCurrentOrbScore()) return;
	    lastUpdatedOrbSize = gameController.getCurrentOrbScore();
	    orbCtrl.setSize(orbSize(lastUpdatedOrbSize));
	}

    public float orbSize(int size)
    {
        return size/100f;
    }



    public void stun(float time){
		isStunned = true;
		noTarget = true;
		stunEndTime = Time.time + time;
		noTargetEndTime = Time.time + NOTARGET_TIME + time;
	}

}
