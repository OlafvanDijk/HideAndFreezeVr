﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RootMotion.FinalIK;

public class SpawnAvatarInSceneTest : MonoBehaviour {

    [SerializeField]
    VRAvatarController VRSetup;
    [SerializeField]
    List<VRIK> avatarPrefab;

    [SerializeField]
    Toggle avatarToggle;
    [SerializeField]
    Toggle controllerToggle;
    [SerializeField]
    Toggle selectToggle;
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
        selectToggle.onValueChanged.AddListener(delegate
        {
            ToggleButtonSelection();
        });
        VRSetup.showControllers = false;
    }

    private void ToggleAvatar()
    {
        if (avatarToggle.isOn)
        {
            AvatarManager.Instance.avatarset.listOfAvatars = avatarPrefab;
        }
        else
        {
            AvatarManager.Instance.avatarset.listOfAvatars = null;
        }
    }

    private void ToggleControllers()
    {
        VRSetup.showControllers = controllerToggle.isOn;
    }

    private void ToggleButtonSelection()
    {
        VRSetup.enableButtonSelecting = selectToggle.isOn;
    }

    public void SpawnPlayer()
    {
        VRSetup.gameObject.SetActive(true);
        canvasCamera.enabled = false;
        this.gameObject.SetActive(false);
    }
}
