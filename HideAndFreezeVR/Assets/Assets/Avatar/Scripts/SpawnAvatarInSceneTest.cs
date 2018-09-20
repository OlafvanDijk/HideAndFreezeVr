using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RootMotion.FinalIK;

public class SpawnAvatarInSceneTest : MonoBehaviour {

    [SerializeField]
    VRAvatarController VRSetup;
    [SerializeField]
    VRIK avatarPrefab;

    [SerializeField]
    Toggle avatarToggle;
    [SerializeField]
    Toggle controllerToggle;
    [SerializeField]
    Camera canvasCamera;

    private void Start()
    {
        avatarToggle.onValueChanged.AddListener(delegate
        {
            ToggleAvatar();
        });
        controllerToggle.onValueChanged.AddListener(delegate
        {
            ToggleControllers();
        });
        VRSetup.showControllers = false;
    }

    private void ToggleAvatar()
    {
        if (avatarToggle.isOn)
        {
            VRSetup.avatarPrefab = avatarPrefab;
        }
        else
        {
            VRSetup.avatarPrefab = null;
        }
    }

    private void ToggleControllers()
    {
        VRSetup.showControllers = controllerToggle.isOn;
    }

    public void SpawnPlayer()
    {
        VRSetup.gameObject.SetActive(true);
        canvasCamera.enabled = false;
        this.gameObject.SetActive(false);
    }
}
