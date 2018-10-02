using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOutfit : MonoBehaviour {

    [SerializeField]
    private List<MaterialBody> materialBodies;

    public void ChangeClothes(List<Texture> textures)
    {
        try
        {
            for (int i = 0; i < textures.Count; i++)
            {
                GameObject body = materialBodies[i].body;
                SkinnedMeshRenderer skinnedMeshRenderer = body.GetComponent<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer != null)
                {
                    skinnedMeshRenderer.materials[materialBodies[i].indexMaterial].mainTexture = textures[i];
                }
                else
                {
                    MeshRenderer meshRenderer = body.GetComponent<MeshRenderer>();
                    meshRenderer.materials[materialBodies[i].indexMaterial].mainTexture = textures[i];
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
