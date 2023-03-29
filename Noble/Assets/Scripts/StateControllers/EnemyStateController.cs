using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    // make sure the enemy is in sight before firing
    private RaycastHit2D hit;
    private bool playerIsInSight;
    private Vector2 rayDirection;

    private enum FightState
    {
        ATTACK,
        PATROL,
        WAIT
    }

    private enum EnemyDirection
    {
        LEFT,
        RIGHT
    }

    FightState currentFightState;
    EnemyDirection currentEnemyDirection;

    void Start()
    {
        playerIsInSight = false;
        rayDirection = new Vector2(1, 0);
    }

    void FixedUpdate() { }

    public void SetCurrentFightState(string state)
    {
        if (state.Equals("ATTACK"))
        {
            currentFightState = FightState.ATTACK;
        }
        else if (state.Equals("PATROl"))
        {
            currentFightState = FightState.PATROL;
        }
        else
        {
            currentFightState = FightState.WAIT;
        }
    }

    public string GetCurrentFightState()
    {
        return currentFightState.ToString();
    }

    public void SetCurrentDirection(string state)
    {
        if (state.Equals("LEFT"))
        {
            currentEnemyDirection = EnemyDirection.LEFT;
        }
        else
        {
            currentEnemyDirection = EnemyDirection.RIGHT;
        }
    }

    public string GetCurrentDirection()
    {
        return currentEnemyDirection.ToString();
    }

    public bool GetIfPlayerIsInSight(float yDirection, bool ignoreEnemyDirection)
    {
        if (!ignoreEnemyDirection)
        {
            if (currentEnemyDirection == EnemyDirection.LEFT)
            {
                rayDirection.x = -1;
            }
            else
            {
                rayDirection.x = 1;
            }
        }
        else
        {
            if (player.transform.position.x < this.transform.position.x)
            {
                rayDirection.x = -1;
            }
            else
            {
                rayDirection.x = 1;
            }
        }

        rayDirection.y = yDirection;
        Debug.DrawRay(this.transform.position, rayDirection, Color.green);
        hit = Physics2D.Raycast(this.transform.position, rayDirection, Mathf.Infinity);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}
