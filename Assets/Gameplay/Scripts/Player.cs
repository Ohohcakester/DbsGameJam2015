using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private const float WalkSpeed = 5;
    private const float JumpSpeed = 8;
    private Rigidbody2D rigidbody2D;
    private PlayerGroundCollider groundCollider;

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
    }

    // Update is called once per frame
	void Update ()
	{
	    float vx = 0;

	    if (Input.GetKey(KeyCode.LeftArrow))
	    {
	        vx -= WalkSpeed;
	    }
        if (Input.GetKey(KeyCode.RightArrow))
	    {
	        vx += WalkSpeed;
	    }
        if (Input.GetKeyDown(KeyCode.Space))
	    {
	        if (IsOnPlatform())
	        {
	            Jump();
	        }
	    }

	    rigidbody2D.velocity = OhVec.SetX(rigidbody2D.velocity, vx);
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
