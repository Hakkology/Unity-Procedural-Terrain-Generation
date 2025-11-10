using UnityEngine;

[System.Serializable]
public class DetailProperties
{
    public GameObject prototype = null;
    public Texture2D protoTypeTexture = null;
    public float minHeight = .1f; // height is between 0 and 1 assuming.
    public float maxHeight = .2f;
    public float minSlope = 0;
    public float maxSlope = 90;

    public Color dryColor = Color.white;
    public Color healthyColor = Color.white;
    public Vector2 heightRange = new Vector2(1, 1);
    public Vector2 widthRange = new Vector2(1, 1);
    public float noiseSpread = .5f;

    public float overlap = 0.01f;
    public float feather = 0.05f;
    public float density = 0.5f;
    public bool remove = false;
}