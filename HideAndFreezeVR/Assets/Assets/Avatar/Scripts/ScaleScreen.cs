using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleScreen : MonoBehaviour {

    [SerializeField]
    [Tooltip("Scaling script for the Avatar.")]
    private ScaleAvatar scaleAvatar;
    [SerializeField]
    [Tooltip("The main text field to be changed.")]
    private Text panelText;
    [SerializeField]
    [Tooltip("Loading bar of the panel.")]
    private Slider loadingBar;
    [SerializeField]
    [Tooltip("Mirror object to show/hide.")]
    private GameObject mirror;
    [SerializeField]
    [Tooltip("Time after scaling until you can press the button again.")]
    private float waitUntilReset;
    [SerializeField]
    [Tooltip("Colors of the background.")]
    private List<Color> buttonBackgroundColors;

    private bool reset;
    private float delay;

	/// <summary>
    /// Add listeners and set the delay.
    /// </summary>
	void Start () {
        scaleAvatar.heightCalcDone.AddListener(ChangeLoadingBar);
        scaleAvatar.scalingDone.AddListener(DoneScaling);
        delay = waitUntilReset;
	}
	
	/// <summary>
    /// Reset's the scaling process
    /// </summary>
	void Update () {
        if (reset)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                ChangeBackground();
                reset = false;
                delay = waitUntilReset;
                loadingBar.value = loadingBar.minValue;
                panelText.text = "Touch to Scale";
                GetComponent<BoxCollider>().enabled = true;
            }
        }
	}

    /// <summary>
    /// Updates the loading bar when the event heightCalcDone is getting invoked.
    /// </summary>
    private void ChangeLoadingBar()
    {        
        if (loadingBar.value == loadingBar.minValue)
        {
            ChangeBackground();
            GetComponent<BoxCollider>().enabled = false;
            loadingBar.gameObject.SetActive(true);
            panelText.text = "Scaling...";
        }
        loadingBar.value++;
    }

    /// <summary>
    /// Done scaling is getting called when the scalingDone event has been invoked.
    /// This sets the canvas to the next state.
    /// </summary>
    private void DoneScaling()
    {
        loadingBar.gameObject.SetActive(false);
        panelText.text = "Look in the mirror --------->";
        mirror.SetActive(true);
        reset = true;
    }

    /// <summary>
    /// Changes the emission of the button.
    /// </summary>
    private void ChangeBackground()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material backgroundMaterial = renderer.materials[0];
        Color emission;
        if (reset)
        {
            emission = buttonBackgroundColors[0];
        }
        else
        {
            emission = buttonBackgroundColors[1];
        }
        backgroundMaterial.SetColor("_EmissionColor", emission);
    }
}
