using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateController : MonoBehaviour
{
    private enum FightState
    {
        Attack,
        Patrol
    }

    private enum EnemyDirection
    {
        Left,
        Right
    }

    FightState currentFightState;
    EnemyDirection currentEnemyDirection;

    public void SetCurrentFightState(string state)
    {
        if (state.Equals("Attack"))
        {
            currentFightState = FightState.Attack;
        }
        else
        {
            currentFightState = FightState.Patrol;
        }
    }

    public string GetCurrentFightState()
    {
        return currentFightState.ToString();
    }

    public void SetCurrentDirection(string state)
    {
        if (state.Equals("Left"))
        {
            currentEnemyDirection = EnemyDirection.Left;
        }
        else
        {
            currentEnemyDirection = EnemyDirection.Right;
        }
    }

    public string GetCurrentDirection()
    {
        return currentEnemyDirection.ToString();
    }
}
