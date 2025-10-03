using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollisionTally : MonoBehaviour
{
    [Header("Tally Settings")]
    public int collisionCount = 0; 
    public string targetTag = "Block";
    public bool useTagFilter = true; 
    [Header("UI Settings")]
    public TextMeshProUGUI tallyText; 
    public string displayPrefix = "Collisions: ";
    [Header("Collision Cooldown")]
    public bool useCooldown = true;
    public float cooldownTime = 0.5f; 
    private float lastCollisionTime = 0f;
    [Header("Debug")]
    public bool debugLog = true;
    
    private void Start()
    {
        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogWarning("Noob forgor Rigidbody " + gameObject.name);
        }
        
        UpdateTallyDisplay();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (useCooldown && Time.time < lastCollisionTime + cooldownTime)
        {
            return;
        }
    if (useTagFilter && !collision.gameObject.CompareTag(targetTag))
        { 
            return;
        }
        collisionCount++;
        lastCollisionTime = Time.time;
        PlayerStats.Instance.AddCollision();
        UpdateTallyDisplay();
        if (debugLog)
            Debug.Log("Collided with: " + collision.gameObject.name + ". Total collisions: " + collisionCount);
    }
    
    private void UpdateTallyDisplay()
    {
        if (tallyText != null)
            tallyText.text = displayPrefix + collisionCount;
    }
}