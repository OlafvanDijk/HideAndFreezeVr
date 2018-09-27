using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaterialBody {

    [SerializeField]
    public int indexMaterial;
    [SerializeField]
    public GameObject body;

    public MaterialBody(int indexMaterial, GameObject body)
    {
        this.indexMaterial = indexMaterial;
        this.body = body;
    }
}
