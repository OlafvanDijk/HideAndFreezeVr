using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvatarButton : MonoBehaviour {

    [SerializeField]
    AvatarButton avatarButton;
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private ChooseAvatar chooseAvatar;
    [SerializeField]
    private float touchDelay;
    [SerializeField]
    private bool avatarOrOutfit;
    private float onEnterDelay;
    private float delay;
    private Vector3 oldScale;

    private void Start()
    {
        oldScale = new Vector3(button.transform.localScale.x, button.transform.localScale.y, button.transform.localScale.z);
    }

    private void Update()
    {
        UpdateDelay(ref delay);
        UpdateDelay(ref onEnterDelay);
    }

    private void UpdateDelay(ref float refDelay)
    {
        if (refDelay > 0)
        {
            refDelay -= Time.deltaTime;
        }

        if (refDelay < 0)
        {
            refDelay = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onEnterDelay == 0)
        {
            Vector3 scale = button.transform.localScale;
            scale.y = scale.y / 1.5f;
            button.transform.localScale = scale;

            try
            {
                switch (avatarButton)
                {
                    case AvatarButton.Previous:
                        chooseAvatar.NextOrPrevious(false, avatarOrOutfit);
                        break;
                    case AvatarButton.Next:
                        chooseAvatar.NextOrPrevious(true, avatarOrOutfit);
                        break;
                    case AvatarButton.Select:
                        Debug.Log(other.gameObject);
                        chooseAvatar.PickAvatar();
                        break;
                    default:
                        break;
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
            onEnterDelay = touchDelay;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (delay == 0)
        {
            //Vector3 scale = button.transform.localScale;
            //scale.y = scale.y * 1.5f;
            //button.transform.localScale = scale;
            button.transform.localScale = oldScale;
            delay = touchDelay;
        }
    }

}

public enum AvatarButton
{
    Previous,
    Next,
    Select
}