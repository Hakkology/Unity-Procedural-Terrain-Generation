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

    GUITableState splatMapTable;
    GUITableState vegetationTable;
    protected SerializedProperty maxTrees;
    protected SerializedProperty treeSpacing;


    protected virtual void OnEnable()
    {
        terrainProp = serializedObject.FindProperty("terrain");
        terrainDataProp = serializedObject.FindProperty("terrainData");

        splatMapTable = new GUITableState("splatMapTable");

        maxTrees = serializedObject.FindProperty("maxTrees");
        treeSpacing = serializedObject.FindProperty("treeSpacing");
        vegetationTable = new GUITableState("vegetationTable");
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

        DrawTerrainTextureParameters();
        EditorGUILayout.Space();
        DrawTerrainVegetationParameters();
        EditorGUILayout.Space();
        DrawAdditionalButtons();

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawTerrainTextureParameters()
    {
        ITexturable terrain = (ITexturable)target;
        EditorGUILayout.Space();

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
        EditorGUILayout.Space();

        showVegetation = EditorGUILayout.Foldout(showVegetation, "Vegetation");
        if (showVegetation)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Vegetation Properties", EditorStyles.boldLabel);

            EditorGUILayout.IntSlider(maxTrees, 0, 10000, new GUIContent("Max Trees"));
            EditorGUILayout.Slider(treeSpacing, 0f, 50f, new GUIContent("Tree Spacing"));
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
    
    protected virtual void DrawAdditionalButtons() { }

}