using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SurvivalController : MonoBehaviour
{
    // array with all enemy types
    [SerializeField]
    private GameObject[] enemyVarients;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject rightBoundary;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private Vector3 playerSpawnPos;

    [SerializeField]
    private Transform leftSpawn;

    [SerializeField]
    private Transform rightSpawn;

    // spawn time
    [SerializeField]
    private float spawnTimer;

    // vars
    private List<GameObject> survivalEnemies;
    private TextMeshProUGUI[] tmps;
    private EnemyRespawner respawner;
    private HealthConstructor playerHealth;
    private SpriteRenderer playerRenderer;
    private PlayerMovement playerMovement;
    private Coroutine spawner;
    private AudioSource soundtrack;
    private Vector3 rightBoundarySPos;
    private float timer;
    public bool isSurvivalOn;
    private bool hasStarted;
    private string gameLog;
    private int playerPoints;
    private int minutes;
    private int seconds;
    private int enemiesKilled;

    // start method
    void Start()
    {
        respawner = this.GetComponent<EnemyRespawner>();
        soundtrack = this.GetComponent<AudioSource>();
        isSurvivalOn = false;
        hasStarted = false;
        timer = 0;
        tmps = menu.transform.GetComponentsInChildren<TextMeshProUGUI>();
        playerHealth = player.GetComponent<HealthConstructor>();
        playerSpawnPos = player.transform.position;

        playerRenderer = player.GetComponent<SpriteRenderer>();
        playerMovement = player.GetComponent<PlayerMovement>();
        togglePlayer();
        enemiesKilled = 0;
        rightBoundarySPos = rightBoundary.transform.position;
        survivalEnemies = new List<GameObject>();
    }

    // update method
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && !hasStarted)
        {
            isSurvivalOn = true;
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.L) && !hasStarted)
        {
            StartGame();
        }

        if (hasStarted)
        {
            timer += Time.deltaTime;
            timer = Mathf.Round(timer * 100.0f) * 0.01f;
            UpdateGame();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame(-1);
        }
    }

    // start game here
    private void StartGame()
    {
        tmps[0].gameObject.SetActive(false);
        tmps[1].gameObject.SetActive(false);
        tmps[2].text = "Health: 100	Score: 0";
        soundtrack.Play();
        togglePlayer();
        if (isSurvivalOn)
        {
            spawner = StartCoroutine(DoSpawnScheduling());
        }
        else
        {
            SetupLevel();
        }
        hasStarted = true;
    }

    // setup level
    public void SetupLevel()
    {
        respawner.ResetEnemies();
        rightBoundary.transform.position = new Vector3(240, 1, 0);
    }

    // scheduler
    private IEnumerator DoSpawnScheduling()
    {
        do
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnTimer);

            // check score and modify the timer here
        } while (true);
    }

    // spawn the enemy in here
    private void SpawnEnemy()
    {
        int enemytoSpawn = Random.Range(0, enemyVarients.Length);
        int xPos = Random.Range(0, 2);
        // get actual x coordinate to spawn in at
        GameObject obj;
        if (xPos == 0)
        {
            obj = Instantiate(enemyVarients[enemytoSpawn], leftSpawn.position, Quaternion.identity);
        }
        else
        {
            obj = Instantiate(
                enemyVarients[enemytoSpawn],
                rightSpawn.position,
                Quaternion.identity
            );
        }
        survivalEnemies.Add(obj);
    }

    // update the game logic
    public void UpdateGame()
    {
        if (isSurvivalOn)
        {
            tmps[2].text = "Health: " + playerHealth.Health + "\tScore: " + playerPoints;
            if (enemiesKilled % 5 == 0 && spawnTimer > 0)
            {
                spawnTimer -= 1;
                enemiesKilled += 1;
            }
        }
        else
        {
            minutes = (int)(timer / 0.60f);
            seconds = Mathf.RoundToInt((timer % .60f * 10.0f) * 10);

            tmps[2].text = "Health: " + playerHealth.Health + "\tScore: " + minutes + ":" + seconds;
        }
    }

    // update points here
    public void IncrementPoints(GameObject slainEnemy)
    {
        if (slainEnemy.name.Contains("SlimeEnemy"))
        {
            playerPoints += 2;
        }
        else
        {
            playerPoints += 3;
        }
        enemiesKilled += 1;
    }

    // reset the game
    public void ResetGame(int condition)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        switch (condition)
        {
            case 0:
                WinGame();
                break;
            case 1:
                if (isSurvivalOn)
                    WinGame();
                break;
        }

        soundtrack.Stop();
        if (isSurvivalOn)
        {
            foreach (GameObject obj in survivalEnemies)
            {
                Destroy(obj);
            }
            survivalEnemies.Clear();
            StopCoroutine(spawner);
        }

        playerHealth.Health = 100;
        player.transform.position = playerSpawnPos;
        timer = 0;
        playerPoints = 0;
        tmps[0].gameObject.SetActive(true);
        tmps[1].gameObject.SetActive(true);
        tmps[2].text = "";
        isSurvivalOn = false;
        hasStarted = false;
        respawner.DisableEnemies();
        rightBoundary.transform.position = rightBoundarySPos;
        togglePlayer();
    }

    // toggle if the player is active
    void togglePlayer()
    {
        playerRenderer.enabled = !playerRenderer.enabled;
        playerMovement.enabled = !playerMovement.enabled;
    }

    // win the game
    public void WinGame()
    {
        string score = playerPoints.ToString();
        if (isSurvivalOn)
        {
            score = minutes + ":" + seconds;
        }

        gameLog += (isSurvivalOn ? "Survival" : "Level") + "\t Score: " + score + "\n";
    }
}
