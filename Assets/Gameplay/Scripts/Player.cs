using UnityEngine;
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
        gameVariables = Camera.main.GetComponent<GameController>().GetGameVariables();

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

				if (Input.GetKey (KeyCode.LeftArrow)) {
					vx -= WalkSpeed;
					FaceDirection (true);
				}
				if (Input.GetKey (KeyCode.RightArrow)) {
					vx += WalkSpeed;
					FaceDirection (false);
				}
				if (Input.GetKeyDown (KeyCode.Space)) {
					if (IsOnPlatform ()) {
						Jump ();
					}
				}

				if (Input.GetKeyDown (KeyCode.LeftControl) && IsOnPlatform () && (gController.getCurrentOrbScore () != 0)) {
					isConsuming = true;
					hasFinishedConsuming = false;
					startConsumingTime = Time.time;
					vx = 0;
					anim.Play ("FishTryingToEat");
				}

				if (Mathf.Abs (rigidbody2D.velocity.x) <= 0.1f) {
					anim.Play ("FishIdle");
				} else if (Mathf.Abs (rigidbody2D.velocity.x) >= 0) {
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
			anim.Play ("FishEat"); //replace with stunned animation
			if (Time.time >= stunEndTime) {
				isStunned = false;
				gController.resetOrbScore ();
				RegenerateOrb ();
			}
		}

		if (noTarget) {
			if (Time.time >= noTargetEndTime) {
				noTarget = false;
			}
		}
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
	    gController.resetOrbScore();
		isConsuming = false;
		orbCtrl.Gone ();
	}

	public void RegenerateOrb(){
		orbCtrl.setOriginalSize ();
	}

	public void UpdateOrbSize(int size){
	    orbCtrl.setSize(orbSize(size));
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
