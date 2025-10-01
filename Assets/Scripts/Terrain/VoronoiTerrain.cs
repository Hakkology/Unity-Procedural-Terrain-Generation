using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VoronoiTerrain : BaseTerrain
{
    public int voronoiPeakCount = 3;
    public float voronoiFalloff = 0.2f;
    public float voronoiDropoff = 0.6f;
    public float voronoiMinHeight = 0.01f;
    public float voronoiMaxHeight = 0.24f;
    public float voronoiRoughness = 0.05f;
    public float voronoiSharpness = 6;
    public float voronoiPlateau = 0.02f;
    public PerlinParameters perlinParameters = new();


    public override void GenerateTerrain()
    {
        GenerateVoronoiTerrain();
    }

    public void GenerateVoronoiTerrain()
    {
        float[,] heightMap = GetHeightMap();

        Vector3 peak = new Vector3(Random.Range(0, heightMapRes), Random.value * voronoiFalloff, Random.Range(0, heightMapRes));
        heightMap[(int)peak.x, (int)peak.z] = peak.y;

        Vector2 peakLocation = new Vector2(peak.x, peak.z);
        float maxDistance = Vector2.Distance(new Vector2(0, 0), new Vector2(heightMapRes, heightMapRes));

        for (int y = 0; y < heightMapRes; y++)
        {
            for (int x = 0; x < heightMapRes; x++)
            {
                if (!(x == peak.x && y == peak.z))
                {
                    float distanceToPeak = Vector2.Distance(peakLocation, new Vector2(x, y)) / maxDistance;
                    //float h = peak.y - Mathf.Pow(distanceToPeak, voronoiPower);
                    // float h = peak.y - distanceToPeak * voronoiFalloff - Mathf.Pow(distanceToPeak, voronoiDropoff);
                    float h = peak.y - Mathf.Sin(distanceToPeak * 100) * .5f;
                    heightMap[x, y] = h;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    public void GenerateVoronoiPeakTerrain()
    {
        float[,] heightMap = GetHeightMap();

        for (int p = 0; p < voronoiPeakCount; p++)
        {
            Vector3 peak = new Vector3(
                Random.Range(0, heightMapRes),
                Random.Range(voronoiMinHeight, voronoiMaxHeight),
                Random.Range(0, heightMapRes));

            if (heightMap[(int)peak.x, (int)peak.z] < peak.y)
                heightMap[(int)peak.x, (int)peak.z] = peak.y;
            else
                continue;

            Vector2 peakLocation = new Vector2(peak.x, peak.z);
            float maxDistance = Vector2.Distance(new Vector2(0, 0), new Vector2(heightMapRes, heightMapRes));

            for (int y = 0; y < heightMapRes; y++)
            {
                for (int x = 0; x < heightMapRes; x++)
                {
                    if (!(x == peak.x && y == peak.z))
                    {
                        float distanceToPeak = Vector2.Distance(peakLocation, new Vector2(x, y)) / maxDistance;
                        float h = peak.y - distanceToPeak * voronoiFalloff - Mathf.Pow(distanceToPeak, voronoiDropoff);

                        if (heightMap[x, y] < h)
                            heightMap[x, y] = h;
                    }
                }
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
        // Debug.Log(string.Format("{0} {1} {2} {3} {4}", voronoiPeakCount, voronoiMinHeight, voronoiMaxHeight, voronoiDropoff, voronoiFalloff));
    }

    public void GenerateVoronoiRealisticTerrain()
    {
        float[,] heightMap = GetHeightMap();

        List<Vector3> peaks = new List<Vector3>();
        for (int p = 0; p < voronoiPeakCount; p++)
        {
            Vector3 peak = new Vector3(
                Random.Range(0, heightMapRes),
                Random.Range(voronoiMinHeight, voronoiMaxHeight),
                Random.Range(0, heightMapRes));
            peaks.Add(peak);
        }

        float maxDistance = heightMapRes;

        for (int y = 0; y < heightMapRes; y++)
        {
            for (int x = 0; x < heightMapRes; x++)
            {
                float bestHeight = 0f;

                foreach (var peak in peaks)
                {
                    float distance = Vector2.Distance(
                        new Vector2(peak.x, peak.z),
                        new Vector2(x, y)) / maxDistance;

                    float h = peak.y * Mathf.Exp(-Mathf.Pow(distance * voronoiSharpness, 2));

                    if (distance < voronoiPlateau)
                        h = peak.y;

                    h += Mathf.PerlinNoise(x * 0.05f, y * 0.05f) * voronoiRoughness * (1f - distance);

                    if (h > bestHeight)
                        bestHeight = h;
                }

                heightMap[x, y] = Mathf.Clamp01(bestHeight);
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    public void GenerateVoronoiPerlinTerrain()
    {
        float[,] heightMap = GetHeightMap();


        for (int p = 0; p < voronoiPeakCount; p++)
        {
            Vector3 peak = new Vector3(
                Random.Range(0, heightMapRes),
                Random.Range(voronoiMinHeight, voronoiMaxHeight),
                Random.Range(0, heightMapRes));

            if (heightMap[(int)peak.x, (int)peak.z] < peak.y)
                heightMap[(int)peak.x, (int)peak.z] = peak.y;
            else
                continue;

            Vector2 peakLocation = new Vector2(peak.x, peak.z);
            float maxDistance = Vector2.Distance(new Vector2(0.0f, 0.0f), new Vector2(heightMapRes, heightMapRes));

            for (int y = 0; y < heightMapRes; y++)
            {
                for (int x = 0; x < heightMapRes; x++)
                {
                    if (!(x == peak.x && y == peak.z))
                    {
                        float distanceToPeak = Vector2.Distance(peakLocation, new Vector2(x, y)) / maxDistance;
                        float h = peak.y - distanceToPeak * voronoiFalloff - Mathf.Pow(distanceToPeak, voronoiDropoff);

                        h = peak.y - distanceToPeak * voronoiFalloff +
                        Utils.FractalBrownianMotion((x + perlinParameters.mPerlinXOffset) * perlinParameters.mPerlinXScale,
                                                    (y + perlinParameters.mPerlinYOffset) * perlinParameters.mPerlinYScale,
                                                    perlinParameters.mPerlinOctaves,
                                                    perlinParameters.mPerlinPersistance) * perlinParameters.mPerlinHeightScale; ;

                        if (heightMap[x, y] < h)
                        {
                            heightMap[x, y] = h;
                        }
                    }


                }
            }
        }
        
        terrainData.SetHeights(0, 0, heightMap);
    }
}
