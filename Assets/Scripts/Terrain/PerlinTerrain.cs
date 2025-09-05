using System.Collections.Generic;
using UnityEngine;

public class PerlinTerrain : BaseTerrain
{
    public float perlinXScale = 0.01f;
    public float perlinYScale = 0.01f;

    public int perlinXOffset = 0;
    public int perlinYOffset = 0;

    public int perlinOctaves = 3;
    public float perlinPersistance = 8;
    public float perlinHeightScale = 0.09f;

    public float perlinRidgeConstant = 0.5f;


    public List<PerlinParameters> perlinParameters = new List<PerlinParameters>()
    {
        new PerlinParameters()
    };

    public override void GenerateTerrain()
    {
        GeneratePerlinTerrain();
    }

    public void GeneratePerlinTerrain(bool fractal = false)
    {
        float[,] heightMap = GetHeightMap();

        for (int y = 0; y < heightMapRes; y++)
        {
            for (int x = 0; x < heightMapRes; x++)
            {
                if (fractal)
                {
                    heightMap[x, y] += Utils.FractalBrownianMotion((x + perlinXOffset) * perlinXScale, (y + perlinYOffset) * perlinYScale, perlinOctaves, perlinPersistance) * perlinHeightScale;
                }
                else
                {
                    heightMap[x, y] += Mathf.PerlinNoise((x + perlinXOffset) * perlinXScale, (y + perlinYOffset) * perlinYScale);
                }

            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    public void GenerateMultiplePerlinTerrain()
    {
        float[,] heightMap = GetHeightMap();

        for (int y = 0; y < heightMapRes; y++)
        {
            for (int x = 0; x < heightMapRes; x++)
            {
                foreach (PerlinParameters p in perlinParameters)
                {
                    heightMap[x, y] += Utils.FractalBrownianMotion((x + p.mPerlinXOffset) * p.mPerlinXScale, (y + p.mPerlinYOffset) * p.mPerlinYScale, p.mPerlinOctaves, p.mPerlinPersistance) * p.mPerlinHeightScale;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    public void GenerateRidgeNoiseTerrain()
    {
        ResetTerrain();
        GenerateMultiplePerlinTerrain();
        float[,] heightMap = GetHeightMap();

        for (int y = 0; y < heightMapRes; y++)
        {
            for (int x = 0; x < heightMapRes; x++)
            {
                heightMap[x, y] = 1f - Mathf.Abs(heightMap[x, y] - perlinRidgeConstant);
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    public void AddNewPerlin()
    {
        perlinParameters.Add(new PerlinParameters());
    }

    public void RemovePerlin()
    {
        List<PerlinParameters> keptPerlinParameters = new List<PerlinParameters>();
        for (int i = 0; i < perlinParameters.Count; i++)
        {
            if (!perlinParameters[i].remove)
            {
                keptPerlinParameters.Add(perlinParameters[i]);
            }
        }
        
        if (keptPerlinParameters.Count == 0)
        {
            keptPerlinParameters.Add(perlinParameters[0]);
        }
        perlinParameters = keptPerlinParameters;
    }
}