using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject enemy;

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    Transform projectileFireTransform;

    [SerializeField]
    float timer;

    [SerializeField]
    float minDistanceToFire;

    [SerializeField]
    float timerDelta;

    public Rigidbody2D rb;
    private Vector3 direction;

    void Start()
    {
        timer = 3.0f;
        timerDelta = timer;
        Debug.Log("Here is the timer delta" + timerDelta);
    }

    void Update()
    {
        timerDelta -= Time.deltaTime;
        if (timerDelta < 0)
        {
            timerDelta = timer;

            if (
                Mathf.Abs(player.transform.position.x - this.transform.position.x)
                < minDistanceToFire
            )
            {
                Instantiate(projectile, projectileFireTransform.position, Quaternion.identity);
                rb = projectile.GetComponent<Rigidbody2D>();
            }
        }
    }
}
