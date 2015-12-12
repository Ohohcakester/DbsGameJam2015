using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const float WalkSpeed = 5;
    private const float JumpSpeed = 10;
    private Rigidbody2D rigidbody2D;
    private PlayerGroundCollider groundCollider;

    private Transform playerSprite;

	GameController gController;
	private bool isConsuming = false;
	private float startConsumingTime;
	private float consumeDelay = 2.0f;

    public bool facingRight { get; private set; }

    // Use this for initialization
	void Start ()
	{
	    Initialise();
	}

    private void Initialise()
    {
        if (rigidbody2D != null) return;
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundCollider = transform.FindChild("GroundCollider").GetComponent<PlayerGroundCollider>();
		gController = Camera.main.GetComponent<GameController> ();
        playerSprite = transform.FindChild("PlayerSprite");
    }

    // Update is called once per frame
	void Update ()
	{
		if (!isConsuming) {
			float vx = 0;

			if (Input.GetKey (KeyCode.LeftArrow)) {
				vx -= WalkSpeed;
			    FaceDirection(true);
			}
			if (Input.GetKey (KeyCode.RightArrow)) {
                vx += WalkSpeed;
                FaceDirection(false);
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (IsOnPlatform ()) {
					Jump ();
				}
			}


			if (Input.GetKeyDown (KeyCode.LeftControl) && IsOnPlatform ()) {
				isConsuming = true;
				startConsumingTime = Time.time;
				vx = 0;
			}
			rigidbody2D.velocity = OhVec.SetX (rigidbody2D.velocity, vx);

		} else if (Time.time - startConsumingTime >= consumeDelay) {
			isConsuming = false;
			gController.transferOrbScoreToTotal ();
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
}
