using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class defines the properties of each cloud object
public class CloudProperties : MonoBehaviour
{
    // how fast the cloud is moving
    private float speed;

    // when the cloud should despawn
    private float endPositionX;

    // cloud properties constructor
    public void StartFloating(float s, float x)
    {
        speed = s;
        endPositionX = x;
    }

    // Update is called once per frame
    void Update()
    {
        // move cloud
        transform.Translate(Vector3.right * (Time.deltaTime * speed));

        // destroy the cloud when applicable
        if (this.transform.position.x > endPositionX)
        {
            Destroy(this.gameObject);
        }
    }
}
