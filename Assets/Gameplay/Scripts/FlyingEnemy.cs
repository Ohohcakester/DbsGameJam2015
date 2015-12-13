using System;
using UnityEngine;
using System.Collections;
using Random = OhRandom;

public class FlyingEnemy : MonoBehaviour
{
    private float currentAngle;
    private float targetAngle;
    private bool isTurning;

    private Rigidbody2D rigidbody2D;

    private MazeManager mazeManager;
    private GameVariables gameVariables;

    private PathData lastPathData = PathData.Null();

    private float nextTurnTime;

    private bool isAggressive;
    private float endAggressionTime;
    private float aggressionDuration = 8f;

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
        gameVariables = Camera.main.GetComponent<GameController>().GetGameVariables();
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.DrawLine(this.transform.position, 5 * ToUnitVector(targetAngle) + OhVec.toVector2(transform.position));
        Gizmos.DrawLine(this.transform.position, MoveVelocity(currentAngle) + OhVec.toVector2(transform.position));
    }*/

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

	    if (gameVariables.turnSpeed >= Mathf.Abs(diff))
	    {
	        FinishTurning();
	    }
	    else
	    {
	        if (diff < 0)
	        {
                currentAngle -= gameVariables.turnSpeed;
	        }
	        else
	        {
                currentAngle += gameVariables.turnSpeed;
	        }
	    }

	    rigidbody2D.velocity = MoveVelocity(currentAngle);
	}

    private void Pow(float power, ref float a)
    {
        a = Mathf.Pow(a, power);
    }

    private float StraightLineDistanceToPlayer()
    {
        return OhVec.Distance2D(OhVec.toVector2(transform.position), mazeManager.PlayerPosition());
    }

    private void TriggerAggression()
    {
        isAggressive = true;
        endAggressionTime = Time.time + aggressionDuration;
    }

    private float Aggressiveness()
    {
        if (mazeManager.PlayerNoTarget())
        {
            isAggressive = false;
            return -1;
        }


        float a = gameVariables.baseAggressiveness;

        if (isAggressive)
        {
            if (Time.time >= endAggressionTime)
            {
                isAggressive = false;
            }
            else
            {
                Pow(0.35f, ref a);
            }
        }

        if (StraightLineDistanceToPlayer() < gameVariables.baseDetectionRange) Pow(0.25f, ref a);
        if (!lastPathData.IsNull())
        {
            if (lastPathData.isDirect) Pow(0.1f, ref a);
        }
        return a;
    }

    private float DecideNextAngle()
    {

        // OBJECTIVE:: REDUCE PATH FINDS
        if (Random.Range(0f, 1f) < Aggressiveness())
        {
            UpdatePathData();
            if (!lastPathData.IsNull())
            {
                Vector2 currentPos = this.transform.position;
                Vector2 moveDir;
                if (lastPathData.isDirect)
                {
                    TriggerAggression();
                    Vector2 playerPos = mazeManager.PlayerPosition();
                    moveDir = new Vector2(playerPos.x - currentPos.x, playerPos.y - currentPos.y);
                }
                else
                {
                    moveDir = new Vector2(lastPathData.realNextX - currentPos.x, lastPathData.realNextY - currentPos.y);
                }
                return ToAngle(moveDir);
            }
        }
        return Random.Range(0, 360);
    }

    private void UpdatePathData()
    {
        Vector2 playerPos = mazeManager.PlayerPosition();
        Vector2 currentPos = transform.position;
        lastPathData = mazeManager.gridGraph.PathFind(currentPos.x, currentPos.y, playerPos.x, playerPos.y);
    }



    private void UpdateAI()
    {
        if (Time.time > nextTurnTime)
        {
            TurnTowards(DecideNextAngle());
            if (!lastPathData.IsNull() && lastPathData.isDirect) nextTurnTime = Time.time;
            else nextTurnTime = Time.time + gameVariables.decisionTime + Random.Range(0, gameVariables.decisionTimeExtra);
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
        return dirVec * gameVariables.moveSpeed;
    }

    private void FinishTurning()
    {
        currentAngle = targetAngle;
        isTurning = false;
    }
}
