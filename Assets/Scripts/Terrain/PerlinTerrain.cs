using UnityEngine;

public class PerlinTerrain : BaseTerrain
{
    public float perlinXScale = 0.01f;
    public float perlinYScale = 0.01f;

    public int perlinXOffset = 0;
    public int perlinYOffset = 0;

    public override void GenerateTerrain()
    {
        GeneratePerlinTerrain();
    }

    public void GeneratePerlinTerrain(bool add = false)
    {
        float[,] heightMap = GetHeights();

        for (int y = 0; y < heightMapRes; y++)
        {
            for (int x = 0; x < heightMapRes; x++)
            {
                if (add)
                    heightMap[x, y] += Mathf.PerlinNoise((x + perlinXOffset) * perlinXScale, (y + perlinYOffset) * perlinYScale);
                else
                    heightMap[x, y] = Mathf.PerlinNoise((x + perlinXOffset) * perlinXScale, (y + perlinYOffset) * perlinYScale);
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }
}