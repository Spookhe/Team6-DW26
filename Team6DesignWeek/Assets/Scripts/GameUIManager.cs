using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button startButton;

    private void Awake()
    {
        // Singleton for updating score and displaying UI
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;

        // UI Button actions
        retryButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Start()
    {
        HideGameOverUI(); // Hide UI on game start
    }

    // Updates score display
    public void UpdateScore(float currentScore)
    {
        scoreText.text = "Score: " + Mathf.FloorToInt(currentScore).ToString("D5");

        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);
        hiscoreText.text = "Hi-Score: " + Mathf.FloorToInt(hiscore).ToString("D5");
    }

    // Shows game over screen and checks the high score (Saves using Playerprefs)
    public void DisplayGameOverUI(float currentScore)
    {
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        UpdateHiscore(currentScore);
    }

    public void HideGameOverUI()
    {
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }

    // Saves new high score if player beats current
    private void UpdateHiscore(float currentScore)
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);
        if (currentScore > hiscore)
        {
            PlayerPrefs.SetFloat("hiscore", currentScore);
            hiscore = currentScore;
        }

        hiscoreText.text = "Hi-Score: " + Mathf.FloorToInt(hiscore).ToString("D5");
    }

    public void UpdateLives(int currentLives)
    {
        string currentLivesText = currentLives.ToString();
        livesText.text = "Lives:" + currentLivesText;
    }

    private void RestartGame()
    {
        GameManager.Instance.StartNewGame();
    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR // Editor debug to test quit button
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
