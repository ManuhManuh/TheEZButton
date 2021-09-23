using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public string thumbstickBeamInputName;
    public float thumbstickForwardThreshold;
    public Color validColour;
    public Color invalidColour;
    public LineRenderer beam;
    public float range;
    public GameObject teleportIndicator;
    public Transform player;
    public int invalidTargetLayer;

    private bool hasValidTeleportTarget;

    void Start()
    {
        SetBeamVisible(false);
    }

    void Update()
    {
        // If the thumbstick is pressed forward
        if (Input.GetAxis(thumbstickBeamInputName) < thumbstickForwardThreshold)
        {
            // Show the teleport beam
            SetBeamVisible(true);

            // Extend beam to maximum range (tranform.position is hand in world)
            SetBeamEndPoint(transform.position + transform.forward * range);

            // Check if the beam hit something
 
            if (Physics.Raycast(transform.position, transform.forward, out var hit, range))
            {
                // Update beam end point to the point in space it hit (so it doesn't pass though any objects)
                SetBeamEndPoint(hit.point);

                // If the thing it hit is a valid teleport target
                if (IsValidTeleportTarget(hit.collider.gameObject))
                {
                    // Set the beam to be valid (which will change colour, show spot, etc.) 
                    SetTeleportValid(true);

                    // Set the position of the teleport indicator (just off the floor to avoid z-fighting
                    teleportIndicator.transform.position = hit.point + Vector3.up * 0.001f;
                }
                else   // If the thing it his is an invalid teleport target
                {
                    // Set the beam to be invalid
                    SetTeleportValid(false);
                }

            }
            else      // If we didn't hit anything
            {
                // If we didn't hit anything
                // Set the beam to be invalid
                SetTeleportValid(false);
            }

        }
        // If the thumbstick has been released or is not being pushed forward
        else
        {
            // Hide the teleport beam
            SetBeamVisible(false);

            // If we have a valid teleport target
            if (hasValidTeleportTarget)
            {
                // Teleport the player there
                player.position = teleportIndicator.transform.position;

                // Remove the target indicator
                teleportIndicator.SetActive(false);
            }

        }
    }

    private bool IsValidTeleportTarget(GameObject gameObject)
    {
        return !(gameObject.layer == invalidTargetLayer);
    }

    private void SetBeamVisible(bool visible)
    {
        beam.enabled = visible;
    }

    private void SetTeleportValid(bool valid)
    {
        // Set the appropriate colour of the beam
        beam.material.color = valid ? validColour : invalidColour;

        // Show or hide the teleport indicator
        teleportIndicator.SetActive(valid);

        // Remember if we have a valid target or not
        hasValidTeleportTarget = valid;
    }

    private void SetBeamEndPoint(Vector3 endPoint)
    {
        // Set the start and end positions of the beam
        beam.SetPosition(0, transform.position);    // set start position: 0 is index of start point
        beam.SetPosition(1, endPoint);              // set end position: 1 is index of start point
    }
}
