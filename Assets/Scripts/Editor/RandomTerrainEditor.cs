using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomTerrain))]
public class RandomTerrainEditor : BaseTerrainEditor
{
    SerializedProperty randomHeightRange;
    bool showRandom = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();

        showRandom = EditorGUILayout.Foldout(showRandom, "Random Heights");
        if (showRandom)
        {
            EditorGUILayout.PropertyField(randomHeightRange);
        }
    }
}