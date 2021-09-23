using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Throwable : Grabbable
{
    public int numVelocitySamples;

    private Queue<Vector3> previousVelocities = new Queue<Vector3>();
    private FixedJoint joint;
    private Vector3 prevPosition;
    private int throwBoost = 175;

    public override void OnGrab(Grabber grabber)
    {
        // Add a fixed joint between this object's rigidbody and the grabber's rigidbody
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = grabber.GetComponent<Rigidbody>();

    }

    public override void OnDrop()
    {
        // Remove the fixed joint
        Destroy(joint);

        // Calculate the average velocity from all valocity samples

        Vector3 averageVelocity = Vector3.zero;
        foreach(Vector3 velocity in previousVelocities)
        {
            averageVelocity += velocity;
        }

        averageVelocity /= previousVelocities.Count;

        // Apply the calculated average velocity to the rigidbody to move it (with optional boost)
        GetComponent<Rigidbody>().velocity = averageVelocity * throwBoost; 
        
    }

    public void Update()
    {
        // Note that Grabbable does NOT have an update method

        // Calculate the velocity of the object since the last update
        Vector3 velocity = transform.position - prevPosition;
        prevPosition = transform.position;

        // Add this calculated velocity to the list of previous velocities
        //TODO: this would be better if it was time based so it would be frame rate independent
        previousVelocities.Enqueue(velocity);

        // Make sure we don't store too many velocity samples
        if(previousVelocities.Count > numVelocitySamples)
        {
            // Toss out the oldest sample (dequeue without collecting)
            previousVelocities.Dequeue();
        }
        
    }
}
