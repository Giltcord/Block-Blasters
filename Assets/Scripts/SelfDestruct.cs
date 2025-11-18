using UnityEngine; 
public class SelfDestruct : ProjectileThrow
{
    public float lifetime = 10f;
    Rigidbody thrownObject;
    void Start()
    {
        Destroy( thrownObject, lifetime);
    }
}