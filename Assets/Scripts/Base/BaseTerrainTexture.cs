using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseTerrainTexture : MonoBehaviour, ITexturable
{
    public Terrain terrain;  
    public TerrainData terrainData;
    public List<SplatHeights> splatHeights = new List<SplatHeights>()
    {
        new SplatHeights()
    };

    void OnEnable()
    {
        Debug.Log("Initializing terrain data");
        terrainData = terrain.terrainData;
    }

    public void SplatMaps()
    {
        TerrainLayer[] newSplatPrototypes;
        newSplatPrototypes = new TerrainLayer[splatHeights.Count];

        int spIndex = 0;

        foreach (SplatHeights sh in splatHeights)
        {
            newSplatPrototypes[spIndex] = new TerrainLayer();
            newSplatPrototypes[spIndex].diffuseTexture = sh.texture;
            newSplatPrototypes[spIndex].diffuseTexture.Apply(true);
            string path = "Assets/New TerrainLayer " + spIndex + ".terrainLayer";
            AssetDatabase.CreateAsset(newSplatPrototypes[spIndex], path);

            spIndex++;
            Selection.activeObject = this.gameObject;
        }

        terrainData.terrainLayers = newSplatPrototypes;
    }

    public void AddNewSplatHeight()
    {
        splatHeights.Add(new SplatHeights());
    }
    
    public void RemoveSplatHeights()
    {
        List<SplatHeights> keepSplatHeights = new List<SplatHeights>();

        for (int i = 0; i < splatHeights.Count; i++)
        {
            if (!splatHeights[i].remove)
            {
                keepSplatHeights.Add(splatHeights[i]);
            }
        }

        if (keepSplatHeights.Count == 0)
        {
            keepSplatHeights.Add(splatHeights[0]);
        }
        
        splatHeights = keepSplatHeights;
    }
}