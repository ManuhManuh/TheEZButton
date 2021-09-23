using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public string gripInputName;
    public string triggerInputName;

    private Touchable touchedObject;
    private Grabbable grabbedObject;
    
    // Update is called once per frame
    void Update()
    {
        // If the grip button is pressed
        if (Input.GetButtonDown(gripInputName))
        {
            // Update the animator to play the grip animation
            GetComponent<Animator>().SetBool("Gripped", true);

            // If we are touching an grabbable object, grab it
            if (grabbedObject != null)

            {
                // Let the touched object know that it has been grabbed
                grabbedObject.OnGrab(this);
            }

        }

        // If the grip button is released
        if(Input.GetButtonUp(gripInputName))
        {
            // Update the animator to stop the grip animation
            GetComponent<Animator>().SetBool("Gripped", false);

            // If we have something grabbed, drop it
            if (grabbedObject != null)
            {
                // Let the touched object know it has been dropped
                grabbedObject.OnDrop();

                // Forget the grabbed object
                grabbedObject = null;
            }
                
        }

        // If the trigger button is pressed
        if (Input.GetButtonDown(triggerInputName))
        {
            // If we are grabbing an object, call the trigger function
            if (grabbedObject != null)
            {
                // Let the grabbed object know that it has been triggered
                grabbedObject.OnTriggerStart();
            }
        }

        // If the trigger button is released
        if(Input.GetButtonUp(triggerInputName))
        {
            // If we have something grabbed, call the stop trigger function
            if (grabbedObject != null)
            {
                // Let the touched object know it has stopped being triggered
                grabbedObject.OnTriggerEnd();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object we touched is touchable (has the touchable script on it) - this just changes the colour of the object
        Touchable touchable = other.GetComponent<Touchable>();

        if (touchable != null)
        {
            // Let the object know it was touched
            touchable.OnTouched();

            // Store the currently touched object
            touchedObject = touchable;
        }

        // Check if the object we touched is grabbable (has the grabbable script on it)
        Grabbable grabbable = other.GetComponent<Grabbable>();

        if (grabbable != null)
        {
            // Store the current grabbable object
            grabbedObject = grabbable;
        }

    }

    private void OnTriggerExit(Collider other)
    {

        // Check if the object we stopped touching was touchable (has the touchable script on it) 
        if (touchedObject != null)
        {
            // Let the object know it is no longer being touched
            touchedObject.OnUntouched();

            // Reset the touched object
            touchedObject = null;

        }

    }

 
}
