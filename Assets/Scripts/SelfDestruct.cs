using UnityEngine; 
public class SelfDestruct : MonoBehaviour
{
    public float lifetime = 10f;
    
    void Start()
    {
        // Automatically destroy this game object after lifetime seconds
        Destroy(gameObject, lifetime);
    }
}