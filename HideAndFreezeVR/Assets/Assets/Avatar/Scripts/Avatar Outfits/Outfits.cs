using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

[CreateAssetMenu(fileName = "OutfitsPreset", menuName = "Outfit/Create Outfit", order = 1)]
public class Outfits : ScriptableObject {

    public VRIK avatar;
    public List<Outfit> outfits;

    public Outfits(List<Outfit> outfits, VRIK avatar)
    {
        this.outfits = outfits;
        this.avatar = avatar;
    }
}
