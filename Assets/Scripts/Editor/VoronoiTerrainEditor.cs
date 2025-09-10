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
    bool showVoronoiSection = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        voronoiDropoff = serializedObject.FindProperty("voronoiDropoff");
        voronoiFalloff = serializedObject.FindProperty("voronoiFalloff");
        voronoiPeakCount = serializedObject.FindProperty("voronoiPeakCount");
        voronoiMinHeight = serializedObject.FindProperty("voronoiMinHeight");
        voronoiMaxHeight = serializedObject.FindProperty("voronoiMaxHeight");
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
        }
    }

    protected override void DrawAdditionalButtons()
    {
        VoronoiTerrain terrain = (VoronoiTerrain)target;

EditorGUILayout.Space();
        if (GUILayout.Button("Generate Terrain + Multiple Peaks"))
        {
            terrain.GenerateVoronoiPeakTerrain();
        }
        EditorGUILayout.Space();
    }
}