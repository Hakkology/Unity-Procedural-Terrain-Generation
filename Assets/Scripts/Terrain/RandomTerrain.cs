using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class RandomTerrain : BaseTerrain
{
    public Vector2 randomHeightRange = new Vector2(0, 0.1f);

    public override void GenerateTerrain()
    {
        GenerateRandomTerrain();
    }

    public void GenerateRandomTerrain()
    {
        float[,] heightMap = GetHeightMap();
        for (int x = 0; x < heightMapRes; x++) // ++x on content, check if anything differs
        {
            for (var z = 0; z < heightMapRes; z++) // ++z on content, check if anything differs
            {
                heightMap[x, z] += Random.Range(randomHeightRange.x, randomHeightRange.y);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
}
