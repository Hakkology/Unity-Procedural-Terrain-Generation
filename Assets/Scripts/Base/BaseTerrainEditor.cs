using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTerrain), true)]
public class BaseTerrainEditor : Editor
{
    protected SerializedProperty terrainProp;
    protected SerializedProperty terrainDataProp;

    protected virtual void OnEnable()
    {
        terrainProp = serializedObject.FindProperty("terrain");
        terrainDataProp = serializedObject.FindProperty("terrainData");
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
    }

    protected virtual void DrawAdditionalButtons() { }
}