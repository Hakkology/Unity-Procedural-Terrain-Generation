using UnityEngine;

public class HeightMapTerrain : BaseTerrain 
{
    public Texture2D heightMapImage;
    public Vector3 heightMapScale = new Vector3(1, 1, 1);

    public override void GenerateTerrain()
    {
        LoadTexture();
    }

    /// <summary>
    ///  parameter is for adding or creating the terrain. image resolution and the texture heightmap resolution is proportional.
    /// </summary>
    /// <param name="keepHeights"></param> 
    public void LoadTexture(bool keepHeights = false)
    {
        float[,] heightMap;

        if (!keepHeights)
        {
            heightMap = new float[heightMapRes, heightMapRes];
            for (int x = 0; x < heightMapRes; x++)
            {
                for (int z = 0; z < heightMapRes; z++)
                {
                    heightMap[x, z] = heightMapImage.GetPixel((int)(x * heightMapScale.x), (int)(z * heightMapScale.z)).grayscale * heightMapScale.y;
                }
            }
            terrainData.SetHeights(0, 0, heightMap);
        }
        else
        {
            heightMap = GetHeights();
            for (int x = 0; x < heightMapRes; x++)
            {
                for (int z = 0; z < heightMapRes; z++)
                {
                    heightMap[x, z] += heightMapImage.GetPixel((int)(x * heightMapScale.x), (int)(z * heightMapScale.z)).grayscale * heightMapScale.y;
                }
            }
            terrainData.SetHeights(0, 0, heightMap);
        }
    }
}