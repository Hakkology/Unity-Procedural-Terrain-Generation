using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BaseTerrainSky : MonoBehaviour, ICloudGenerate
{
    protected int heightMapRes => terrainData.heightmapResolution;
    protected float[,] GetHeights() => terrainData.GetHeights(0, 0, heightMapRes, heightMapRes);

    public Terrain terrain;
    public TerrainData terrainData;

    public int numberOfClouds = 1;
    public int particlesPerClouds = 50;
    public int cloudParticleSize = 5;
    public Vector3 cloudStartSize = Vector3.one;
    public Material cloudMaterial;
    public Material cloudShadowMaterial;
    public Color Colour = Color.white;
    public Color Lining = Color.grey;
    public float cloudMinSpeed = 0.2f;
    public float cloudMaxSpeed = 0.5f;
    public int cloudRange = 500;


    void OnEnable()
    {
        Debug.Log("Initializing terrain data");
        terrainData = terrain.terrainData;
    }

    public void GenerateClouds()
    {
        GameObject cloudManager = GameObject.Find("CloudManager");
        if (!cloudManager)
        {
            cloudManager = new GameObject();
            cloudManager.name = "CloudManager";
            cloudManager.AddComponent<CloudManager>();
            cloudManager.transform.position = terrain.transform.position;
        }

        GameObject[] allClouds = GameObject.FindGameObjectsWithTag("Cloud");

        for (int i = 0; i < allClouds.Length; ++i)
            DestroyImmediate(allClouds[i]);


        for (int c = 0; c < numberOfClouds; ++c) {

            GameObject cloudGO = new GameObject();
            cloudGO.name = "Cloud" + c;
            cloudGO.tag = "Cloud";
            cloudGO.layer = LayerMask.NameToLayer("Sky");

            cloudGO.transform.rotation = cloudManager.transform.rotation;
            cloudGO.transform.position = cloudManager.transform.position;
            CloudController cc = cloudGO.AddComponent<CloudController>();
            cc.lining = Lining;
            cc.colour = Colour;
            cc.numberOfParticles = particlesPerClouds;
            cc.minSpeed = cloudMinSpeed;
            cc.maxSpeed = cloudMaxSpeed;
            cc.distance = cloudRange;

            ParticleSystem cloudSystem = cloudGO.AddComponent<ParticleSystem>();
            ParticleSystemRenderer cloudRend = cloudGO.GetComponent<ParticleSystemRenderer>();
            cloudRend.material = cloudMaterial;
            cloudRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            cloudRend.receiveShadows = false;

            GameObject cloudProjector = new GameObject();
            cloudProjector.name = "Shadow";
            cloudProjector.transform.position = cloudGO.transform.position;
            cloudProjector.transform.forward = Vector3.down;
            cloudProjector.transform.parent = cloudGO.transform;

            DecalProjector cp = cloudProjector.AddComponent<DecalProjector>();
            cp.material = cloudShadowMaterial;
            cp.renderingLayerMask = (uint)LayerMask.NameToLayer("Sky");
            cp.size = new Vector3(10, 10, 100);

            ParticleSystem.MainModule main = cloudSystem.main;
            main.loop = false;
            main.startLifetime = Mathf.Infinity;
            main.startSpeed = 0;
            main.startSize = cloudParticleSize;
            main.startColor = Color.white;

            var emission = cloudSystem.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] {
            new ParticleSystem.Burst(0.0f, (short)particlesPerClouds) });

            var shape = cloudSystem.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.scale = new Vector3(cloudStartSize.x, cloudStartSize.y, cloudStartSize.z);

            cloudGO.transform.parent = cloudManager.transform;
            cloudGO.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
