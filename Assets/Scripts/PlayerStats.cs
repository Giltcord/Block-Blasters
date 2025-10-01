using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [Header("UI Display")]
    public TextMeshProUGUI totalCollisionsText;
    public TextMeshProUGUI currentLevelCollisionsText;
    public TextMeshProUGUI levelCompletionText;
    [Header("Stats Display Panel")]
    public GameObject statsPanel;
    public bool showStatsOnLevelComplete = true;
    [Header("Reset Confirmation")]
    public GameObject resetConfirmationPanel;
    public Button confirmResetButton;
    public Button cancelResetButton;
    public Button resetStatsButton; 
   // stats
    private int totalCollisions = 0;
    private int currentLevelCollisions = 0;
    private int completedLevels = 0;
    private Dictionary<string, int> levelCollisions = new Dictionary<string, int>();
    
    private static PlayerStats instance; 
    public static PlayerStats Instance
    { get
        { 
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStats>();
                if (instance == null)
                { 
                    GameObject obj = new GameObject("PlayerStats");
                    instance = obj.AddComponent<PlayerStats>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }
    
    void Awake()
    {
        if (instance == null)
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        if (!PlayerPrefs.HasKey("TotalCollisions"))
        {
            ResetAllStats();
        }
        else
        {
            LoadStats();
        }
    }
    
    void Start()
    {
        if (resetStatsButton != null)
        {
            resetStatsButton.onClick.AddListener(ShowResetConfirmation);
        }
        
        if (confirmResetButton != null)
        {
            confirmResetButton.onClick.AddListener(ConfirmResetStats);
        }
        
        if (cancelResetButton != null)
        {
            cancelResetButton.onClick.AddListener(HideResetConfirmation);
        }
        if (resetConfirmationPanel != null)
        {
            resetConfirmationPanel.SetActive(false);
        }
        UpdateDisplay();
        if (statsPanel != null)
        {
            statsPanel.SetActive(true);
        }
    }
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        currentLevelCollisions = 0;
        
        FindUIElements();
        UpdateDisplay();
        
        SaveStats();
    }
    
    private void FindUIElements()
    {
        if (totalCollisionsText == null)
        {
            TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                if (text.name.Contains("TotalCollisions") || text.name.Contains("Stats"))
                {
                    totalCollisionsText = text;
                }
                else if (text.name.Contains("CurrentCollisions") || text.name.Contains("Level"))
                {
                    currentLevelCollisionsText = text;
                }
            }
        }
    }
    public void AddCollision(string levelName = "")
    {
        totalCollisions++;
        currentLevelCollisions++;
        if (string.IsNullOrEmpty(levelName))
        {
            levelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        if (levelCollisions.ContainsKey(levelName))
        {
            levelCollisions[levelName]++;
        }
        else
        {
            levelCollisions[levelName] = 1;
        }
        UpdateDisplay();
    }
    
    // Call this when a level is completed
    public void LevelCompleted()
    {
        completedLevels++;
        if (showStatsOnLevelComplete && statsPanel != null)
        {
            statsPanel.SetActive(true);
        }
        SaveStats();
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (totalCollisionsText != null)
        {
            totalCollisionsText.text = $"Total Collisions: {totalCollisions}";
        }
        if (currentLevelCollisionsText != null)
        {
            currentLevelCollisionsText.text = $"This Level: {currentLevelCollisions}";
        }
        if (levelCompletionText != null)
        {
            levelCompletionText.text = $"Levels Completed: {completedLevels}";
        }
    }
    
    public int GetTotalCollisions() { return totalCollisions; }
    public int GetCurrentLevelCollisions() { return currentLevelCollisions; }
    public int GetCompletedLevels() { return completedLevels; }
    public Dictionary<string, int> GetLevelCollisions() { return new Dictionary<string, int>(levelCollisions); }
    public void SaveStats()
    {
        PlayerPrefs.SetInt("TotalCollisions", totalCollisions);
        PlayerPrefs.SetInt("CompletedLevels", completedLevels);
        string levelData = "";
        foreach (var entry in levelCollisions)
        {
            levelData += $"{entry.Key}:{entry.Value};";
        }
        PlayerPrefs.SetString("LevelCollisions", levelData);
        PlayerPrefs.Save();
    }
    
    public void LoadStats()
    {
        totalCollisions = PlayerPrefs.GetInt("TotalCollisions", 0);
        completedLevels = PlayerPrefs.GetInt("CompletedLevels", 0);
        
        string levelData = PlayerPrefs.GetString("LevelCollisions", "");
        if (!string.IsNullOrEmpty(levelData))
        {
            levelCollisions.Clear();
            string[] entries = levelData.Split(';');
            foreach (string entry in entries)
            {
                if (!string.IsNullOrEmpty(entry))
                {
                    string[] parts = entry.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                    {
                        levelCollisions[parts[0]] = count;
                    }
                }
            }
        }
    }
    public void ShowResetConfirmation()
    {
        if (resetConfirmationPanel != null)
        {
            resetConfirmationPanel.SetActive(true);
        }
    }
    public void HideResetConfirmation()
    {
        if (resetConfirmationPanel != null)
        {
            resetConfirmationPanel.SetActive(false);
        }
    }
    public void ConfirmResetStats()
    {
        ResetAllStats();
        HideResetConfirmation();
    }
    public void ResetAllStats()
    {
        totalCollisions = 0;
        currentLevelCollisions = 0;
        completedLevels = 0;
        levelCollisions.Clear();
        
        PlayerPrefs.DeleteKey("TotalCollisions");
        PlayerPrefs.DeleteKey("CompletedLevels");
        PlayerPrefs.DeleteKey("LevelCollisions");
        PlayerPrefs.Save();
        
        UpdateDisplay();
        
        Debug.Log("All player stats have been reset to zero.");
    }
    public void ToggleStatsPanel()
    {
        if (statsPanel != null)
        {
            statsPanel.SetActive(!statsPanel.activeSelf);
        }
    }
    public string GetFormattedStats()
    {
        return $"Player Statistics:\n" +
               $"Total Collisions: {totalCollisions}\n" +
               $"Levels Completed: {completedLevels}\n" +
               $"Current Level Collisions: {currentLevelCollisions}";
    }
}
