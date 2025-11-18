
using UnityEngine;


public class AudioManager : MonoBehaviour
{   
   
        
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    
    public AudioClip BackgroundMusic;
    public AudioClip collectSound;
    public AudioClip shoot;
    public AudioClip LevelComplete;
    public AudioClip ButtonClick;

    public static AudioManager Instance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
          
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        musicSource.clip = BackgroundMusic;
        musicSource.Play();
        
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    
}
