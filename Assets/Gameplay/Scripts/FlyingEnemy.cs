using UnityEngine;
using System.Collections;

public class FlyingEnemy : MonoBehaviour
{
    private float currentAngle;
    private float turnSpeed = 5.7f;
    private float targetAngle;
    private bool isTurning;

    private float moveSpeed = 3f;

    private Rigidbody2D rigidbody2D;

    private MazeManager mazeManager;

    private float nextTurnTime;

    private float aggressiveness;

	// Use this for initialization
	void Start ()
	{
	    Initialise();
	}

    private void Initialise()
    {
        if (rigidbody2D != null) return;
        rigidbody2D = GetComponent<Rigidbody2D>();
        mazeManager = Camera.main.GetComponent<MazeManager>();
        aggressiveness = 1f;
    }


    public void TurnTowards(float angle)
    {
        angle = NormaliseAngle(angle);
        targetAngle = angle;
        isTurning = true;
    }

    /// <summary>
    /// -180 <= angle < 180
    /// </summary>
    private float NormaliseAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle <= -180) angle += 360;
        return angle;
    }

    // Update is called once per frame
	void Update ()
	{
	    UpdateAI();

	    float diff = targetAngle - currentAngle;
        diff = NormaliseAngle(diff);

	    if (diff < 0)
	    {
	        if (diff + turnSpeed >= 0)
	        {
	            FinishTurning();
	        }
	        else
	        {
	            currentAngle += turnSpeed;
	        }
	    }
	    else
	    {
	        if (diff - turnSpeed <= 0)
	        {
	            FinishTurning();
	        }
	        else
	        {
	            currentAngle -= turnSpeed;
	        }
	    }

	    rigidbody2D.velocity = MoveVelocity(currentAngle);
	}

    private float DecideNextAngle()
    {
        if (Random.Range(0f, 1f) < aggressiveness)
        {
            Vector2 playerPos = mazeManager.PlayerPosition();
            Vector2 currentPos = this.transform.position;
            Vector2 moveDir = mazeManager.gridGraph.GetMoveDirection(currentPos.x, currentPos.y, playerPos.x, playerPos.y);
            return ToAngle(moveDir);
        }
        return Random.Range(0, 360);
    }

    private float ToAngle(Vector2 vec)
    {
        return Mathf.Rad2Deg*Mathf.Atan2(-vec.y, -vec.x);
    }

    private void UpdateAI()
    {
        if (Time.time > nextTurnTime)
        {
            TurnTowards(DecideNextAngle());
            nextTurnTime = Time.time + Random.Range(1, 2);
        }
    }

    private Vector2 MoveVelocity(float angle)
    {
        var dirVec = new Vector2(Mathf.Cos(Mathf.Deg2Rad*angle), Mathf.Sin(Mathf.Deg2Rad*angle));
        return dirVec*moveSpeed;
    }

    private void FinishTurning()
    {
        currentAngle = targetAngle;
        isTurning = false;
    }
}
