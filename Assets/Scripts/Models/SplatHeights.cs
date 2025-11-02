using UnityEngine;

[System.Serializable]
public class SplatHeights
{
    public Texture2D texture = null;
    public float minHeight = .1f; // height is between 0 and 1 assuming.
    public float maxHeight = .2f;
    public bool remove = false;
}