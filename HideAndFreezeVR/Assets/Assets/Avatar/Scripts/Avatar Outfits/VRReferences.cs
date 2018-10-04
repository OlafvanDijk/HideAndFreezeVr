using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRReferences : MonoBehaviour {

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
    

    private void Start()
    {
        foreach (GameObject partToHide in partsToHide)
        {
            HidePartsInChildren(partToHide);
        }
    }
    

    private void HidePartsInChildren(GameObject parent)
    {
        parent.layer = layerToHideNumber;
        foreach (Transform child in parent.transform)
        {
            HidePartsInChildren(child.gameObject);
        }
    }
}

    private void Start()
    {
        foreach (GameObject partToHide in partsToHide)
        {
            HidePartsInChildren(partToHide);
        }
    private void HidePartsInChildren(GameObject parent)
    {
        parent.layer = layerToHideNumber;
        foreach (Transform child in parent.transform)
        {
            HidePartsInChildren(child.gameObject);
        }