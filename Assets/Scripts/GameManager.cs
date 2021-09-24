using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioClip[] bossResponses;

    private void Awake()
    {

        if (instance != null)
        {
            // Error
            Debug.LogError("There was more than 1 Game Manager");
        }
        else
        {
            instance = this;
        }
    }
 
    //public void OnCyborgHit(GameObject hitBy)
    //{
    //    //If the cyborg was hit by an office supply item (as opposed to hitting the environment)


    //    //Figure out which clip to play
    //    AudioClip randomClip = bossResponses[Random.Range(0, bossResponses.Length - 1)];

    //    // Play the sound clip
    //    SoundManager.PlaySound(gameObject, randomClip.name.ToString());

    //}
}
