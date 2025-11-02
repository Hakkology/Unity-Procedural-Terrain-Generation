using UnityEngine;

[System.Serializable]
public class VegetationProperties
{
    public GameObject prefab = null;
    public float minHeight = .1f; // height is between 0 and 1 assuming.
    public float maxHeight = .2f;
    public float minSlope = 0;
    public float maxSlope = 90;

    public float minScale = .5f;
    public float maxScale = 1f;
    public float minRotation = 0.0f;
    public float maxRotation = 360.0f;
    public float density = 0.5f;

    public Color colour1 = Color.white;
    public Color colour2 = Color.white;
    public Color lightColour = Color.white;
    public bool remove = false;
}