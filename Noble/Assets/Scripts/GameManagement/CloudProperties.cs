using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudProperties : MonoBehaviour
{
    private float speed;
    private float endPositionX;

    // Cloud properties Constructor
    public void StartFloating(float s, float x)
    {
        speed = s;
        endPositionX = x;
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * speed));
        if (transform.position.x > endPositionX)
        {
            Destroy(this.gameObject);
        }
    }
}
