using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RootMotion.FinalIK;

public class ChooseAvatar : MonoBehaviour {

    [SerializeField]
    private List<Outfits> avatarOutfits;
    [SerializeField]
    private List<RawImage> colors;
    [SerializeField]
    private List<Image> outlines;
    [SerializeField]
    private VRAvatarController avatarController;
    [SerializeField]
    private GameObject placeHolder;

    public bool avatarOrOutfit;

    private List<Outfit> savedOutfits;
    private List<VRIK> avatarPrefabs;
    private int currentAvatar = -1;
    private int currentOutfit = 0;
    private VRIK avatar;

    void Start () {
        avatarOrOutfit = true;
        avatarPrefabs = new List<VRIK>();
        savedOutfits = new List<Outfit>();
        StartCoroutine(WaitForVRSetup());
    }

    public void PickAvatar()
    {
        try
        {
            if (avatarController.indexActualAvatar != currentAvatar)
            {
                avatarController.ChangeAvatar(currentAvatar);
            }
            PickColor();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
        
    }

    private void SetPlaceholder()
    {
        //TODO Deze gebruikt de models voor de andere spelers (Met hoofd).
        avatar = Instantiate(avatarPrefabs[currentAvatar], placeHolder.transform);
        savedOutfits.Clear();
        savedOutfits.AddRange(avatarOutfits[currentAvatar].outfits);
        //ChangeColors();
    }

    public void PickColor()
    {
        try
        {
            GameObject avatarObject = avatarController.actualAvatarVRIK.gameObject;
            ChangeOutfit changeOutfit = avatarObject.GetComponent<ChangeOutfit>();
            changeOutfit.ChangeClothes(savedOutfits[currentOutfit].texture);
        }
        catch (System.Exception)
        {
            Debug.Log("Outfit could not be changed.");
        }
    }

    private void ChangeColors()
    {
        for (int i = 0; i < savedOutfits.Count; i++)
        {
            colors[i].color = savedOutfits[i].color;
        }
    }

    private void ToggleOutlineAvatar(int index)
    {
        outlines[index].enabled = !outlines[index].enabled;
    }

    private void PickColorPlaceholder()
    {
        ChangeOutfit changeOutfit = avatar.GetComponent<ChangeOutfit>();
        changeOutfit.ChangeClothes(savedOutfits[currentOutfit].texture);
    }

    public void NextOrPrevious(bool nextOrPrevious, bool avatarOrOutfitbool)
    {
        int value = (nextOrPrevious) ? 1 : -1;
        switch (avatarOrOutfitbool)
        {
            case true:
                currentAvatar = currentAvatar + value;
                currentOutfit = 0;
                SetTrackers(ref currentAvatar, avatarPrefabs.Count);
                Destroy(avatar.gameObject);
                SetPlaceholder();
                break;
            case false:
                currentOutfit = currentOutfit + value;
                SetTrackers(ref currentOutfit, savedOutfits.Count);
                PickColorPlaceholder();
                break;
        }
        PickAvatar();
    }

    private void SetTrackers(ref int current, int listCount)
    {
        if (current >= listCount)
        {
            current = 0;
        }
        else if (current < 0)
        {
            current = listCount - 1;
        }
    }

    private IEnumerator WaitForVRSetup()
    {
        yield return new WaitUntil(() => avatarController != null);
        yield return new WaitUntil(() => avatarController.indexActualAvatar > -1);
        if (avatarController != null)
        {
            //TODO verwijs hier naar avatars zonder hoofd.
            avatarPrefabs.AddRange(avatarController.avatarPrefab);
            currentAvatar = avatarController.indexActualAvatar;
        }
        //SetOutfits();
        Invoke("SetPlaceholder", 0.5f);
    }
}
