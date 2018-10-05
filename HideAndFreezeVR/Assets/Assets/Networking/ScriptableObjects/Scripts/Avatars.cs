using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

[CreateAssetMenu(fileName = "AvatarSet", menuName = "AvatarData", order = 1)]
public class Avatars : ScriptableObject {

    public List<VRIK> listOfAvatars;
    [SerializeField]
    private List<Outfits> listOfOutfits;

    private VRIK GetAvatar(int number)
    {
        if (number > listOfAvatars.Count)
        {
            number = 0;
        }
        if (number < 0)
        {
            number = listOfAvatars.Count;
        }

        return listOfAvatars[number];
    }

    public VRIK getAvatarWithHead(int number)
    {
        VRIK avatar = GetAvatar(number);
        avatar.GetComponent<VRReferences>().ShowHead();
        return avatar;
    }

    public VRIK getAvatarWithoutHead(int number)
    {
        VRIK avatar = getAvatarWithHead(number);
        avatar.GetComponent<VRReferences>().HideHead();
        return avatar;
    }

    public Outfits getOutfits(int index)
    {
        return listOfOutfits[index];
    }
    
}
