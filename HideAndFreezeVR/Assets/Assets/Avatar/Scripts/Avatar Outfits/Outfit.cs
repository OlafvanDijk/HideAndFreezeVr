using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Outfit
{
    public List<Texture> texture;
    public Color color;

    public Outfit(List<Texture> texture, Color color)
    {
        this.texture = texture;
        this.color = color;
    }
}
