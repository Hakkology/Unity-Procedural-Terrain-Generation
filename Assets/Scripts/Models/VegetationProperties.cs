using UnityEngine;

[System.Serializable]
public class VegetationProperties
{
    public GameObject prefab = null;
    public float minHeight = .1f; // height is between 0 and 1 assuming.
    public float maxHeight = .2f;
    public float minSlope = 0;
    public float maxSlope = 90;
    public bool remove = false;
}