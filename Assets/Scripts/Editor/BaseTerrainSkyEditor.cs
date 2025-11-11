using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTerrainSky), true)]
public class BaseTerrainSkyEditor : Editor
{
    protected SerializedProperty terrainProp;
    protected SerializedProperty terrainDataProp;


    public bool showClouds = false;
    public bool showTerrainData = true;


    protected SerializedProperty numberOfClouds;
    protected SerializedProperty particlesPerClouds;
    protected SerializedProperty cloudParticleSize;
    protected SerializedProperty size;
    protected SerializedProperty cloudMaterial;
    protected SerializedProperty cloudShadowMaterial;
    protected SerializedProperty Colour;
    protected SerializedProperty Lining;
    protected SerializedProperty minSpeed;
    protected SerializedProperty maxSpeed;
    protected SerializedProperty distanceTravelled;

    protected virtual void OnEnable()
    {
        terrainProp = serializedObject.FindProperty("terrain");
        terrainDataProp = serializedObject.FindProperty("terrainData");

        numberOfClouds = serializedObject.FindProperty("numberOfClouds");
        particlesPerClouds = serializedObject.FindProperty("particlesPerClouds");
        cloudParticleSize = serializedObject.FindProperty("cloudParticleSize");
        size = serializedObject.FindProperty("size");
        cloudMaterial = serializedObject.FindProperty("cloudMaterial");
        cloudShadowMaterial = serializedObject.FindProperty("cloudShadowMaterial");
        Colour = serializedObject.FindProperty("Colour");
        Lining = serializedObject.FindProperty("Lining");
        minSpeed = serializedObject.FindProperty("minSpeed");
        maxSpeed = serializedObject.FindProperty("maxSpeed");
        distanceTravelled = serializedObject.FindProperty("distanceTravelled");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        showTerrainData = EditorGUILayout.Foldout(showTerrainData, "Terrain Data");
        if (showTerrainData)
        {
            EditorGUILayout.PropertyField(terrainProp);
            EditorGUILayout.PropertyField(terrainDataProp);
        }

        EditorGUILayout.Space();
        DrawTerrainCloudParameters();
        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }

private void DrawTerrainCloudParameters()
{
    showClouds = EditorGUILayout.Foldout(showClouds, "Clouds Section");
    if (showClouds)
    {
        ICloudGenerate cloudGenerate = (ICloudGenerate)target;
        
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Cloud Settings", EditorStyles.boldLabel);

        EditorGUILayout.IntSlider(numberOfClouds, 1, 200, new GUIContent("Number of Clouds"));
        EditorGUILayout.IntSlider(particlesPerClouds, 1, 1000, new GUIContent("Particles Per Cloud"));
        EditorGUILayout.IntSlider(cloudParticleSize, 1, 50, new GUIContent("Cloud Particle Size"));
        EditorGUILayout.PropertyField(size, new GUIContent("Cloud Size"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(cloudMaterial, new GUIContent("Cloud Material"));
        EditorGUILayout.PropertyField(cloudShadowMaterial, new GUIContent("Shadow Material"));

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(Colour, new GUIContent("Cloud Colour"));
        EditorGUILayout.PropertyField(Lining, new GUIContent("Cloud Lining"));

        EditorGUILayout.Space();
        EditorGUILayout.Slider(minSpeed, 0f, 5f, new GUIContent("Min Speed"));
        EditorGUILayout.Slider(maxSpeed, 0f, 5f, new GUIContent("Max Speed"));
        EditorGUILayout.IntSlider(distanceTravelled, 1, 5000, new GUIContent("Distance Travelled"));

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Clouds")) cloudGenerate.GenerateClouds();
    }
}

}