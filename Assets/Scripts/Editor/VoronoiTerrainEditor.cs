using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoronoiTerrain))]
public class VoronoiTerrainEditor : BaseTerrainEditor
{
    SerializedProperty voronoiPeakCount;
    SerializedProperty voronoiFalloff;
    SerializedProperty voronoiDropoff;
    SerializedProperty voronoiMinHeight;
    SerializedProperty voronoiMaxHeight;
    SerializedProperty voronoiRoughness;
    SerializedProperty voronoiSharpness;
    SerializedProperty voronoiPlateau;
    SerializedProperty perlinParameters;
    bool showVoronoiSection = true;
    bool showPerlinSection = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        voronoiDropoff = serializedObject.FindProperty("voronoiDropoff");
        voronoiFalloff = serializedObject.FindProperty("voronoiFalloff");
        voronoiPeakCount = serializedObject.FindProperty("voronoiPeakCount");
        voronoiMinHeight = serializedObject.FindProperty("voronoiMinHeight");
        voronoiMaxHeight = serializedObject.FindProperty("voronoiMaxHeight");
        voronoiRoughness = serializedObject.FindProperty("voronoiRoughness");
        voronoiSharpness = serializedObject.FindProperty("voronoiSharpness");
        voronoiPlateau = serializedObject.FindProperty("voronoiPlateau");
        perlinParameters = serializedObject.FindProperty("perlinParameters");
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();

        showVoronoiSection = EditorGUILayout.Foldout(showVoronoiSection, "Voronoi Terrain Settings");
        if (showVoronoiSection)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Voronoi Section", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            EditorGUILayout.IntSlider(voronoiPeakCount, 1, 8, new GUIContent("Peak Count"));
            EditorGUILayout.Slider(voronoiFalloff, 0, 10, new GUIContent("Falloff"));
            EditorGUILayout.Slider(voronoiDropoff, 0, 10, new GUIContent("Dropoff"));
            EditorGUILayout.Slider(voronoiMinHeight, 0, 1f, new GUIContent("Min Height"));
            EditorGUILayout.Slider(voronoiMaxHeight, 0, 1f, new GUIContent("Max Height"));

            EditorGUILayout.Space();
            GUILayout.Label("Voronoi Realistic Section", EditorStyles.boldLabel);
            EditorGUILayout.Slider(voronoiRoughness, 0, 1f, new GUIContent("Roughness"));
            EditorGUILayout.Slider(voronoiSharpness, 0, 1f, new GUIContent("Sharpness"));
            EditorGUILayout.Slider(voronoiPlateau, 0, 1f, new GUIContent("Plateau"));
            EditorGUILayout.Space();
        }

        showPerlinSection = EditorGUILayout.Foldout(showPerlinSection, "Additional Perlin Terrain Settings");
        if (showPerlinSection)
        {
            
        }
    }

    protected override void DrawAdditionalButtons()
    {
        VoronoiTerrain terrain = (VoronoiTerrain)target;

        if (GUILayout.Button("Generate Terrain + Multiple Peaks"))
        {
            terrain.GenerateVoronoiPeakTerrain();
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Terrain + Realistic"))
        {
            terrain.GenerateVoronoiRealisticTerrain();
        }
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Terrain + Perlin"))
        {
            terrain.GenerateVoronoiPerlinTerrain();
        }
        EditorGUILayout.Space();
    }
}