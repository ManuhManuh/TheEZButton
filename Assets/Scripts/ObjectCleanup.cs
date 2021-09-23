using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCleanup : MonoBehaviour
{
    public float objectLifeSpan;
    private float objectTimer;
    private float timeSinceThrown;

    private void Awake()
    {
        objectTimer = Time.time;
    }
    
    void Update()
    {
        
        timeSinceThrown = Time.time - objectTimer;
        // if timer has run out
        if (timeSinceThrown > objectLifeSpan)
        {
            // destroy the food
            Destroy(gameObject);
        }
    }
}
