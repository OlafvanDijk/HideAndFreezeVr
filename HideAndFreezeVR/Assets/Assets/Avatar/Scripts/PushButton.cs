using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushButton : MonoBehaviour
{
    
    [SerializeField]
    [Tooltip("Button to be pressed down.")]
    GameObject button;
    [SerializeField]
    [Tooltip("Delay between touches.")]
    private float touchDelay;
    [SerializeField]
    [Tooltip("Two materials to change between.")]
    List<Material> materials;

    public UnityEvent buttonPressEvent;

    private float delay;
    private float onEnterDelay;
    private Vector3 oldScale;
    private bool pressed;
    private MeshRenderer meshRenderer;

    /// <summary>
    /// Set original scale.
    /// </summary>
    private void Start()
    {
        if (button != null)
        {
            oldScale = new Vector3(button.transform.localScale.x, button.transform.localScale.y, button.transform.localScale.z);
            meshRenderer = button.GetComponent<MeshRenderer>();
        }
    }

    /// <summary>
    /// Update timers.
    /// </summary>
    private void Update()
    {
        UpdateDelay(ref delay);
        UpdateDelay(ref onEnterDelay);
    }

    /// <summary>
    /// Works as a timer for the parameter that gets passed on.
    /// </summary>
    /// <param name="refDelay">Current time of the timer.</param>
    private void UpdateDelay(ref float refDelay)
    {
        if (refDelay > 0)
        {
            refDelay -= Time.deltaTime;
        }

        if (refDelay < 0)
        {
            refDelay = 0;
        }
    }

    /// <summary>
    /// Invokes the button press event if it wasn't pressed already.
    /// Also changes the scale of the button in the Y value
    /// </summary>
    /// <param name="other">Collider of the object that touches it.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (!pressed)
        {
            if (onEnterDelay == 0)
            {
                buttonPressEvent.Invoke();
                pressed = true;
                Vector3 scale = button.transform.localScale;
                scale.y = scale.y / 1.5f;
                button.transform.localScale = scale;
                onEnterDelay = touchDelay;
                ChangeMaterial(1);
            }
        }
    }

    /// <summary>
    /// Changes button back to normal if the delay timer has reached 0
    /// </summary>
    /// <param name="other">Collider of the object that touches it.</param>
    private void OnTriggerExit(Collider other)
    {
        if (delay == 0)
        {
            button.transform.localScale = oldScale;
            delay = touchDelay;
            pressed = false;
            ChangeMaterial(0);
        }
    }

    /// <summary>
    /// Changes the material of the button
    /// </summary>
    /// <param name="index">Index of the color from the "materials" list.</param>
    private void ChangeMaterial(int index)
    {
        if (materials != null && materials.Count > 0)
        {
            meshRenderer.material = materials[index];
        }
    }
}
