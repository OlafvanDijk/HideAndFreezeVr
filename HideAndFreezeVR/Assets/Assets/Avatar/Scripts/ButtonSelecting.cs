using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(VRTK.VRTK_ControllerEvents))]
public class ButtonSelecting : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField]
    private VRTK.VRTK_ControllerEvents inputManager;

    private bool selectedUIItem;

    [SerializeField]
    private HandSide side;

    [SerializeField]
    private float rayDistance;

    private bool lineActive;
    [SerializeField]
    private bool delayActive;

    [SerializeField]
    [Tooltip("Position one for not selecting anything. Position two for when its selecting something")]
    private Material[] selectionColors;

    private Ray rayCast;
    private RaycastHit rayCastHit;

    private void Start()
    {
        lineActive = false;
        selectedUIItem = false;
        delayActive = false;

        lineRenderer = GetComponent<LineRenderer>();
        inputManager = GetComponent<VRTK.VRTK_ControllerEvents>();

        HandAlias handAlias = GetComponentInChildren<HandAlias>();
        if (handAlias != null)
        {
            side = GetComponentInChildren<HandAlias>().side;
        }

        StartCoroutine(Delay(.1f));
    }

    private void Update()
    {
        rayCast = new Ray(transform.position, transform.forward);

        Debug.DrawRay(rayCast.origin, rayCast.direction * rayDistance);


        if (Physics.Raycast(rayCast, out rayCastHit, rayDistance) && rayCastHit.collider.gameObject.layer == 5)
        {
            lineActive = true;
        }
        else
        {
            lineActive = false;
        }

        //Disable/Enable's the line renderer depending on the state of the bool
        if (lineRenderer.enabled != lineActive)
        {
            lineRenderer.enabled = lineActive;
        }

        switch (lineActive)
        {
            case true:
                lineRenderer.SetPosition(0, transform.position);

                RayCast();
                break;
        }

        if(OVRManager.isHmdPresent && !delayActive && !selectedUIItem)
        {
            switch(side)
            {
                case HandSide.Left:
                    if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
                    {
                        lineActive = !lineActive;
                        StartCoroutine(Delay(.1f));
                    }
                    break;
                case HandSide.Right:
                    if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                    {
                        lineActive = !lineActive;
                        StartCoroutine(Delay(.1f));
                    }
                    break;
                default:
                    Debug.LogWarning("Please assign the handside");
                    break;
            }
        }
    }

    public HandSide GethandSide()
    {
        return side;
    }

    private void RayCast()
    {
        try
        {
            if (Physics.Raycast(rayCast, out rayCastHit, 200) && rayCastHit.collider.GetComponent<UnityEngine.UI.Button>() != null)
            {
                //Change the state of the bool
                if (inputManager.triggerClicked && !OVRManager.isHmdPresent && !delayActive)
                {
                    UnityEngine.UI.Button button = rayCastHit.collider.GetComponent<UnityEngine.UI.Button>();

                    button.onClick.Invoke();
                    StartCoroutine(Delay(.1f));
                }
                else if (OVRManager.isHmdPresent && !delayActive)
                {
                    switch (side)
                    {
                        case HandSide.Left:
                            ButtonClick(OVRInput.Controller.LTouch);
                            break;
                        case HandSide.Right:
                            ButtonClick(OVRInput.Controller.RTouch);
                            break;
                        default:
                            Debug.Log("Assign a handside");
                            break;
                    }
                }
                selectedUIItem = true;

                //Set the line color to green and snap the position of the line
                lineRenderer.material = selectionColors[1];
            }
            else
            {
                selectedUIItem = false;

                //Set the line color to red and allow the line to follow the ray
                lineRenderer.material = selectionColors[0];
            }
            lineRenderer.SetPosition(1, transform.position + (transform.forward * rayCastHit.distance));
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    private void ButtonClick(OVRInput.Controller controller)
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller))
        {
            UnityEngine.UI.Button button = rayCastHit.collider.GetComponent<UnityEngine.UI.Button>();

            button.onClick.Invoke();

            StartCoroutine(Delay(.1f));
        }
    }

    private IEnumerator Delay(float delaySeconds)
    {
        float delay = delaySeconds * 2;
        delayActive = true;

        while (delay > 0)
        {
            delay -= delaySeconds;
            yield return new WaitForSeconds(delaySeconds);
        }

        delayActive = false;
    }
}
