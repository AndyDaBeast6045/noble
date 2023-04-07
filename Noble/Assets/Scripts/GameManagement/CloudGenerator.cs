using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class spawns clouds into the scene
public class CloudGenerator : MonoBehaviour
{
    // array of cloud prefabs
    [SerializeField]
    private GameObject[] clouds;

    // The rate at which the clouds will spawn in at
    [SerializeField]
    private float spawnRate;

    // where the clouds will despawn at
    [SerializeField]
    private GameObject endPoint;

    // the start position of the cloud spawner
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
        Prewarm();
        StartCoroutine(CloudSpawnScheduler());
    }

    // spawn a new cloud object into the scene
    public void SpawnCloud(Vector3 startPosition)
    {
        // get the height and scale of the cloud
        float startY = UnityEngine.Random.Range(startPosition.y - 1f, startPosition.y + 5f);
        float scale = UnityEngine.Random.Range(0.8f, 1.2f);

        // instantiate the cloud and set its position and scale
        GameObject cloud = Instantiate(clouds[UnityEngine.Random.Range(0, clouds.Length)]);
        cloud.transform.localScale = new Vector2(scale, scale);
        cloud.transform.position = new Vector3(startPosition.x, startY, startPosition.z);
        cloud
            .GetComponent<CloudProperties>()
            .StartFloating(UnityEngine.Random.Range(0.5f, 1.5f), endPoint.transform.position.x);
    }

    // preload clouds into the scene so they are there when the game begins
    public void Prewarm()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 spawnPos = startPosition + Vector3.right * (i * 2);
            SpawnCloud(spawnPos);
        }
    }

    // spawns in clouds at the specified rate
    public IEnumerator CloudSpawnScheduler()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnCloud(startPosition);
        }
    }
}
