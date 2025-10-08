using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MidpointTerrain))]
public class MidpointTerrainEditor : BaseTerrainEditor
{
    // SerializedProperty randomHeightRange;
    bool midpoint = true;

    protected override void OnEnable()
    {
        base.OnEnable();
        // randomHeightRange = serializedObject.FindProperty("randomHeightRange");
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();

        midpoint = EditorGUILayout.Foldout(midpoint, "Midpoint Parameters");
        if (midpoint)
        {
            // EditorGUILayout.PropertyField(randomHeightRange);
        }
    }
}