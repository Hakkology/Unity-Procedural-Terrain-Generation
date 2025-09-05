using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VoronoiTerrain))]
public class VoronoiTerrainEditor : BaseTerrainEditor
{

    bool showVoronoiSection = true;

    // düzeltme: protected override void OnEnable()
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();

        showVoronoiSection = EditorGUILayout.Foldout(showVoronoiSection, "Voronoi Terrain Settings");
        if (showVoronoiSection)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Voronoi Section", EditorStyles.boldLabel);
        }
    }
}