using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideController : MonoBehaviour {

    [SerializeField]
    private HandAlias handAlias;
    [SerializeField]
    private MultiVRSetup multiVRSetup;
    [SerializeField]
    private GameObject steamVRController;

    /// <summary>
    /// Turns the controllers (in)visible based on the SDK used.
    /// </summary>
    /// <param name="value"></param>
    public void HideVRController(bool value)
    {
        PlayAreaType playArea = multiVRSetup.playAreaAlias.playArea.type;

        switch (playArea)
        {
            case PlayAreaType.SIMULATED:
                break;
            case PlayAreaType.STEAM:
                foreach (Transform child in steamVRController.transform)
                {
                    Renderer renderer = child.GetComponent<MeshRenderer>();

                    if (renderer != null)
                    {
                        renderer.enabled = value;
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
    }
}
