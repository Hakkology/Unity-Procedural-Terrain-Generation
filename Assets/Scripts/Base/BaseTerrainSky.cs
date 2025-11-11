using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseTerrainSky : MonoBehaviour, ICloudGenerate
{
    protected int heightMapRes => terrainData.heightmapResolution;
    protected float[,] GetHeights() => terrainData.GetHeights(0, 0, heightMapRes, heightMapRes);

    public Terrain terrain;
    public TerrainData terrainData;

    public int numberOfClouds = 1;
    public int particlesPerClouds = 50;
    public int cloudParticleSize = 5;
    public Vector3 size = Vector3.one;
    public Material cloudMaterial;
    public Material cloudShadowMaterial;
    public Color Colour;
    public Color Lining;
    public float minSpeed = 0.2f;
    public float maxSpeed = 0.5f;
    public int distanceTravelled = 500;


    void OnEnable()
    {
        Debug.Log("Initializing terrain data");
        //terrain = GetComponent<Terrain>();
        terrainData = terrain.terrainData;
    }

    List<Vector2> GenerateNeighbours(Vector2 pos, int width, int height) {

        List<Vector2> neighbours = new List<Vector2>();

        for (int y = -1; y < 2; ++y) { // between -1 and 1.

            for (int x = -1; x < 2; ++x) {

                if (!(x == 0 && y == 0)) {

                    Vector2 nPos = new Vector2(
                        Mathf.Clamp(pos.x + x, 0.0f, width - 1),
                        Mathf.Clamp(pos.y + y, 0.0f, height - 1));

                    if (!neighbours.Contains(nPos))
                        neighbours.Add(nPos);
                }
            }
        }
        return neighbours;
    }

    public void GenerateClouds()
    {
        
    }
}
