using Mono.Cecil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerSystem : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTime = 60f; // Total time in seconds
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
    public GameObject player; // Reference to player or game objects to control
    
    private float currentTime;
    private bool timerRunning = false;
    private bool gameEnded = false;
    
    void Start()
    {
        // Initialize timer
        currentTime = totalTime;
        UpdateTimerDisplay();
        
        // Hide menu panel at start
        menuPanel.SetActive(false);
        
        // Setup button listeners
        retryButton.onClick.AddListener(RetryLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        nextLevelButton.onClick.AddListener(GoToNextLevel);
        
        // Start the timer
        StartTimer();
    }
    
    void Update()
    {
        if (timerRunning && !gameEnded)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
            
            // Check if time runs out
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                TimerEnded(false); // Time ran out - player failed
            }
        }
    }
    
    void UpdateTimerDisplay()
    {
        // Update timer text (format: MM:SS)
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        // Update slider
        if (timerSlider != null)
        {
            timerSlider.value = currentTime / totalTime;
        }
    }
    
    public void StartTimer()
    {
        timerRunning = true;
        gameEnded = false;
    }
    
    public void StopTimer()
    {
        timerRunning = false;
    }
    
    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;
        UpdateTimerDisplay();
    }
    
    // Call this when player completes level successfully
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
    
        // Show menu panel
        menuPanel.SetActive(true);
    
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    
        // Set result text
        if (levelCompleted)
        {
            resultText.text = "Level Complete!";
            resultText.color = Color.green;
            nextLevelButton.interactable = true;
        
            // Notify PlayerStats that level was completed
            PlayerStats.Instance.LevelCompleted();
        }
        else
        {
            resultText.text = "Time's Up!";
            resultText.color = Color.red;
            nextLevelButton.interactable = true;
        }
    
        // Optional: Pause game logic
        Time.timeScale = 0f;
    }
    
    // Button Functions
    public void RetryLevel()
    {
        Time.timeScale = 1f; // Resume normal time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
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
    
    // For debugging purposes
    public void CompleteLevelDebug()
    {
        LevelCompleted();
    }
    
    public void FailLevelDebug()
    {
        TimerEnded(false);
    }
}