﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RootMotion.FinalIK;

[RequireComponent(typeof(VRAvatarController))]
public class ChooseAvatar : MonoBehaviour {

    [SerializeField]
    private List<Outfits> avatarOutfits;
    [SerializeField]
    private RawImage color1;
    [SerializeField]
    private RawImage color2;
    [SerializeField]
    private RawImage color3;

    private List<Outfit> savedOutfits;
    private VRAvatarController avatarController;


    // Use this for initialization
    void Start () {
        savedOutfits = new List<Outfit>();
        avatarController = GetComponent<VRAvatarController>();
        Invoke("SetOutfits", 1f);
	}
	
    public void PickAvatar(int indexAvatar)
    {
        avatarController.ChangeAvatar(indexAvatar);
        Invoke("SetOutfits", 0.5f);
    }

    private void SetOutfits()
    {
        Debug.Log("Setoutfits");
        savedOutfits.Clear();
        savedOutfits.AddRange(avatarOutfits[avatarController.indexActualAvatar].outfits);
        Debug.Log(savedOutfits[1]);
        ChangeColors();
    }

    public void PickColor(int indexColor)
    {
        try
        {
            GameObject avatarObject = avatarController.actualAvatarVRIK.gameObject;
            ChangeOutfit changeOutfit = avatarObject.GetComponent<ChangeOutfit>();
            Debug.Log(changeOutfit);
            changeOutfit.ChangeClothes(savedOutfits[indexColor].texture);
        }
        catch (System.Exception)
        {
            Debug.Log("Outfit could not be changed.");
        }
    }

    private void ChangeColors()
    {
        color1.color = savedOutfits[0].color;
        color2.color = savedOutfits[1].color;
        color3.color = savedOutfits[2].color;
    }
}