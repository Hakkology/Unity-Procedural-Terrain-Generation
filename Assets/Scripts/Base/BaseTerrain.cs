using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BaseTerrain : MonoBehaviour, IGeneratable
{
    [SerializeField]
    int terrainLayer = 0;
    int skyLayer = 0;

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
    public int smoothCount = 1;

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

        AddTag(tagsProp, "Terrain", TagType.Tag);
        AddTag(tagsProp, "Cloud", TagType.Tag);
        AddTag(tagsProp, "Shore", TagType.Tag);
        tagManager.ApplyModifiedProperties();

        SerializedProperty layerProp = tagManager.FindProperty("layers");
        terrainLayer = AddTag(layerProp, "Terrain", TagType.Layer);
        skyLayer = AddTag(layerProp, "Sky", TagType.Layer);
        tagManager.ApplyModifiedProperties();

        terrain.gameObject.tag = "Terrain";
        terrain.gameObject.layer = terrainLayer;
    }

    int AddTag(SerializedProperty tagsProp, string newTag, TagType tType)
    {
        bool found = false;
        for (var i = 0; i < tagsProp.arraySize; i++)
        {
            SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(newTag)) { found = true; return i; }
        }

        if (!found && tType == TagType.Tag)
        {
            tagsProp.InsertArrayElementAtIndex(0);
            SerializedProperty newTagProp = tagsProp.GetArrayElementAtIndex(0);
            newTagProp.stringValue = newTag;
        }
        else if (!found && tType == TagType.Layer)
        {
            for (int j = 8; j < tagsProp.arraySize; j++)
            {
                SerializedProperty newLayer = tagsProp.GetArrayElementAtIndex(j);
                if (newLayer.stringValue == "")
                {
                    newLayer.stringValue = newTag;
                    return j;
                }
            }
        }

        return -1;
    }

    public void ResetTerrain()
    {
        float[,] heightMap = new float[heightMapRes, heightMapRes];
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void SmoothTerrain()
    {
        if (resetTerrain)
        {
            Debug.Log("Can't smooth with reset.");
            return;
        }

        float[,] heightMap = GetHeightMap();
        int totalIterations = smoothCount;

        try
        {
            for (int i = 0; i < totalIterations; i++)
            {
                for (int y = 0; y < heightMapRes; y++)
                {
                    for (int x = 0; x < heightMapRes; x++)
                    {
                        float avgHeight = heightMap[x, y];
                        List<Vector2> neighbours = GenerateNeighbours(new Vector2(x, y), heightMapRes, heightMapRes);

                        foreach (Vector2 n in neighbours)
                            avgHeight += heightMap[(int)n.x, (int)n.y];

                        heightMap[x, y] = avgHeight / ((float)neighbours.Count + 1);
                    }

                    float progress = ((i * heightMapRes) + y) / (float)(totalIterations * heightMapRes);
                    EditorUtility.DisplayProgressBar("Smoothing Terrain...", $"Iteration {i + 1}/{totalIterations}", progress);
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    List<Vector2> GenerateNeighbours(Vector2 pos, int width, int height) {

        List<Vector2> neighbours = new List<Vector2>();

        for (int y = -1; y < 2; ++y) { // between -1 and 1.

            for (int x = -1; x < 2; ++x) {

                if (!(x == 0 && y == 0)) {

                    Vector2 nPos = new Vector2(
                        Mathf.Clamp(pos.x + x, 0.0f, width - 1),
                        Mathf.Clamp(pos.y + y, 0.0f, height - 1));

                    if (!neighbours.Contains(nPos))
                        neighbours.Add(nPos);
                }
            }
        }
        return neighbours;
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

