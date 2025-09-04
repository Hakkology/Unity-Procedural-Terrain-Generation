[System.Serializable]
public class PerlinParameters
{
    public float mPerlinXScale = 0.01f;
    public float mPerlinYScale = 0.01f;

    public int mPerlinXOffset = 0;
    public int mPerlinYOffset = 0;

    public int mPerlinOctaves = 3;
    public float mPerlinPersistance = 8;
    public float mPerlinHeightScale = 0.09f;

    public bool remove = false;
}