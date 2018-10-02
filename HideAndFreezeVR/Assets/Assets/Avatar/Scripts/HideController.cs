using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideController : MonoBehaviour {

    [SerializeField]
    private MultiVRSetup multiVRSetup;
    [SerializeField]
    private bool showControllers;
    [SerializeField]
    private bool enableButtonSelecting;

    void Awake()
    {
        Invoke("Delay", 2);
    }

    /// <summary>
    /// Turns the controllers (in)visible based on the SDK used.
    /// </summary>
    /// <param name="value"></param>
    private void ShowVRController(bool value)
    {
        PlayAreaType playArea = multiVRSetup.playAreaAlias.playArea.type;
        Transform controller = this.transform.parent;
        Transform controllerClone = controller.parent;
        GameObject controllerCloneParent = controllerClone.parent.gameObject;
        switch (playArea)
        {
            case PlayAreaType.SIMULATED:
                break;
            case PlayAreaType.STEAM:
                if (!value)
                {
                    SteamVR_RenderModel renderModel = controllerCloneParent.GetComponentInChildren<SteamVR_RenderModel>();

                    foreach (Transform child in renderModel.transform)
                    {
                        Renderer renderer = child.GetComponent<MeshRenderer>();

                        if (renderer != null)
                        {
                            renderer.enabled = value;
                        }
                    }
                }
                break;
            case PlayAreaType.OCULUS:
                OvrAvatar avatar = multiVRSetup.playAreaAlias.GetComponentInChildren<OvrAvatar>(true);
                avatar.ShowFirstPerson = value;
                avatar.ShowControllers(value);
                
                break;
            case PlayAreaType.GEAR:
                break;
            default:
                break;
        }
        EnableButtonSelecting(controllerCloneParent, enableButtonSelecting);
    }

    /// <summary>
    /// Enables/Disables the button selection for the given controller based on the value.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="value"></param>
    private void EnableButtonSelecting(GameObject controller, bool value)
    {
        ButtonSelecting buttonSelecting = controller.GetComponent<ButtonSelecting>();
        if (buttonSelecting != null)
        {
            LineRenderer lineRenderer = controller.GetComponent<LineRenderer>();
            lineRenderer.enabled = value;
            buttonSelecting.enabled = value;
        }
    }

    /// <summary>
    /// Is Invoked with a delay so the playArea can be set.
    /// </summary>
    private void Delay()
    {
        ShowVRController(showControllers);
    }

    /// <summary>
    /// Disables/Enables the controllor and button selecting. This method is for outside calls.
    /// </summary>
    /// <param name="toggleController"></param>
    /// <param name="toggleButtonSelecting"></param>
    public void ToggleShowControllers(bool toggleController, bool toggleButtonSelecting)
    {
        showControllers = toggleController;
        enableButtonSelecting = toggleButtonSelecting;
        Invoke("Delay", 2.0f);
    }
}
