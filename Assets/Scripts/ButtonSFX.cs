using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSFX : MonoBehaviour
{
   public static ButtonSFX Instance { get; private set; }
    
    [SerializeField] private AudioClip defaultButtonClickSFX;
    private AudioSource audioSource;
    private List<Button> allButtons = new List<Button>();
     private AudioManager audioManager;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
         audioManager = FindObjectOfType<AudioManager>();
         if (audioManager == null)
         {
             Debug.LogError("AudioManager not found in scene!");
         }
        FindAllButtons();
    }
    void Start()
    {
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
        }
        
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found. Button SFX will not play.");
        }
    }
    void FindAllButtons()
    {
        allButtons.Clear();
        
        Button[] buttonsInScene = FindObjectsOfType<Button>(true);
        allButtons.AddRange(buttonsInScene);
        
        Debug.Log($"Found {allButtons.Count} buttons in scene");
        
        foreach (Button button in allButtons)
        {
           
            button.onClick.RemoveListener(OnAnyButtonClicked);
            
            button.onClick.AddListener(OnAnyButtonClicked);
        }
    }
    void OnAnyButtonClicked()
    {
        PlayButtonSFX();
    }
    
    public void PlayButtonSFX()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.ButtonClick);
        }
        else if (defaultButtonClickSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(defaultButtonClickSFX);
            Debug.Log("Playing SFX directly via AudioSource");
        }
        else
        {
            Debug.LogWarning("Cannot play button SFX: No AudioManager found and no default SFX set");
        }
    }
    
    public void RefreshButtonList()
    {
        FindAllButtons();
    }
    public List<Button> GetAllButtons()
    {
        return new List<Button>(allButtons);
    }
    public int GetButtonCount()
    {
        return allButtons.Count;
    }
    public void SetAudioManager(AudioManager manager)
    {
        audioManager = manager;
    }
}

