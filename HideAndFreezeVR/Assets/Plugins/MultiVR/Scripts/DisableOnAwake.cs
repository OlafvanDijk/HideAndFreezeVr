using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Disables an object once when it awakes, then deletes this.
/// </summary>
public class DisableOnAwake : MonoBehaviour
{
    private void Update() // Need this so that we can disable the behaviour.
    {
        
    }

    private void Awake()
    {
        if (!enabled || PlayArea.wasInitialized) // Don't do this if the play area was already initialized, because then the awake did not happen at the start of the scene.
        {
            Destroy(this);
            return;
        }

        gameObject.SetActive(false);

        Destroy(this);
    }
}
