using UnityEngine;

[System.Serializable]
public class SplatHeights
{
    public Texture2D texture = null;
    public float minHeight = .1f; // height is between 0 and 1 assuming.
    public float maxHeight = .2f;

    public float splatOffset = 0.1f;
    public float splatNoiseXScale = 0.01f;
    public float splatNoiseYScale = 0.01f;
    public float splatNoiseZScale = 0.1f;
    public bool remove = false;
}