using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    // player reference
    private GameObject player;

    // make sure the enemy is in sight before firing
    private RaycastHit2D hit;
    private Vector2 rayDirection;

    // different combat states an enemy can be in
    private enum FightState
    {
        ATTACK,
        PATROL,
        WAIT,
        DEAD,
        STAGGERED
    }

    // directions the enemy can be facing
    private enum EnemyDirection
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    // enum references
    FightState currentFightState;
    EnemyDirection currentEnemyDirection;

    void Start()
    {
        player = GameObject.Find("Player");
        rayDirection = new Vector2(1, 0);
    }

    // set current fight state
    public void SetCurrentFightState(string state)
    {
        if (state.Equals("ATTACK"))
        {
            currentFightState = FightState.ATTACK;
        }
        else if (state.Equals("PATROL"))
        {
            currentFightState = FightState.PATROL;
        }
        else if (state.Equals("WAIT"))
        {
            currentFightState = FightState.WAIT;
        }
        else if (state.Equals("DEAD"))
        {
            currentFightState = FightState.DEAD;
        }
        else if (state.Equals("STAGGERED"))
        {
            currentFightState = FightState.STAGGERED;
        }
    }

    // get the current fight state
    public string GetCurrentFightState()
    {
        return currentFightState.ToString();
    }

    // set the enemy's direction
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

    // get the enemy's direction
    public string GetCurrentDirection()
    {
        return currentEnemyDirection.ToString();
    }

    // check if the player is visable to the enemy
    public bool GetIfPlayerIsInSight(float yDirection, bool ignoreEnemyDirection)
    {
        // returns true if there is no obstacle and the enemy is facing the player
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
        else // this method will return true if there is no obstacle between the enemy and player
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

        // cast a ray and see if it hits the player
        rayDirection.y = yDirection;
        hit = Physics2D.Raycast(this.transform.position, rayDirection, Mathf.Infinity);
        if (hit.collider != null)
        {
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
