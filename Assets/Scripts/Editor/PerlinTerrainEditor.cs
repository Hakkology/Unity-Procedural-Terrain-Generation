using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerlinTerrain))]
public class PerlinTerrainEditor : BaseTerrainEditor
{
    SerializedProperty perlinXScale;
    SerializedProperty perlinYScale;
    SerializedProperty perlinXOffset;
    SerializedProperty perlinYOffset;
    bool showLoadHeights = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        perlinXScale = serializedObject.FindProperty("perlinXScale");
        perlinYScale = serializedObject.FindProperty("perlinYScale");
        perlinXOffset = serializedObject.FindProperty("perlinXOffset");
        perlinYOffset = serializedObject.FindProperty("perlinYOffset");
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();

        showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Heightmap Settings");
        if (showLoadHeights)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Perlin Noise", EditorStyles.boldLabel);

            EditorGUILayout.Slider(perlinXScale, 0, 1, new GUIContent("X Scale"));
            EditorGUILayout.Slider(perlinYScale, 0, 1, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(perlinXOffset, 0, 10000, new GUIContent("X Offset"));
            EditorGUILayout.IntSlider(perlinYOffset, 0, 10000, new GUIContent("Y Offset"));
        }
    }
    
    protected override void DrawAdditionalButtons()
    {
        PerlinTerrain terrain = (PerlinTerrain)target;

        if (GUILayout.Button("Generate + Keep Heights"))
        {
            terrain.GeneratePerlinTerrain(true);
        }
        EditorGUILayout.Space();
    }
}