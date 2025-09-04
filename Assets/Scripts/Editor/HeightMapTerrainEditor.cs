using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HeightMapTerrain))]
public class HeightMapTerrainEditor : BaseTerrainEditor
{
    SerializedProperty heightMapImage;
    SerializedProperty heightMapScale;
    bool showLoadHeights = true;

    // düzeltme: protected override void OnEnable()
    protected override void OnEnable()
    {
        base.OnEnable();
        heightMapImage = serializedObject.FindProperty("heightMapImage");
        heightMapScale = serializedObject.FindProperty("heightMapScale");
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();

        showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Heightmap Settings");
        if (showLoadHeights)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Heights from Texture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(heightMapScale);
            EditorGUILayout.PropertyField(heightMapImage);
        }
    }
}