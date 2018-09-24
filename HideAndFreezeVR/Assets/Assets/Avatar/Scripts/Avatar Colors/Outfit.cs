using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Outfit
{
    public Texture texture;
    public Color color;

    public Outfit(Texture texture, Color color)
    {
        this.texture = texture;
        this.color = color;
    }
}
