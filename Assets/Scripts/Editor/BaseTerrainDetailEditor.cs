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

    GUITableState splatMapTable;
    GUITableState vegetationTable;
    GUITableState detailsTable;
    
    protected SerializedProperty maxTrees;
    protected SerializedProperty treeSpacing;
    protected SerializedProperty maxDetails;
    protected SerializedProperty detailSpacing;


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
    
    protected virtual void DrawAdditionalButtons() { }

}