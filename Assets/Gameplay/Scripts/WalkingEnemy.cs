using UnityEngine;
using System.Collections.Generic;

public class WalkingEnemy : MonoBehaviour
{
    private bool movingLeft;
    private Rigidbody2D rigidbody2D;
    private Transform enemySprite;
    private float edgeDetectionRange = 0.5f;
    private float wallDetectionRange = 0.7f;
    private float floorBelow = 0.8f;
    private MazeManager mazeManager;
    private GameVariables gameVariables;

    private int turnsLeft;
    private float nextTurnsLeftRegen;

    private Vector3 lastPosition;
    private float runningAverageMoveDistance = 1;
    private float defaultRunningAverage = 1f;
    private float alpha = 0.3f;

    private void Start()
    {
        Initialise();
    }

    private void FixedUpdate()
    {
        float distanceMoved = OhVec.Distance2D(lastPosition, transform.position);
        lastPosition = transform.position;
        runningAverageMoveDistance = alpha*distanceMoved + (1 - alpha)*runningAverageMoveDistance;
        if (runningAverageMoveDistance < 0.002f)
        {
            TurnAround();
            runningAverageMoveDistance = defaultRunningAverage;
        }
    }

    /*private void OnDrawGizmos()
    {
        {
            float queryX = transform.position.x;
            if (movingLeft) queryX -= wallDetectionRange;
            else queryX += wallDetectionRange;

            float queryY = transform.position.y;

            Gizmos.DrawLine(this.transform.position, new Vector3(queryX, queryY, 0));
        }
        {
            float queryX = transform.position.x;
            if (movingLeft) queryX -= edgeDetectionRange;
            else queryX += edgeDetectionRange;

            float queryY = transform.position.y - floorBelow;
            Gizmos.DrawLine(this.transform.position, new Vector3(queryX, queryY, 0));
        }

    }*/

    private void Initialise()
    {
        if (rigidbody2D != null) return;
        rigidbody2D = GetComponent<Rigidbody2D>();
        mazeManager = Camera.main.GetComponent<MazeManager>();
        gameVariables = Camera.main.GetComponent<GameController>().GetGameVariables();
        enemySprite = transform.FindChild("EnemySprite");
        lastPosition = this.transform.position;
    }

    private void Update()
    {
        if (CannotMoveFurther() && turnsLeft > 0)
        {
            TurnAround();
            turnsLeft--;
        }
        else
        {
            float vx = 0;
            if (movingLeft) vx -= gameVariables.walkingEnemySpeed;
            else vx += gameVariables.walkingEnemySpeed;
            rigidbody2D.velocity = OhVec.SetX(rigidbody2D.velocity, vx);
        }

        if (Time.time >= nextTurnsLeftRegen)
        {
            if (turnsLeft < 5) turnsLeft++;
            nextTurnsLeftRegen += 5f;
        }
    }

    private void TurnAround()
    {
        enemySprite.localScale = OhVec.FlipX(enemySprite.localScale);
        movingLeft = !movingLeft;
    }

    private bool CannotMoveFurther()
    {
        return AtEdgeOfPlatform() || BlockedByWall();
    }

    private bool BlockedByWall()
    {
        float queryX = transform.position.x;
        if (movingLeft) queryX -= wallDetectionRange;
        else queryX += wallDetectionRange;

        float queryY = transform.position.y;

        return mazeManager.gridGraph.IsBlockedActual(queryX, queryY);
    }

    private bool AtEdgeOfPlatform()
    {
        float queryX = transform.position.x;
        if (movingLeft) queryX -= edgeDetectionRange;
        else queryX += edgeDetectionRange;

        float queryY = transform.position.y - floorBelow;

        return !mazeManager.gridGraph.IsBlockedActual(queryX, queryY);
    }

}