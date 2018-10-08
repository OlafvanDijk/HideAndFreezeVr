using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

[CreateAssetMenu(fileName = "AvatarSet", menuName = "AvatarData", order = 1)]
public class Avatars : ScriptableObject {

    public List<VRIK> listOfAvatars;
    [SerializeField]
    private List<Outfits> listOfOutfits;

    /// <summary>
    /// Get the avatar belonging to a specific number.
    /// </summary>
    /// <param name="number"> The number an avatar should belong to. </param>
    /// <returns> An gameobject with the VRIK script. </returns>
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

    /// <summary>
    /// Get the avatar belonging to a specific number with their head visible.
    /// </summary>
    /// <param name="number"> The number an avatar should belong to. </param>
    /// <returns> An gameobject with the VRIK script. </returns>
    public VRIK getAvatarWithHead(int number)
    {
        VRIK avatar = GetAvatar(number);
        avatar.GetComponent<VRReferences>().ShowHead();
        return avatar;
    }

    /// <summary>
    /// Get the avatar belonging to a specific number with their head invisible.
    /// </summary>
    /// <param name="number"> The number an avatar should belong to. </param>
    /// <returns> An gameobject with the VRIK script. </returns>
    public VRIK getAvatarWithoutHead(int number)
    {
        VRIK avatar = getAvatarWithHead(number);
        avatar.GetComponent<VRReferences>().HideHead();
        return avatar;
    }

    /// <summary>
    /// Get an Outfits scriptable object belonging to an index.
    /// </summary>
    /// <param name="index"> The index belonging to a specific Outfits scriptable object. </param>
    /// <returns> Am scriptable object. </returns>
    public Outfits getOutfits(int index)
    {
        return listOfOutfits[index];
    }

    /// <summary>
    /// Get the indexes belongin to a specific outfit.
    /// </summary>
    /// <param name="outfit"> An outfit class holding textures and colors. </param>
    /// <returns> The indexes belonging to an outfit's location. </returns>
    public int[] getOutfitIndex(Outfit outfit)
    {
        int[] location = new int[2] { 0, 0 };
        foreach(Outfits outfitslist in listOfOutfits)
        {
            foreach(Outfit singleOutfit in outfitslist.outfits)
            {
                if(singleOutfit == outfit)
                {
                    return location;
                }
                location[1]++;
            }
            location[1] = 0;
            location[0]++;
        }
        return null;
    }
}
