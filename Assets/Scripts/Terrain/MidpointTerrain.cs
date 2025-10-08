using UnityEngine;

public class MidpointTerrain : BaseTerrain
{

    public override void GenerateTerrain()
    {
        GenerateMidpointDisplacementTerrain();

    }

    public void GenerateMidpointDisplacementTerrain()
    {
        float[,] heightMap = GetHeightMap();
        int width = heightMapRes - 1;
        int squareSize = width;

        int cornerX, cornerY;
        int midX, midY;
        int pmiXL, pmidXR, pmidYU, pmidYD;

        heightMap[0, 0] = Random.Range(0, .2f);
        heightMap[0, heightMapRes - 1] = Random.Range(0, .2f);
        heightMap[heightMapRes - 1, 0] = Random.Range(0, .2f);
        heightMap[heightMapRes - 1, heightMapRes - 1] = Random.Range(0, .2f);

        for (int x = 0; x < width; x += squareSize)
        {
            for (int y = 0; y < width; y += squareSize)
            {
                cornerX = (x + squareSize);
                cornerY = (y + squareSize);

                midX = (int)(x + squareSize / 2.0f);
                midY = (int)(y + squareSize / 2.0f);

                heightMap[midX, midY] = (float)((heightMap[x, y] + heightMap[cornerX, y] + heightMap[x, cornerY] + heightMap[cornerX, cornerY]) / 4.0f);
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }
}