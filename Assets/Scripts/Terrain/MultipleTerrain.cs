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
        float perlinZeroOffset = (Terrain.activeTerrains.Length + 1) * Terrain.activeTerrains[0].terrainData.size.x;

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
                        (worldPositionX + perlinOffsetX + perlinZeroOffset) * perlinXScale,
                        (worldPositionZ + perlinOffsetZ) * perlinZScale, perlinOctaves, perlinPersistance) * perlinHeightScale;
                }
            }

            terrainData.SetHeights(0, 0, heightMap);
        }

        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            TerrainData terrainData = terrain.terrainData;
            int heightMapRes = terrainData.heightmapResolution;
            float[,] thisHeightMap = terrainData.GetHeights(0, 0, heightMapRes, heightMapRes);

            if (terrain.topNeighbor != null)
            {

                float[,] topNeighbourHeightMap = terrain.topNeighbor.terrainData.GetHeights(0, 0, heightMapRes, heightMapRes);

                for (int z = 0; z < heightMapRes; ++z)
                {

                    topNeighbourHeightMap[0, z] = thisHeightMap[heightMapRes - 1, z];
                }
                terrain.topNeighbor.terrainData.SetHeights(0, 0, topNeighbourHeightMap);
            }

            if (terrain.rightNeighbor != null)
            {

                float[,] rightNeighbourHeightMap = terrain.rightNeighbor.terrainData.GetHeights(0, 0, heightMapRes, heightMapRes);

                for (int x = 0; x < heightMapRes; ++x)
                {

                    rightNeighbourHeightMap[x, 0] = thisHeightMap[x, heightMapRes - 1];
                }
                terrain.rightNeighbor.terrainData.SetHeights(0, 0, rightNeighbourHeightMap);
            }
        }
        
        // due to perlin noise, 0,0 is still mirrored and flawed on intersections.
    }
}
