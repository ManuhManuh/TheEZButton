using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public GameObject spawnOrigin;
    public Rigidbody[] officeSupplies;

    private void Awake()
    {

        if (instance != null)
        {
            // Error
            Debug.LogError("There was more than 1 Spawn Manager");
        }
        else
        {
            instance = this;
        }
    }

    public void SpawnOfficeSupplies()
    {
        // Spawn random office supply item
        Rigidbody randomObject = officeSupplies[Random.Range(0, officeSupplies.Length - 1)];

        Instantiate(randomObject, spawnOrigin.transform.position, spawnOrigin.transform.rotation);
    }
}
