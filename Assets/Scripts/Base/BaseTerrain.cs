using UnityEditor;
using UnityEngine;

public class BaseTerrain : MonoBehaviour, IGeneratable
{
    protected int heightMapRes => terrainData.heightmapResolution;
    protected float[,] GetHeights() => terrainData.GetHeights(0, 0, heightMapRes, heightMapRes);
    protected float[,] GetHeightMap()
    {
        if (!resetTerrain)
            return GetHeights();
        else
            return new float[heightMapRes, heightMapRes];
    }

    public Terrain terrain;  
    public TerrainData terrainData;
    public bool resetTerrain = true;

    void OnEnable()
    {
        Debug.Log("Initializing terrain data");
        //terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
    }

    void Start()
    {
        SerializedObject tagManager = new SerializedObject(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]
        );

        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        AddTag(tagsProp, "Terrain");
        AddTag(tagsProp, "Cloud");
        AddTag(tagsProp, "Shore");
        tagManager.ApplyModifiedProperties();
        this.gameObject.tag = "Terrain";
    }

    void AddTag(SerializedProperty tagsProp, string newTag)
    {
        bool found = false;
        for (var i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(newTag)) { found = true; break; }
        }

        if (!found)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(0);
            newTagProp.stringValue = newTag;
        }
    }

    public void ResetTerrain()
    {
        float[,] heightMap = new float[heightMapRes, heightMapRes];
        terrainData.SetHeights(0, 0, heightMap);
    }
    
    /// <summary>
    /// base is empty.
    /// </summary> <summary>
    /// 
    /// </summary>
    public virtual void GenerateTerrain()
    {
        Debug.LogWarning($"{this.GetType().Name} has not implemented GenerateTerrain()");
    }

}

