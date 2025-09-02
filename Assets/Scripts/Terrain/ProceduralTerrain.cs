using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ProceduralTerrain : MonoBehaviour
{
    public Vector2 RandomHeightRange = new Vector2(0, 0.1f);

    public Texture2D heightMapImage;
    public Vector3 heightMapScale = new Vector3(1, 1, 1);
    
    public Terrain terrain;
    public TerrainData terrainData;

    void OnEnable()
    {
        Debug.Log("Initializing terrain data");
        terrain = GetComponent<Terrain>();
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

    public void RandomTerrain()
    {
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        for (int x = 0; x < terrainData.heightmapResolution; x++) // ++x on content, check if anything differs
        {
            for (var z = 0; z < terrainData.heightmapResolution; z++) // ++z on content, check if anything differs
            {
                heightMap[x, z] += Random.Range(RandomHeightRange.x, RandomHeightRange.y);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }

    public void ResetTerrain()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        terrainData.SetHeights(0, 0, heightMap);
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

    // Update is called once per frame
    void Update()
    {

    }
}
