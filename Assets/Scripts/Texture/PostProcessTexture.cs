using UnityEngine;

public class PostProcessTexture : BaseTerrainTexture 
{
    protected override void DefineAdditionalTextureBehaviour()
    {
        if (terrainData == null || splatHeights == null || splatHeights.Count == 0)
        return;

        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        int layers = terrainData.alphamapLayers;

        float[,,] alphaMap = terrainData.GetAlphamaps(0, 0, width, height);
        float[,] heights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float normX = x / (float)(width - 1);
                float normY = y / (float)(height - 1);
                float steepness = terrainData.GetSteepness(normX, normY);

                int hx = x * (heights.GetLength(0) - 1) / width;
                int hy = y * (heights.GetLength(1) - 1) / height;
                float h = heights[hx, hy];

                for (int i = 0; i < splatHeights.Count; i++)
                {
                    SplatHeights sh = splatHeights[i];

                    if (steepness < sh.minSlope || steepness > sh.maxSlope)
                        alphaMap[x, y, i] *= 0.25f;

                    float noise = Mathf.PerlinNoise(x * sh.splatNoiseXScale, y * sh.splatNoiseYScale);
                    alphaMap[x, y, i] *= Mathf.Lerp(0.8f, 1.2f, noise);

                    if (h > sh.maxHeight - 0.05f)
                        alphaMap[x, y, i] = Mathf.Min(1f, alphaMap[x, y, i] * 1.3f);
                }

                float total = 0f;
                for (int i = 0; i < layers; i++) total += alphaMap[x, y, i];
                if (total > 0)
                    for (int i = 0; i < layers; i++) alphaMap[x, y, i] /= total;
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMap);
        Debug.Log("DefineAdditionalTextureBehaviour: slope/height/noise post-processing applied.");
    }
}