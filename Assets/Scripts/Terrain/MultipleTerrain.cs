using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTerrain : MonoBehaviour
{
    public float perlinOffsetX = 0;
    public float perlinOffsetZ = 0;
    public float perlinXScale = 0.001f;
    public float perlinZScale = 0.001f;
    public float perlinHeightScale = 0.5f;
    public float perlinPersistance = 8;
    public int perlinOctaves = 3;

    void OnValidate()
    {
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            TerrainData terrainData = terrain.terrainData;
            int heightMapRes = terrainData.heightmapResolution;
            float[,] heightMap = new float[heightMapRes, heightMapRes];

            for (int z = 0; z < heightMapRes; z++)
            {
                for (int x = 0; x < heightMapRes; x++)
                {
                    float worldPositionX = ((float)x / (float)heightMapRes) * terrainData.size.x + terrain.transform.position.x;
                    float worldPositionZ = ((float)z / (float)heightMapRes) * terrainData.size.z + terrain.transform.position.z;
                    heightMap[z, x] += Utils.FractalBrownianMotion(
                        (worldPositionX + perlinOffsetX) * perlinXScale,
                        (worldPositionZ + perlinOffsetZ) * perlinZScale, perlinOctaves, perlinPersistance) * perlinHeightScale;
                }
            }

            terrainData.SetHeights(0, 0, heightMap);
        }
    }
}
