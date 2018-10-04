using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AvatarSet", menuName = "AvatarData", order = 1)]
public class Avatars : ScriptableObject {

    [SerializeField]
    private List<GameObject> listOfAvatars;

    public GameObject getAvatarWithHead(int number)
    {
        if (number > listOfAvatars.Count)
        {
            number = 0;
        }
        if(number < 0)
        {
            number = listOfAvatars.Count;
        }

        return listOfAvatars[number];
    }

    public GameObject getAvatarWithoutHead(int number)
    {
        GameObject avatar = getAvatarWithHead(number);
        avatar.GetComponent<VRReferences>().HideHead();
        return avatar;
    }
    
}
