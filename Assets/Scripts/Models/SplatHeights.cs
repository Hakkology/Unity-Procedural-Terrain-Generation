using UnityEngine;

[System.Serializable]
public class SplatHeights
{
    public Texture2D texture = null;
    public Texture2D textureNormalMap = null;
    public float minHeight = .1f; // height is between 0 and 1 assuming.
    public float maxHeight = .2f;
    public float minSlope = 0;
    public float maxSlope = 90;

    public float splatOffset = 0.1f;
    public float splatNoiseXScale = 0.01f;
    public float splatNoiseYScale = 0.01f;
    public float splatNoiseZScale = 0.1f;
    public Vector2 tileOffset = Vector2.zero;
    public Vector2 tileSize = new Vector2(50.0f, 40.0f);

    public bool remove = false;
}