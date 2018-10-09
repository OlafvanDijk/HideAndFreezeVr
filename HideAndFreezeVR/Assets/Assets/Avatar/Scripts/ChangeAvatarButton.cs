using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvatarButton : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Specifies what kind of button this is.")]
    AvatarButton avatarButton;
    [SerializeField]
    [Tooltip("References the chooseAvatar script.")]
    private ChooseAvatar chooseAvatar;
    [SerializeField]
    [Tooltip("True if this button changes avatar. False if this button changes clothes.")]
    private bool avatarOrOutfit;

    /// <summary>
    /// Calls the corresponding method in the ChooseAvatar script depending on the avatarrButton type.
    /// </summary>
    public void PressButton()
    {
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
    }
}

public enum AvatarButton
{
    Previous,
    Next,
    Select
}