using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTerrain), true)]
public abstract class CustomTerrainEditor : Editor
{
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

        if (GUILayout.Button("Reset Terrain"))
        {
            terrain.ResetTerrain();
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    protected abstract void DrawTerrainParameters();
}