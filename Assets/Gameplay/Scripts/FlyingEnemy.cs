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

    private PathData lastPathData = PathData.Null();

    private float nextTurnTime;

    private float aggressiveness;

	// Use this for initialization
	void Start ()
	{
        Initialise();
        Debug.Log(ToAngle(ToUnitVector(0)));
        Debug.Log((ToUnitVector(45)));
        Debug.Log(ToAngle(ToUnitVector(90)));
        Debug.Log(ToAngle(ToUnitVector(135)));
        Debug.Log(ToAngle(ToUnitVector(180)));
        Debug.Log(ToAngle(ToUnitVector(225)));
	}

    private void Initialise()
    {
        if (rigidbody2D != null) return;
        rigidbody2D = GetComponent<Rigidbody2D>();
        mazeManager = Camera.main.GetComponent<MazeManager>();
        aggressiveness = 1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, 5 * ToUnitVector(targetAngle) + OhVec.toVector2(transform.position));
        Gizmos.DrawLine(this.transform.position, MoveVelocity(currentAngle) + OhVec.toVector2(transform.position));
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

	    if (turnSpeed >= Mathf.Abs(diff))
	    {
	        FinishTurning();
	    }
	    else
	    {
	        if (diff < 0)
	        {
	            currentAngle -= turnSpeed;
	        }
	        else
	        {
	            currentAngle += turnSpeed;
	        }
	    }

	    rigidbody2D.velocity = MoveVelocity(currentAngle);
	}

    private float DecideNextAngle()
    {
        UpdatePathData();
        if (!lastPathData.IsNull() && Random.Range(0f, 1f) < aggressiveness)
        {
            Vector2 currentPos = this.transform.position;
            var moveDir = new Vector2(lastPathData.realNextX - currentPos.x, lastPathData.realNextY - currentPos.y);
            return ToAngle(moveDir);
        }
        return Random.Range(0, 360);
    }

    private void UpdatePathData()
    {
        Vector2 playerPos = mazeManager.PlayerPosition();
        Vector2 currentPos = this.transform.position;
        lastPathData = mazeManager.gridGraph.PathFind(currentPos.x, currentPos.y, playerPos.x, playerPos.y);
    }



    private void UpdateAI()
    {
        if (Time.time > nextTurnTime)
        {
            TurnTowards(DecideNextAngle());
            nextTurnTime = Time.time + Random.Range(1, 2);
        }
    }

    private float ToAngle(Vector2 vec)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(vec.y, vec.x);
    }

    private Vector2 ToUnitVector(float angle)
    {
        return new Vector2(Mathf.Cos(Mathf.Deg2Rad*angle), Mathf.Sin(Mathf.Deg2Rad*angle));
    }

    private Vector2 MoveVelocity(float angle)
    {
        var dirVec = ToUnitVector(angle);
        return dirVec*moveSpeed;
    }

    private void FinishTurning()
    {
        currentAngle = targetAngle;
        isTurning = false;
    }
}
