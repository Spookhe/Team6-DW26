using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialSpeed = 5f;
    public float speedGainRate = 0.1f;
    public float gameSpeed { get; private set; }

    public Background background;

    private float score;
    public float Score => score;

    private PlayerController player;
    private ObjectSpawner spawner;

    private void Awake()
    {
        // Singleton for managing game logic
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Find core game components
        player = FindObjectOfType<PlayerController>();
        spawner = FindObjectOfType<ObjectSpawner>();

        StartNewGame();
    }

    private void Update()
    {
        // Accelerates the game over playtime
        gameSpeed += speedGainRate * Time.deltaTime;

        // Score increase based on speed acceleration
        score += gameSpeed * Time.deltaTime;

        GameUIManager.Instance.UpdateScore(score);
    }

    public void GameOver()
    {
        // Stop game logic
        gameSpeed = 0f;
        enabled = false;

        // Disables player and spawner when game is lost
        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);

        GameUIManager.Instance.DisplayGameOverUI(score);
    }

    public void StartNewGame()
    {

        // Clears all cacti from the game to prevent restart glitches
        ClearAllObstacles();

        score = 0f;
        gameSpeed = initialSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);

        // Reset the background colour to white
        Background bg = FindObjectOfType<Background>();
        if (bg != null)
            bg.ResetBackground();

        GameUIManager.Instance.HideGameOverUI();
    }

    // Clears all "obstacle" tags before restarting the game
    private void ClearAllObstacles()
    {
        foreach (var obstacle in GameObject.FindGameObjectsWithTag("Obstacle"))
            Destroy(obstacle);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
