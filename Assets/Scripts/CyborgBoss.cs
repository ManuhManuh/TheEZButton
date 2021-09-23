using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgBoss : MonoBehaviour
{
    public EasyButton game;
    public float speed;
    public float allowableIdleTime;

    private bool walking;
    private float idleTimer;
    private float timeIdle;
    

    private void Start()
    {
        idleTimer = Time.time;
    }
    private void Update()
    {
        // If the walk state is not Idle, apply the change in position
        if(walking)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            // Check to see how long idling has happened
            if(Time.time - idleTimer >= allowableIdleTime)
            {
                //TODO: Game over UI

                // End the game
                Application.Quit();
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the floor did not cause the collision
        if (other.gameObject.name != "Ground")
        {
            // Reset the idle timer (consecutive random idle returns from hits are not counted toward total idle time)
            

            // Let the game know that the cyborg was hit
            game.OnCyborgHit();

            // If the fence or an obstacle was hit, make the rotation 180 degrees instead of 90
            int rotationMultiplier = other.gameObject.CompareTag("TurnAround") ? 2 : 1;

            // Generate random action
            int action = Random.Range(0, 5);

            // Set walking parameter and do any required rotation (depending on random value generated)
            switch (action)
            {
                case 0:         // Stop walking
                    walking = false;
                    if (rotationMultiplier == 2)
                    {
                        // Turn around so walking is possible again if hit by office supplies
                        transform.Rotate(0, 180, 0);
                    }
                    break;
                case 1:         // Turn 90 degrees right (or 180 if the fence was hit)
                case 2:
                    transform.Rotate(0, rotationMultiplier * 90, 0);
                    walking = true;
                    break;
                case 3:         // turn 90 degrees left (or 180 if the fence was hit)
                case 4:
                    transform.Rotate(0, rotationMultiplier * -90, 0);
                    walking = true;
                    break;
                default:
                    break;
            }

            // Change the animation parameter
            GetComponent<Animator>().SetBool("Walking", walking);
            if (!walking)
            {
                idleTimer = Time.time;
            }
        }
    }
    
}


