using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarOutfitSwitch : MonoBehaviour {

    [SerializeField]
    private List<Material> colors;
    [SerializeField]
    private ChooseAvatar chooseAvatar;
    [SerializeField]
    private GameObject thisButton;
    [SerializeField]
    private AvatarOutfitSwitch otherButton;
    [SerializeField]
    private bool pressed;

    private float yPosition;

    private void Start()
    {
        yPosition = thisButton.transform.localPosition.y;
        SetPosition();
        int color = 0;
        if (!pressed)
        {
            color = 1;
        }
        MeshRenderer meshRenderer = thisButton.GetComponent<MeshRenderer>();
        meshRenderer.material = colors[color];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!pressed)
        {
            Switch();
            otherButton.Switch();
            chooseAvatar.avatarOrOutfit = !chooseAvatar.avatarOrOutfit;
        }
    }

    public void Switch()
    {
        pressed = !pressed;
        MeshRenderer meshRenderer = thisButton.GetComponent<MeshRenderer>();
        int color = 0;
        if (!pressed)
        {
            color = 1;            
        }
        meshRenderer.material = colors[color];
        SetPosition();
    }

    private void SetPosition()
    {
        float y = 0f;
        if (pressed)
        {
            y = 0.005f;
        }
        else
        {
            y = yPosition;
        }
        Vector3 pos = new Vector3(thisButton.transform.localPosition.x, y, thisButton.transform.localPosition.z);
        thisButton.transform.localPosition = pos;
    }
}
