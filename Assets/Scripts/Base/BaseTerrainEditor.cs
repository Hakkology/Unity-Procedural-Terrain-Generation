using UnityEditor;

[CustomEditor(typeof(BaseTerrain), true)]
public class BaseTerrainEditor : CustomTerrainEditor
{
    protected SerializedProperty terrainProp;
    protected SerializedProperty terrainDataProp;

    // protected virtual yaptık
    protected virtual void OnEnable()
    {
        terrainProp = serializedObject.FindProperty("terrain");
        terrainDataProp = serializedObject.FindProperty("terrainData");
    }

    protected override void DrawTerrainParameters()
    {
        EditorGUILayout.PropertyField(terrainProp);
        EditorGUILayout.PropertyField(terrainDataProp);
    }
}