using UnityEngine;

[ExecuteInEditMode]
public class VoronoiTerrain : BaseTerrain
{
    public int voronoiPeakCount = 3;
    public float voronoiFalloff = 0.2f;
    public float voronoiDropoff = 0.6f;
    public float voronoiMinHeight = 0.01f;
    public float voronoiMaxHeight = 0.24f;

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

        Vector3 peak = new Vector3(Random.Range(0, heightMapRes), Random.value * voronoiFalloff, Random.Range(0, heightMapRes));
        heightMap[(int)peak.x, (int)peak.z] = peak.y;
    }
}
