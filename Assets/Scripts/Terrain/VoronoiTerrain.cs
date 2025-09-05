using UnityEngine;

[ExecuteInEditMode]
public class VoronoiTerrain : BaseTerrain
{
    public float voronoiFalloff = 0.5f;
    public float voronoiPower = .5;
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
                    float h = peak.y - Mathf.Pow(distanceToPeak, voronoiPower);
                    heightMap[x, y] = h;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }
}
