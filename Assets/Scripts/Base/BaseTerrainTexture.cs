using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class BaseTerrainTexture : MonoBehaviour, ITexturable
{
    protected int heightMapRes => terrainData.heightmapResolution;
    protected float[,] GetHeights() => terrainData.GetHeights(0, 0, heightMapRes, heightMapRes);

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

        float[,] heightMap = GetHeights();
        float[,,] splatMapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float[] splat = new float[terrainData.alphamapLayers];

                for (int i = 0; i < splatHeights.Count; i++)
                {
                    float noise = Mathf.PerlinNoise(x * splatHeights[i].splatNoiseXScale, y * splatHeights[i].splatNoiseYScale) * splatHeights[i].splatNoiseZScale;
                    float offset = splatHeights[i].splatOffset + noise;

                    float thisHeightStart = splatHeights[i].minHeight - offset;
                    float thisHeightStop = splatHeights[i].maxHeight + offset;

                    int hmx = x * ((heightMapRes - 1) / terrainData.alphamapWidth);
                    int hmy = y * ((heightMapRes - 1) / terrainData.alphamapHeight);

                    if (heightMap[hmx, hmy] >= thisHeightStart && heightMap[hmx, hmy] <= thisHeightStop)
                    {
                        if (heightMap[hmx, hmy] <= splatHeights[i].minHeight)
                            splat[i] = 1- Mathf.Abs(heightMap[hmx, hmy] - splatHeights[i].minHeight) / offset;
                        else if (heightMap[hmx, hmy] >= splatHeights[i].maxHeight)
                            splat[i] = 1- Mathf.Abs(heightMap[hmx, hmy] - splatHeights[i].maxHeight) / offset;
                        else  splat[i] = 1;
                    }
                }

                NormalizeVector(ref splat);

                for (int j = 0; j < splatHeights.Count; j++)
                {
                    splatMapData[x, y, j] = splat[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, splatMapData);
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
    void NormalizeVector(ref float[] v)
    {
        float total = 0.0f;

        for (int i = 0; i < v.Length; i++)
        {
            total += v[i];
        }

        for (int i = 0; i < v.Length; i++)
        {
            v[i] /= total;
        }
    }

    protected virtual void DefineAdditionalTextureBehaviour() {}
}