using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(VRTK.VRTK_ControllerEvents))]
public class ButtonSelecting : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    [SerializeField]
    private VRTK.VRTK_ControllerEvents _inputManager;

    private bool _selectedUIItem;

    [SerializeField]
    private HandSide _side;

    [SerializeField]
    private float _rayDistance = 200; //200 is recommended, because of the current distance between the vr player and the ui 

    private bool _lineActive;
    [SerializeField]
    private bool _delayActive;

    [SerializeField]
    [Tooltip("Position one for not selecting anything. Position two for when its selecting something")]
    private Material[] _selectionColors;

    private void Start()
    {
        _lineActive = false;
        _selectedUIItem = false;
        _delayActive = false;

        _lineRenderer = GetComponent<LineRenderer>();
        _inputManager = GetComponent<VRTK.VRTK_ControllerEvents>();

        HandAlias handAlias = GetComponentInChildren<HandAlias>();
        if (handAlias != null)
        {
            _side = GetComponentInChildren<HandAlias>().side;
        }

        StartCoroutine(Delay(.1f));
    }
    private void Update()
    {
        //Disable/Enable's the line renderer depending on the state of the bool
        if (_lineRenderer.enabled != _lineActive) _lineRenderer.enabled = _lineActive;

        switch (_lineActive)
        {
            case true:
                _lineRenderer.SetPosition(0, transform.position);

                RayCast();
                break;
        }

        //Change the state of the bool
        if (_inputManager.triggerClicked && !OVRManager.isHmdPresent && !_delayActive && !_selectedUIItem)
        {
            _lineActive = !_lineActive;

            StartCoroutine(Delay(.1f));
        }
        else if(OVRManager.isHmdPresent && !_delayActive && !_selectedUIItem)
        {
            switch(_side)
            {
                case HandSide.Left:
                    if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
                    {
                        _lineActive = !_lineActive;
                        StartCoroutine(Delay(.1f));
                    }
                    break;
                case HandSide.Right:
                    if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                    {
                        _lineActive = !_lineActive;
                        StartCoroutine(Delay(.1f));
                    }
                    break;
                default:
                    Debug.LogWarning("Please assign the handside");
                    break;
            }
        }
    }

    private void RayCast()
    {
        Ray rayCast = new Ray(transform.position, transform.forward);
        RaycastHit rayCastHit;

        Debug.DrawRay(rayCast.origin, rayCast.direction * 200);

        if(Physics.Raycast(rayCast, out rayCastHit, 200) && rayCastHit.collider.GetComponent<UnityEngine.UI.Button>() != null)
        {
            //Change the state of the bool
            if (_inputManager.triggerClicked && !OVRManager.isHmdPresent && !_delayActive)
            {
                UnityEngine.UI.Button button = rayCastHit.collider.GetComponent<UnityEngine.UI.Button>();

                button.onClick.Invoke();
                //button.GetComponent<ButtonSibling>().InvokeSiblingAndOwnButton();

                StartCoroutine(Delay(.1f));
            }
            else if (OVRManager.isHmdPresent && !_delayActive)
            {
                switch (_side)
                {
                    case HandSide.Left:
                        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.LTouch))
                        {
                            UnityEngine.UI.Button button = rayCastHit.collider.GetComponent<UnityEngine.UI.Button>();

                            button.onClick.Invoke();

                            StartCoroutine(Delay(.1f));
                        }
                        break;
                    case HandSide.Right:
                        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
                        {
                            UnityEngine.UI.Button button = rayCastHit.collider.GetComponent<UnityEngine.UI.Button>();

                            button.onClick.Invoke();

                            StartCoroutine(Delay(.1f));
                        }
                        break;
                    default:
                        Debug.LogWarning("Please assign the handside");
                        break;
                }
            }

                _selectedUIItem = true;

            //Set the line color to green and snap the position of the line
            _lineRenderer.material = _selectionColors[1];
            _lineRenderer.SetPosition(1, rayCastHit.collider.transform.position);
        }
        else
        {
            _selectedUIItem = false;

            //Set the line color to red and allow the line to follow the ray
            _lineRenderer.material = _selectionColors[0];
            _lineRenderer.SetPosition(1, transform.position + (transform.forward * 200));
        }
    }

    private IEnumerator Delay(float delaySeconds)
    {
        float delay = delaySeconds * 2;
        _delayActive = true;

        while (delay > 0)
        {
            delay -= delaySeconds;
            yield return new WaitForSeconds(delaySeconds);
        }

        _delayActive = false;
    }
}
