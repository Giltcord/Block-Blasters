using System;
using Unity.Collections;
using UnityEngine;

public class Collection : MonoBehaviour
{ 
    void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            Debug.Log($"Collided with: {collision.rigidbody.name}");
        }
    }
}
