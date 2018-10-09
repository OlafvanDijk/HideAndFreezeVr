using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRReferences : MonoBehaviour
{

    [SerializeField]
    private Transform head;
    [SerializeField]
    private Transform leftHand;
    [SerializeField]
    private Transform rightHand;
    [SerializeField]
    private List<GameObject> partsToHide = new List<GameObject>();
    [SerializeField]
    private int layerToHideNumber;


    private void HidePartsInChildren(GameObject parent, bool hideOrShow)
    {
        if (hideOrShow)
        {
            parent.layer = layerToHideNumber;
        }
        else
        {
            parent.layer = 0;
        }
        foreach (Transform child in parent.transform)
        {
            HidePartsInChildren(child.gameObject, hideOrShow);
        }
    }


    public void HideHead()
    {
        foreach (GameObject partToHide in partsToHide)
        {
            HidePartsInChildren(partToHide, true);
        }
    }
    public void ShowHead()
    {
        foreach (GameObject partToHide in partsToHide)
        {
            HidePartsInChildren(partToHide, false);
        }
    }
}