using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerSystem : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 60f; 
    public TextMeshProUGUI timerText;
    public Slider timerSlider;
    
    [Header("Menu Panel")]
    public GameObject menuPanel;
    public TextMeshProUGUI resultText;
    
    [Header("Buttons")]
    public Button retryButton;
    public Button mainMenuButton;
    public Button nextLevelButton;
    
    [Header("Game Objects")]
    public GameObject player; 
    private float currentTime;
    private bool timerRunning = false;
    private bool gameEnded = false;
    
    void Start()
    {
        currentTime = totalTime;
        UpdateTimerDisplay();
        menuPanel.SetActive(false);
        retryButton.onClick.AddListener(RetryLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        nextLevelButton.onClick.AddListener(GoToNextLevel);
        StartTimer();
    }
    
    void Update()
    {
        if (timerRunning && !gameEnded)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                TimerEnded(true); 
            }
        }
    }
    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        if (timerSlider != null) timerSlider.value = currentTime / totalTime;
    }
    public void StartTimer()
    {
        timerRunning = true;
        gameEnded = false;
    }
    public void LevelCompleted()
    {
        if (!gameEnded)
        {
            TimerEnded(true);
        }
    }
    private void TimerEnded(bool levelCompleted)
    {
        timerRunning = false;
        gameEnded = true;
        menuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (levelCompleted)
        {
            resultText.text = "Level Complete!";
            resultText.color = Color.green;
            nextLevelButton.interactable = true;
            PlayerStats.Instance.LevelCompleted();
        }
        else
        {
            resultText.text = "Time's Up!";
            resultText.color = Color.red;
            nextLevelButton.interactable = true;
        }
        Time.timeScale = 0f;
    }
    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
    public void GoToNextLevel()
    {
        Time.timeScale = 1f;
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentIndex + 1;
    
        Debug.Log($"Current scene index: {currentIndex}. Attempting to load scene at index: {nextSceneIndex}. Total scenes in build: {SceneManager.sceneCountInBuildSettings}");
    
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No next scene found. Going to MainMenu.");
            SceneManager.LoadScene("MainMenu");
        }
    }
    public void CompleteLevelDebug()
    {
        LevelCompleted();
    }
}