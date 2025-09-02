using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(ProceduralTerrain))]
[CanEditMultipleObjects]
public class CustomTerrainEditor : Editor
{
    // foldouts
    bool showRandom = false;

    // properties
    SerializedProperty randomHeightRange;
    void OnEnable()
    {
        randomHeightRange = serializedObject.FindProperty("RandomHeightRange");
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

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            if (GUILayout.Button("Reset Terrain"))
            {
                terrain.ResetTerrain();
            }
        }



        serializedObject.ApplyModifiedProperties();
    }
}