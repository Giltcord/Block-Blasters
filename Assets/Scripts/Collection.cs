
using UnityEngine;

public class Collection : MonoBehaviour
{ 
    
    AudioManager audioManager;

    private void Awake()
    {
       
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void OnCollisionEnter(Collision collision)
    {
     
        if (collision.rigidbody != null)
        {
            audioManager.PlaySFX(audioManager.collectSound);
            Debug.Log($"Collided with: {collision.rigidbody.name}");
            
        }
    }
}
