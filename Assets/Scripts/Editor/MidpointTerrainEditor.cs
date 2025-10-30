using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MidpointTerrain))]
public class MidpointTerrainEditor : BaseTerrainEditor
{
    // SerializedProperty randomHeightRange;
    SerializedProperty MPDHeightMin;
    SerializedProperty MPDHeightMax;
    SerializedProperty MPDHeightDampner;
    SerializedProperty MPDRoughness;
    bool midpoint = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        // randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        MPDHeightMin = serializedObject.FindProperty("MPDHeightMin");
        MPDHeightMax = serializedObject.FindProperty("MPDHeightMax");
        MPDHeightDampner = serializedObject.FindProperty("MPDHeightDampner");
        MPDRoughness = serializedObject.FindProperty("MPDRoughness");
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();

        midpoint = EditorGUILayout.Foldout(midpoint, "Midpoint Parameters");
        if (midpoint)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Midpoint Displacement", EditorStyles.boldLabel);

            EditorGUILayout.Slider(MPDHeightMin, -10f, 0f, new GUIContent("Height Min"));
            EditorGUILayout.Slider(MPDHeightMax, 0f, 10f, new GUIContent("Height Max"));
            EditorGUILayout.Slider(MPDHeightDampner, 0.1f, 10f, new GUIContent("Height Dampner"));
            EditorGUILayout.Slider(MPDRoughness, 0.1f, 10f, new GUIContent("Roughness"));
        }
    }
}