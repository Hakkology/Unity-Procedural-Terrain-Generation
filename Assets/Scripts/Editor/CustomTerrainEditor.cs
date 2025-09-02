using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(ProceduralTerrain))]
[CanEditMultipleObjects]
public class CustomTerrainEditor : Editor
{
    // foldouts
    bool showRandom = false;
    bool showLoadHeights = false;

    // properties
    SerializedProperty randomHeightRange;
    SerializedProperty heightMapScale;
    SerializedProperty heightMapImage;

    void OnEnable()
    {
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        heightMapScale = serializedObject.FindProperty("heightMapScale");
        heightMapImage = serializedObject.FindProperty("heightMapImage");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ProceduralTerrain terrain = (ProceduralTerrain)target;

        showRandom = EditorGUILayout.Foldout(showRandom, "Random Heights");
        if (showRandom)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set Heights between Random Values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(randomHeightRange);
            if (GUILayout.Button("Random Heights"))
            {
                terrain.RandomTerrain();
            }
        }

        showLoadHeights = EditorGUILayout.Foldout(showLoadHeights, "Load Heights");
        if (showLoadHeights)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Load Heights from Texture", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(heightMapScale);
            EditorGUILayout.PropertyField(heightMapImage);
            if (GUILayout.Button("Load Texture"))
            {
                terrain.LoadTexture(keepHeights: false);
            }
            if (GUILayout.Button("Load Texture with Heights"))
            {
                terrain.LoadTexture(keepHeights: true);
            }
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Reset Terrain"))
        {
            terrain.ResetTerrain();
        }



        serializedObject.ApplyModifiedProperties();
    }
}