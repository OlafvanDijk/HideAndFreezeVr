using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleAvatar : MonoBehaviour {

    [SerializeField]
    [Tooltip("Avatar Controller that is stored on the player.")]
    private VRAvatarController avatarController;
    [SerializeField]
    [Tooltip("Amount of times to calculate the medians.")]
    private int timesToScale;

    private GameObject eye;
    private List<float> scales;
    private GameObject avatar;
    private bool scaling;

    private int t;
    private int countScale = 0;

    public UnityEvent heightCalcDone = new UnityEvent();
    public UnityEvent scalingDone = new UnityEvent();

    /// <summary>
    /// Scales when the "S" key has been pressed.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Setup();
        }
    }

    /// <summary>
    /// Begin scaling when a collider has entered the trigger.
    /// </summary>
    /// <param name="other">Collider of the object that touched this.</param>
    private void OnTriggerEnter(Collider other)
    {
        Setup();
    }

    /// <summary>
    /// Setup variables that are needed for the calculation.
    /// After the setup has been done start the calculation.
    /// </summary>
    private void Setup()
    {
        try
        {
            if (!scaling)
            {
                scales = new List<float>();
                if (avatarController != null)
                {
                    scaling = true;
                    avatar = avatarController.actualAvatarVRIK.gameObject;
                    VRReferences references = avatar.GetComponent<VRReferences>();
                    StartCoroutine(StartCalculation());
                }
            }
        }
        catch (System.Exception)
        {
            scaling = false;
        }
       
    }

    /// <summary>
    /// Calls the Calculate function for the amount of times that has been given to the variable "timesToScale".
    /// </summary>
    /// <returns>Delay of 0.3 seconds.</returns>
    private IEnumerator StartCalculation()
    {
        int time = timesToScale;
        for (int x = 0; x<time; x++)
        {
            scales.Add(Calculate(avatarController.gameObject));
            heightCalcDone.Invoke();
            yield return new WaitForSeconds(0.3f);
        }
        ApplyScale(CalculateMedian());
    }

    /// <summary>
    /// Calculates the player's height.
    /// </summary>
    /// <param name="heightObject"></param>
    /// <returns>Calculated height.</returns>
    private float Calculate(GameObject heightObject)
    {
        Ray ray = new Ray(heightObject.transform.position, Vector3.down);
        RaycastHit hit;
        float height = -1;

        if (Physics.Raycast(ray, out hit, 5))
        {
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("IgnoreRaycast"))
            {
                height = hit.distance;
            }
        }
        return height;
    }

    /// <summary>
    /// Calculates the median in the scales list.
    /// </summary>
    /// <returns>The median height.</returns>
    private float CalculateMedian()
    {
        float median;
        int scalesCount = scales.Count;
        scales.Sort();
        foreach (float item in scales)
        {
            Debug.Log("median: " + item);
        }
        // Even amount of elements
        if (scales.Count % 2 == 0)
        {
            median = 0.5f * (scales[scalesCount / 2 - 1] + scales[scalesCount / 2]);
        }
        // Odd amount of elements
        else
        {
            median = scales[(scalesCount + 1) / 2 - 1];
        }
        return median;
    }

    /// <summary>
    /// Applies the new scale to the player.
    /// Also invokes the scalingDone event.
    /// </summary>
    /// <param name="scale">Median height.</param>
    private void ApplyScale(float scale)
    {
        scalingDone.Invoke();
        scale *= 0.7f;
        avatar.transform.localScale = new Vector3(scale, scale, scale);
        AvatarManager.Instance.ScaleChanged(scale);
        VR_PlayerNetwork.Instance.playerData.scale = scale;
        scaling = false;
    }
}
