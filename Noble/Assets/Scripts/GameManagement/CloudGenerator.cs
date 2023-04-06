using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] clouds;

    [SerializeField]
    private float spawnRate;

    [SerializeField]
    private GameObject endPoint;

    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        Prewarm();
        Invoke("AttemptSpawn", spawnRate); // change to coroutine later
    }

    public void SpawnCloud(Vector3 startPosition)
    {
        float startY = UnityEngine.Random.Range(startPosition.y - 1f, startPosition.y + 1f);
        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        GameObject cloud = Instantiate(clouds[UnityEngine.Random.Range(0, clouds.Length)]);
        cloud.transform.localScale = new Vector2(scale, scale);
        cloud.transform.position = new Vector3(startPosition.x, startY, startPosition.z);
        cloud
            .GetComponent<CloudProperties>()
            .StartFloating(UnityEngine.Random.Range(0.5f, 1.5f), endPoint.transform.position.x);
    }

    public void AttemptSpawn()
    {
        // checking criteria
        SpawnCloud(startPosition);
        Invoke("AttemptSpawn", spawnRate);
    }

    public void Prewarm()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 spawnPos = startPosition + Vector3.right * (i * 2);
            SpawnCloud(spawnPos);
        }
    }
}
