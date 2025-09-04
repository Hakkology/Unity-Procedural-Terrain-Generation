using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerlinTerrain))]
public class PerlinTerrainEditor : BaseTerrainEditor
{
    SerializedProperty perlinXScale;
    SerializedProperty perlinYScale;
    bool showLoadHeights = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        perlinXScale = serializedObject.FindProperty("perlinXScale");
        perlinYScale = serializedObject.FindProperty("perlinYScale");
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();

        showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Heightmap Settings");
        if (showLoadHeights)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Heights from Texture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(perlinXScale);
            EditorGUILayout.PropertyField(perlinYScale);
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