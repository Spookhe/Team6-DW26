using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class startMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(startGame);
    }

    private void startGame()
    {
        SceneManager.LoadScene("CodeTesting");
    }
}
