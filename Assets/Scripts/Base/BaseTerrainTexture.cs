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
            newSplatPrototypes[spIndex].normalMapTexture = sh.textureNormalMap;
            newSplatPrototypes[spIndex].diffuseTexture.Apply(true);
            newSplatPrototypes[spIndex].tileOffset = sh.tileOffset;
            newSplatPrototypes[spIndex].tileSize = sh.tileSize;
            string path = "Assets/New TerrainLayer " + spIndex + ".terrainLayer";
            AssetDatabase.CreateAsset(newSplatPrototypes[spIndex], path);

            spIndex++;
            Selection.activeObject = gameObject;
        }

        terrainData.terrainLayers = newSplatPrototypes;

        float[,] heightMap = GetHeights();
        float[,,] splatMapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float[] splat = new float[terrainData.alphamapLayers];
                bool emptySplat = true;

                for (int i = 0; i < splatHeights.Count; i++)
                {
                    float noise = Mathf.PerlinNoise(x * splatHeights[i].splatNoiseXScale, y * splatHeights[i].splatNoiseYScale) * splatHeights[i].splatNoiseZScale;
                    float offset = splatHeights[i].splatOffset + noise;

                    float thisHeightStart = splatHeights[i].minHeight - offset;
                    float thisHeightStop = splatHeights[i].maxHeight + offset;

                    int hmx = x * ((heightMapRes - 1) / terrainData.alphamapWidth);
                    int hmy = y * ((heightMapRes - 1) / terrainData.alphamapHeight);

                    float normX = x * 1.0f / (terrainData.alphamapWidth - 1);
                    float normY = y * 1.0f / (terrainData.alphamapHeight - 1);

                    var steepness = terrainData.GetSteepness(normX, normY);

                    // where textures should go.
                    if (heightMap[hmx, hmy] >= thisHeightStart && heightMap[hmx, hmy] <= thisHeightStop &&
                        steepness >= splatHeights[i].minSlope && steepness <= splatHeights[i].maxSlope)
                    {
                        if (heightMap[hmx, hmy] <= splatHeights[i].minHeight)
                            splat[i] = 1 - Mathf.Abs(heightMap[hmx, hmy] - splatHeights[i].minHeight) / offset;
                        else if (heightMap[hmx, hmy] >= splatHeights[i].maxHeight)
                            splat[i] = 1 - Mathf.Abs(heightMap[hmx, hmy] - splatHeights[i].maxHeight) / offset;
                        else splat[i] = 1;

                        emptySplat = false;
                    }
                }

                NormalizeVector(ref splat);

                if (emptySplat)
                {
                    splatMapData[x, y, 0] = splat[1];
                }
                else
                {
                    for (int j = 0; j < splatHeights.Count; j++)
                    {
                        splatMapData[x, y, j] = splat[j];
                    }
                }


            }
        }

        DefineAdditionalTextureBehaviour();
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

    public void ResetAllTerrainLayers()
    {
        if (terrainData == null)
        {
            Debug.LogWarning("No TerrainData assigned.");
            return;
        }

        terrainData.terrainLayers = new TerrainLayer[0];

        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        int layers = Mathf.Max(1, terrainData.alphamapLayers); 

        float[,,] emptySplat = new float[width, height, layers];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                for (int l = 0; l < layers; l++)
                    emptySplat[x, y, l] = (l == 0) ? 1f : 0f; // ilk layer tam dolu, diğerleri 0

        terrainData.SetAlphamaps(0, 0, emptySplat);

        Debug.Log("Terrain splatmaps cleared. All layers removed from terrainData.");
    }

    void NormalizeVector(ref float[] v)
    {
        float total = 0.0f;

        for (int i = 0; i < v.Length; i++)
        {
            total += v[i];
        }

        if (total == 0) return;

        for (int i = 0; i < v.Length; i++)
        {
            v[i] /= total;
        }
    }

    protected virtual void DefineAdditionalTextureBehaviour() {}
}