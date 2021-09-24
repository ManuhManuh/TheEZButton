using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyborgBoss : MonoBehaviour
{
    //public EasyButton game;
    public float speed;
    public float allowableIdleTime;
    public ParticleSystem fireParticle;
    public float takeOffForce;
    public int bossDemiseThreshold;

    private bool walking;
    private float idleTimer;
    private float timeIdle;
    private AudioClip[] bossResponses;
    private int stickyCount;
    private bool bossRising;
    private Rigidbody boss;

    private void Start()
    {
        idleTimer = Time.time;
        bossResponses = SoundManager.instance.audioClips;
        stickyCount = 0;
        boss = gameObject.GetComponent<Rigidbody>();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BossTakesOff();
        }

        if (bossRising)
        {
            boss.AddForce(Vector3.up * takeOffForce, ForceMode.Impulse);
        }
        
        if (transform.position.y > bossDemiseThreshold)
        {
            SoundManager.PlaySound(gameObject, "Ah");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if the floor did not cause the collision
        if (other.gameObject.name != "Ground")
        {
            // Reset the idle timer (consecutive random idle returns from hits are not counted toward total idle time)

            string nameStart = other.gameObject.name.Substring(0, 6);
            if (nameStart == "Sticky")
            {
                stickyCount++;

                if (stickyCount > 10)
                {
                    BossTakesOff();
                }
            }
            // Generate sound effects
            GenerateSoundFX(other.gameObject);

            // If the fence or an obstacle was hit, make the rotation 180 degrees instead of 90
            int rotationMultiplier = other.gameObject.CompareTag("TurnAround") ? 2 : 1;
            TakeAction(rotationMultiplier);
        }
    }

    private void GenerateSoundFX(GameObject item)
    {
        if (item.CompareTag("OfficeSupplies"))
        {
            string nameStart = item.name.Substring(0, 6);
            string clipToPlay;

            if (nameStart == "Sticky")
            {
                // Play the sticky clip
                clipToPlay = "Sticky";
                stickyCount++;
            }
            else
            {
                //Figure out which clip to play: do not include 0, as this is the clip for the end scenario
                AudioClip randomClip = bossResponses[Random.Range(1, bossResponses.Length - 1)];
                clipToPlay = randomClip.name;
            }

            // Play the sound clip
            SoundManager.PlaySound(gameObject, clipToPlay);
        }
    }

    private void TakeAction(int rotationMultiplier)
    {
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

    private void BossTakesOff()
    {
        SoundManager.PlaySound(gameObject, "Ah");
        fireParticle.Play();
        bossRising = true;
    }
    
}


