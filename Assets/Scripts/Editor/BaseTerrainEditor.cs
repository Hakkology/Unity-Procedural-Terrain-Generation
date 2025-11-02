using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTerrain), true)]
public class BaseTerrainEditor : Editor
{
    protected SerializedProperty terrainProp;
    protected SerializedProperty terrainDataProp;
    protected SerializedProperty resetTerrain;
    protected SerializedProperty smoothCount;

    bool showCommonParameters = true;

    protected virtual void OnEnable()
    {
        terrainProp = serializedObject.FindProperty("terrain");
        terrainDataProp = serializedObject.FindProperty("terrainData");
        resetTerrain = serializedObject.FindProperty("resetTerrain");
        smoothCount = serializedObject.FindProperty("smoothCount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //DrawDefaultInspector();
        DrawTerrainParameters();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.Space();

        IGeneratable terrain = (IGeneratable)target;

        if (GUILayout.Button("Generate Terrain"))
        {
            terrain.GenerateTerrain();
        }

        EditorGUILayout.Space();
        DrawAdditionalButtons();

        if (GUILayout.Button("Smooth Terrain"))
        {
            terrain.SmoothTerrain();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Reset Terrain"))
        {
            terrain.ResetTerrain();
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawTerrainParameters()
    {
        EditorGUILayout.PropertyField(terrainProp);
        EditorGUILayout.PropertyField(terrainDataProp);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        showCommonParameters = EditorGUILayout.Foldout(showCommonParameters, "Common Parameters");
        if (showCommonParameters)
        {
            EditorGUILayout.PropertyField(resetTerrain);
            EditorGUILayout.IntSlider(smoothCount, 1, 10, new GUIContent("Smooth Iterations"));
        }

        EditorGUILayout.Space();
    }

    protected virtual void DrawAdditionalButtons() { }
}