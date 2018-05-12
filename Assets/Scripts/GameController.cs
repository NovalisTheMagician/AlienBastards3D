using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    enum GameState
    {
        TITLE,
        PLAY,
        GAMEOVER
    };

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private BulletPool bulletPool;

    private Vector3 playerStartPos;

    [SerializeField]
    private Text pointsText;

    [SerializeField]
    private Text finalPointsText;

    [SerializeField]
    private GameObject gameStartPanel;

    [SerializeField]
    private GameObject gameEndPanel;

    [SerializeField]
    private float spawnTimeDelay = 1;

    private float currentTime = 0;

    [SerializeField]
    private GameObject enemyPrefab;

    private List<GameObject> enemies;
    
    public int Points { get; private set; }

    private GameState currentState;
    private GameState nextState;

	void Start ()
    {
        Points = 0;

        player.GetComponent<Player>().OnDeath += OnPlayerDeath;
        playerStartPos = player.transform.position;

        currentState = GameState.TITLE;
        nextState = GameState.TITLE;

        gameStartPanel.SetActive(true);

        enemies = new List<GameObject>();

        bulletPool.SetBoundsCallback(OnBulletOutBounds);
        bulletPool.InitPool();
	}
	
	void Update ()
    {
        UpdatePointsLabel();

        if(nextState != currentState)
        {
            if(nextState == GameState.PLAY)
            {
                ClearEnemies();

                gameStartPanel.SetActive(false);
                gameEndPanel.SetActive(false);

                player.transform.position = playerStartPos;
                player.SetActive(true);

                Points = 0;
            }
            else if(nextState == GameState.GAMEOVER)
            {
                ClearEnemies();
                finalPointsText.text = string.Format("Final Score: {0}", Points);
                player.SetActive(false);
                gameEndPanel.SetActive(true);
            }
        }

        currentState = nextState;

        switch (currentState)
        {
            case GameState.TITLE:
                {
                    if (Input.GetButtonDown("Submit"))
                        nextState = GameState.PLAY;
                }
                break;

            case GameState.PLAY:
                {
                    currentTime += Time.deltaTime;

                    if(currentTime >= spawnTimeDelay)
                    {
                        SpawnEnemy();
                        currentTime -= spawnTimeDelay;
                    }
                }
                break;

            case GameState.GAMEOVER:
                {
                    if (Input.GetButtonDown("Submit"))
                        nextState = GameState.PLAY;
                }
                break;
        }
	}

    private void ClearEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        enemies.Clear();
    }

    public void OnEnemyKill(GameObject enemy)
    {
        Points += 10;
    }

    public void OnEnemyDespawn(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void OnPlayerDeath()
    {
        nextState = GameState.GAMEOVER;
    }

    public void OnBulletOutBounds(GameObject bullet)
    {
        Points -= 5;
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = new Vector3(0, 10, 0);
        spawnPos.x = Random.Range(-10, 10);

        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = spawnPos;

        enemy.GetComponent<Enemy>().OnDeath += OnEnemyKill;
        enemy.GetComponent<Enemy>().OnDespawn += OnEnemyDespawn;

        enemies.Add(enemy);
    }

    private void UpdatePointsLabel()
    {
        pointsText.text = string.Format("Points: {0}", Points);
    }
}
