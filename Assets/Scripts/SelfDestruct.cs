using UnityEngine; 
public class SelfDestruct : MonoBehaviour
{
    public float lifetime = 10f;
    
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}