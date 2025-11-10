using System;
using EditorGUITable;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTerrainDetail), true)]
public class BaseTerrainDetailEditor : Editor
{
    protected SerializedProperty terrainProp;
    protected SerializedProperty terrainDataProp;

    public bool showTerrainData = true;
    public bool showSplatMaps = false;
    public bool showVegetation = false;
    public bool showDetails = false;
    public bool showWater = false;
    public bool showErosion = false;

    GUITableState splatMapTable;
    GUITableState vegetationTable;
    GUITableState detailsTable;
    
    protected SerializedProperty maxTrees;
    protected SerializedProperty treeSpacing;
    protected SerializedProperty maxDetails;
    protected SerializedProperty detailSpacing;

    protected SerializedProperty water;
    protected SerializedProperty waterHeight;

    protected SerializedProperty erosionType;
    protected SerializedProperty erosionStrength;
    protected SerializedProperty droplets;
    protected SerializedProperty solubility;
    protected SerializedProperty springsPerRiver;
    protected SerializedProperty erosionSmoothAmount;

    protected virtual void OnEnable()
    {
        terrainProp = serializedObject.FindProperty("terrain");
        terrainDataProp = serializedObject.FindProperty("terrainData");

        splatMapTable = new GUITableState("splatMapTable");

        maxTrees = serializedObject.FindProperty("maxTrees");
        treeSpacing = serializedObject.FindProperty("treeSpacing");
        vegetationTable = new GUITableState("vegetationTable");

        maxDetails = serializedObject.FindProperty("maxDetails");
        detailSpacing = serializedObject.FindProperty("detailSpacing");
        detailsTable = new GUITableState("detailsTable");

        water = serializedObject.FindProperty("WaterGo");
        waterHeight = serializedObject.FindProperty("waterHeight");

        erosionType = serializedObject.FindProperty("erosionType");
        erosionStrength = serializedObject.FindProperty("erosionStrength");
        droplets = serializedObject.FindProperty("droplets");
        solubility = serializedObject.FindProperty("solubility");
        springsPerRiver = serializedObject.FindProperty("springsPerRiver");
        erosionSmoothAmount = serializedObject.FindProperty("erosionSmoothAmount");
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
        DrawTerrainTextureParameters();
        EditorGUILayout.Space();
        DrawTerrainVegetationParameters();
        EditorGUILayout.Space();
        DrawTerrainDetailingParameters();
        EditorGUILayout.Space();
        DrawWaterDetailParameters();
        EditorGUILayout.Space();
        DrawErosionDetailParameters();
        EditorGUILayout.Space();
        DrawAdditionalButtons();

        serializedObject.ApplyModifiedProperties();
    }


    protected virtual void DrawTerrainTextureParameters()
    {
        ITexturable terrain = (ITexturable)target;
        showSplatMaps = EditorGUILayout.Foldout(showSplatMaps, "Splat Maps");
        if (showSplatMaps)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Splat Map Properties", EditorStyles.boldLabel);
            splatMapTable = GUITableLayout.DrawTable(splatMapTable, serializedObject.FindProperty("splatHeights"));

            GUILayout.Space(20);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+")) terrain.AddNewSplatHeight();
            if (GUILayout.Button("-")) terrain.RemoveSplatHeights();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Apply Splatmaps")) terrain.SplatMaps();
            EditorGUILayout.Space();
            if (GUILayout.Button("Reset Splatmaps")) terrain.ResetAllTerrainLayers();
        }
    }

    public void DrawTerrainVegetationParameters()
    {
        IVegetative terrain = (IVegetative)target;
        showVegetation = EditorGUILayout.Foldout(showVegetation, "Vegetation");
        if (showVegetation)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Vegetation Properties", EditorStyles.boldLabel);

            EditorGUILayout.IntSlider(maxTrees, 0, 10000, new GUIContent("Max Trees"));
            EditorGUILayout.IntSlider(treeSpacing, 1, 50, new GUIContent("Tree Spacing"));
            EditorGUILayout.Space();

            vegetationTable = GUITableLayout.DrawTable(vegetationTable, serializedObject.FindProperty("vegetationProperties"));

            GUILayout.Space(20);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+")) terrain.AddVegetation();
            if (GUILayout.Button("-")) terrain.RemoveVegetation();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Apply Vegetation")) terrain.ApplyVegetation();
        }
    }

    public void DrawTerrainDetailingParameters()
    {
        ITerrainDetail terrainDetail = (ITerrainDetail)target;
        showDetails = EditorGUILayout.Foldout(showDetails, "Detailing");
        if (showDetails)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Detailing Features", EditorStyles.boldLabel);

            EditorGUILayout.IntSlider(maxDetails, 0, 10000, new GUIContent("Max Details"));
            EditorGUILayout.IntSlider(detailSpacing, 1, 50, new GUIContent("Spacing"));
            EditorGUILayout.Space();

            detailsTable = GUITableLayout.DrawTable(detailsTable, serializedObject.FindProperty("detailProperties"));
            ((BaseTerrainDetail)target).terrain.detailObjectDistance = maxDetails.intValue;

            GUILayout.Space(20);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+")) terrainDetail.AddDetail();
            if (GUILayout.Button("-")) terrainDetail.RemoveDetail();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            if (GUILayout.Button("Apply Details")) terrainDetail.ApplyDetails();
        }
    }

    private void DrawWaterDetailParameters()
    {
        IWaterDetail waterDetail = (IWaterDetail)target;
        showWater = EditorGUILayout.Foldout(showWater, "Water Details");
        if (showWater)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Water Detailing Features", EditorStyles.boldLabel);

            EditorGUILayout.Slider(waterHeight, 0, 1, new GUIContent("Water Height"));
            EditorGUILayout.PropertyField(water);

            EditorGUILayout.Space();
            if (GUILayout.Button("Add Water")) waterDetail.AddWater();
        }
    }
    
    private void DrawErosionDetailParameters()
    {
        IErodeDetail erodeDetail = (IErodeDetail)target;
        showErosion = EditorGUILayout.Foldout(showErosion, "Erosion Details");
        if (showErosion)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Erosion Detailing Features", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(erosionType, new GUIContent("Erosion Type"));
            EditorGUILayout.Slider(erosionStrength, 0f, 1f, new GUIContent("Erosion Strength"));
            EditorGUILayout.IntSlider(droplets, 1, 500, new GUIContent("Droplets"));
            EditorGUILayout.Slider(solubility, 0.001f, 1f, new GUIContent("Solubility"));
            EditorGUILayout.IntSlider(springsPerRiver, 1, 20, new GUIContent("Springs Per River"));
            EditorGUILayout.IntSlider(erosionSmoothAmount, 0, 10, new GUIContent("Erosion Smooth Amount"));

            EditorGUILayout.Space();
            if (GUILayout.Button("Add Erode")) erodeDetail.AddErode();
        }
    }
    
    protected virtual void DrawAdditionalButtons() { }

}