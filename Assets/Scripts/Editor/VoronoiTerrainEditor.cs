using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoronoiTerrain))]
public class VoronoiTerrainEditor : BaseTerrainEditor
{
    SerializedProperty voronoiFalloff;
    SerializedProperty voronoiPower;
    bool showVoronoiSection = true;

    // düzeltme: protected override void OnEnable()
    protected override void OnEnable()
    {
        base.OnEnable();
        voronoiFalloff = serializedObject.FindProperty("voronoiFalloff");
        voronoiPower = serializedObject.FindProperty("voronoiPower");
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();

        showVoronoiSection = EditorGUILayout.Foldout(showVoronoiSection, "Voronoi Terrain Settings");
        if (showVoronoiSection)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Voronoi Section", EditorStyles.boldLabel);
            EditorGUILayout.Slider(voronoiFalloff, 0, 1, new GUIContent("Falloff"));
            EditorGUILayout.Slider(voronoiPower, 0, 1, new GUIContent("Power"));
        }
    }
}