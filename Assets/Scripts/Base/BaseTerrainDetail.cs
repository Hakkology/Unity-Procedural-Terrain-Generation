using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseTerrainDetail : MonoBehaviour, ITexturable, IVegetative, ITerrainDetail
{
    protected int heightMapRes => terrainData.heightmapResolution;
    protected float[,] GetHeights() => terrainData.GetHeights(0, 0, heightMapRes, heightMapRes);

    public Terrain terrain;
    public TerrainData terrainData;
    
    public int maxTrees;
    public int treeSpacing;

    public int maxDetails;
    public int detailSpacing;

    public List<DetailProperties> detailProperties = new List<DetailProperties>()
    {
        new DetailProperties()
    };

    public List<SplatHeights> splatHeights = new List<SplatHeights>()
    {
        new SplatHeights()
    };

    public List<VegetationProperties> vegetationProperties = new List<VegetationProperties>()
    {
        new VegetationProperties() 
    };

    void OnEnable()
    {
        Debug.Log("Initializing terrain data");
        terrainData = terrain.terrainData;
    }

    public void SplatMaps()
    {
        TerrainLayer[] newSplatPrototypes;
        newSplatPrototypes = new TerrainLayer[splatHeights.Count];

        int spIndex = 0;

        foreach (SplatHeights sh in splatHeights)
        {
            newSplatPrototypes[spIndex] = new TerrainLayer();
            newSplatPrototypes[spIndex].diffuseTexture = sh.texture;
            newSplatPrototypes[spIndex].normalMapTexture = sh.textureNormalMap;
            newSplatPrototypes[spIndex].diffuseTexture.Apply(true);
            newSplatPrototypes[spIndex].tileOffset = sh.tileOffset;
            newSplatPrototypes[spIndex].tileSize = sh.tileSize;
            string path = "Assets/New TerrainLayer " + spIndex + ".terrainLayer";
            AssetDatabase.CreateAsset(newSplatPrototypes[spIndex], path);

            spIndex++;
            Selection.activeObject = gameObject;
        }

        terrainData.terrainLayers = newSplatPrototypes;

        float[,] heightMap = GetHeights();
        float[,,] splatMapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                float[] splat = new float[terrainData.alphamapLayers];
                bool emptySplat = true;

                for (int i = 0; i < splatHeights.Count; i++)
                {
                    float noise = Mathf.PerlinNoise(x * splatHeights[i].splatNoiseXScale, y * splatHeights[i].splatNoiseYScale) * splatHeights[i].splatNoiseZScale;
                    float offset = splatHeights[i].splatOffset + noise;

                    float thisHeightStart = splatHeights[i].minHeight - offset;
                    float thisHeightStop = splatHeights[i].maxHeight + offset;

                    int hmx = x * ((heightMapRes - 1) / terrainData.alphamapWidth);
                    int hmy = y * ((heightMapRes - 1) / terrainData.alphamapHeight);

                    float normX = x * 1.0f / (terrainData.alphamapWidth - 1);
                    float normY = y * 1.0f / (terrainData.alphamapHeight - 1);

                    var steepness = terrainData.GetSteepness(normX, normY);

                    // where textures should go.
                    if (heightMap[hmx, hmy] >= thisHeightStart && heightMap[hmx, hmy] <= thisHeightStop &&
                        steepness >= splatHeights[i].minSlope && steepness <= splatHeights[i].maxSlope)
                    {
                        if (heightMap[hmx, hmy] <= splatHeights[i].minHeight)
                            splat[i] = 1 - Mathf.Abs(heightMap[hmx, hmy] - splatHeights[i].minHeight) / offset;
                        else if (heightMap[hmx, hmy] >= splatHeights[i].maxHeight)
                            splat[i] = 1 - Mathf.Abs(heightMap[hmx, hmy] - splatHeights[i].maxHeight) / offset;
                        else splat[i] = 1;

                        emptySplat = false;
                    }
                }

                NormalizeVector(ref splat);

                if (emptySplat)
                {
                    splatMapData[x, y, 0] = splat[1];
                }
                else
                {
                    for (int j = 0; j < splatHeights.Count; j++)
                    {
                        splatMapData[x, y, j] = splat[j];
                    }
                }


            }
        }

        DefineAdditionalTextureBehaviour();
        terrainData.SetAlphamaps(0, 0, splatMapData);
    }

    public void AddNewSplatHeight()
    {
        splatHeights.Add(new SplatHeights());
    }

    public void RemoveSplatHeights()
    {
        List<SplatHeights> keepSplatHeights = new List<SplatHeights>();

        for (int i = 0; i < splatHeights.Count; i++)
        {
            if (!splatHeights[i].remove)
            {
                keepSplatHeights.Add(splatHeights[i]);
            }
        }

        if (keepSplatHeights.Count == 0)
        {
            keepSplatHeights.Add(splatHeights[0]);
        }

        splatHeights = keepSplatHeights;
    }

    public void ResetAllTerrainLayers()
    {
        if (terrainData == null)
        {
            Debug.LogWarning("No TerrainData assigned.");
            return;
        }

        terrainData.terrainLayers = new TerrainLayer[0];

        int width = terrainData.alphamapWidth;
        int height = terrainData.alphamapHeight;
        int layers = Mathf.Max(1, terrainData.alphamapLayers);

        float[,,] emptySplat = new float[width, height, layers];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                for (int l = 0; l < layers; l++)
                    emptySplat[x, y, l] = (l == 0) ? 1f : 0f; // ilk layer tam dolu, diğerleri 0

        terrainData.SetAlphamaps(0, 0, emptySplat);

        Debug.Log("Terrain splatmaps cleared. All layers removed from terrainData.");
    }
    
    public void AddVegetation()
    {
        vegetationProperties.Add(new VegetationProperties());
    }

    public void RemoveVegetation()
    {
        List<VegetationProperties> keepVegetationProps = new List<VegetationProperties>();

        for (int i = 0; i < vegetationProperties.Count; i++)
        {
            if (!vegetationProperties[i].remove)
            {
                keepVegetationProps.Add(vegetationProperties[i]);
            }
        }

        if (keepVegetationProps.Count == 0)
        {
            keepVegetationProps.Add(vegetationProperties[0]);
        }

        vegetationProperties = keepVegetationProps;
    }

    public void ApplyVegetation()
    {
        TreePrototype[] newTreePrototypes;
        newTreePrototypes = new TreePrototype[vegetationProperties.Count];
        int tIndex = 0;

        foreach (VegetationProperties t in vegetationProperties)
        {
            newTreePrototypes[tIndex] = new TreePrototype();
            newTreePrototypes[tIndex].prefab = t.prefab; // it wants mesh but defined as prefab ? Confusing.
            tIndex++;
        }
        terrainData.treePrototypes = newTreePrototypes;

        List<TreeInstance> allVegetation = new List<TreeInstance>();
        int layerMask = 1 << LayerMask.NameToLayer("Terrain");

        for (int z = 0; z < terrainData.alphamapHeight; z += treeSpacing)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x += treeSpacing)
            {
                for (int tp = 0; tp < terrainData.treePrototypes.Length; ++tp)
                {
                    if (Random.value > vegetationProperties[tp].density) break;

                    float thisScale = Random.Range(vegetationProperties[tp].minScale, vegetationProperties[tp].maxScale);
                    float thisHeight = terrainData.GetHeight(x, z) / terrainData.size.y;
                    float thisHeightStart = vegetationProperties[tp].minHeight;
                    float thisHeightEnd = vegetationProperties[tp].maxHeight;

                    float normX = x * 1.0f / (terrainData.alphamapWidth - 1);
                    float normY = z * 1.0f / (terrainData.alphamapHeight - 1);
                    float steepness = terrainData.GetSteepness(normX, normY);

                    if ((thisHeight >= thisHeightStart && thisHeight <= thisHeightEnd) &&
                        (steepness >= vegetationProperties[tp].minSlope && steepness <= vegetationProperties[tp].maxSlope))
                    {
                        TreeInstance instance = new TreeInstance();

                        instance.position = new Vector3(
                            (x + Random.Range(-5f, 5f)) / terrainData.alphamapWidth,
                            thisHeight,
                            (z + Random.Range(-5f, 5f)) / terrainData.alphamapHeight);

                        Vector3 treeWorldPos = new Vector3(instance.position.x * terrainData.size.x,
                                                            instance.position.y * terrainData.size.y,
                                                            instance.position.z * terrainData.size.z) + this.transform.position;
                        RaycastHit hit;
                        if (Physics.Raycast(treeWorldPos + new Vector3(0, 10, 0), -Vector3.up, out hit, 100, layerMask) ||
                            Physics.Raycast(treeWorldPos + new Vector3(0, 10, 0), Vector3.up, out hit, 100, layerMask))
                        {
                            float treeHeight = (hit.point.y - terrain.gameObject.transform.position.y) / terrainData.size.y;
                            instance.position = new Vector3(instance.position.x, treeHeight, instance.position.z);
                        }

                        instance.rotation = Random.Range(vegetationProperties[tp].minRotation, vegetationProperties[tp].maxRotation);
                        instance.widthScale = Mathf.Max(0.01f, thisScale);
                        instance.heightScale = Mathf.Max(0.01f, thisScale);
                        instance.prototypeIndex = tp;
                        instance.color = Color.Lerp(vegetationProperties[tp].colour1, vegetationProperties[tp].colour2, Random.value);
                        instance.lightmapColor = vegetationProperties[tp].lightColour;

                        allVegetation.Add(instance);
                        if (allVegetation.Count >= maxTrees) goto TREESDONE;
                    }
                }
            }
        }

    TREESDONE:
        terrainData.treeInstances = allVegetation.ToArray();
    }
    
    public void ApplyDetails()
    {
        DetailPrototype[] newDetailPrototypes;
        newDetailPrototypes = new DetailPrototype[detailProperties.Count];
        int dIndex = 0;

        foreach (DetailProperties d in detailProperties)
        {
            newDetailPrototypes[dIndex] = new DetailPrototype();
            newDetailPrototypes[dIndex].prototype = d.prototype;
            newDetailPrototypes[dIndex].prototypeTexture = d.protoTypeTexture;
            newDetailPrototypes[dIndex].healthyColor = Color.white;

            if (newDetailPrototypes[dIndex].prototype)
            {
                newDetailPrototypes[dIndex].usePrototypeMesh = true;
                newDetailPrototypes[dIndex].renderMode = DetailRenderMode.Grass;
            }
            else
            {
                newDetailPrototypes[dIndex].usePrototypeMesh = false;
                newDetailPrototypes[dIndex].renderMode = DetailRenderMode.GrassBillboard;
            }

            dIndex++;
        }

        terrainData.detailPrototypes = newDetailPrototypes;
    }

    public void AddDetail()
    {
        detailProperties.Add(new DetailProperties());
    }

    public void RemoveDetail()
    {
        List<DetailProperties> keepDetailProps = new List<DetailProperties>();

        for (int i = 0; i < detailProperties.Count; i++)
        {
            if (!detailProperties[i].remove)
            {
                keepDetailProps.Add(detailProperties[i]);
            }
        }

        if (keepDetailProps.Count == 0)
        {
            keepDetailProps.Add(detailProperties[0]);
        }

        detailProperties = keepDetailProps;
    }


    void NormalizeVector(ref float[] v)
    {
        float total = 0.0f;

        for (int i = 0; i < v.Length; i++)
        {
            total += v[i];
        }

        if (total == 0) return;

        for (int i = 0; i < v.Length; i++)
        {
            v[i] /= total;
        }
    }

    protected virtual void DefineAdditionalTextureBehaviour() {}
}