using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOutfit : MonoBehaviour {

    [SerializeField]
    private int indexMaterial;
    [SerializeField]
    GameObject body;
    [SerializeField]
    Material clothes;

    public void ChangeClothes(Texture texture)
    {
        clothes.SetTexture("clothes", texture);
        SkinnedMeshRenderer skinnedMeshRenderer = body.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.materials[indexMaterial].mainTexture = texture;
    }
}
