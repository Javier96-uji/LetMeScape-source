using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Configuracion de Crias")]
    public int totalBabies;
    private int babiesCaught = 0;

    [Header("UI")]
    public TextMeshProUGUI statusText;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject carryingText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        totalBabies = GameObject.FindGameObjectsWithTag("Baby").Length;
        UpdateUI();
    }

    public void BabyRescued()
    {
        babiesCaught++;
        UpdateUI();

        if (babiesCaught == totalBabies)
        {
            WinGame();
        }
    }

    private void UpdateUI()
    {
        if (statusText != null)
            statusText.text = $"Crias rescatadas: {babiesCaught} / {totalBabies}";
    }

    public void WinGame()
    {
        Debug.Log("Has rescatado a todas");
        if (winPanel != null) winPanel.SetActive(true);
        SoundManager.instance.PlayWin();
        Time.timeScale = 0f; // Pausa el juego
    }

    public void LoseGame()
    {
        Debug.Log("El Windigo te ha atrapado");
        if (losePanel != null) losePanel.SetActive(true);
        SoundManager.instance.PlayLose();
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
    public void SetCarryingText(bool active) => carryingText?.SetActive(active);
}
