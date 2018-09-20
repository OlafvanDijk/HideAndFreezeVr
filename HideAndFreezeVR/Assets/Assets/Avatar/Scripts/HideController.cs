using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideController : MonoBehaviour {

    [SerializeField]
    private MultiVRSetup multiVRSetup;
    [SerializeField]
    private bool showControllers;

    void Awake()
    {
        Invoke("Delay", 2);
    }

    /// <summary>
    /// Turns the controllers (in)visible based on the SDK used.
    /// </summary>
    /// <param name="value"></param>
    public void ShowVRController(bool value)
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
               
                EnableButtonSelecting(controllerCloneParent, value);
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
                avatar.ShowFirstPerson = true;
                Debug.Log(value);
                avatar.ShowControllers(value);
                EnableButtonSelecting(controllerCloneParent, value);
                break;
            case PlayAreaType.GEAR:
                break;
            default:
                break;
        }
    }

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
}
