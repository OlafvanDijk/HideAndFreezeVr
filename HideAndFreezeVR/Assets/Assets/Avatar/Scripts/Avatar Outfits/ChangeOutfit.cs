using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOutfit : MonoBehaviour {

    [SerializeField]
    private int indexMaterial;
    [SerializeField]
    GameObject body;

    public void ChangeClothes(Texture texture)
    {
        SkinnedMeshRenderer skinnedMeshRenderer = body.GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.materials[indexMaterial].mainTexture = texture;
    }
}
