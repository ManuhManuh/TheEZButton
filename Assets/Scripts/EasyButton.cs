using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyButton : ControlledObject
{

    public override void OnPressed()
    {
        // Play the button click sound
        SoundManager.PlaySound(gameObject, "ButtonClick");

        // Spawn an item
        SpawnManager.instance.SpawnOfficeSupplies();
    }

    public override void OnReleased()
    {
        // Play the button click sound
        SoundManager.PlaySound(gameObject, "ButtonClick");

    }
}
