using UnityEngine;

[ExecuteInEditMode]
public class VoronoiTerrain : BaseTerrain
{


    public override void GenerateTerrain()
    {
        GenerateVoronoiTerrain();
    }

    public void GenerateVoronoiTerrain()
    {
        float[,] heightMap = GetHeightMap();
        for (int x = 0; x < heightMapRes; x++) // ++x on content, check if anything differs
        {
            for (var z = 0; z < heightMapRes; z++) // ++z on content, check if anything differs
            {
                // heightMap[x, z] += Random.Range(randomHeightRange.x, randomHeightRange.y);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
}
