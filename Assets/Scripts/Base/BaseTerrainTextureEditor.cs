using EditorGUITable;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseTerrainTexture), true)]
public class BaseTerrainTextureEditor : Editor
{
    protected SerializedProperty terrainProp;
    protected SerializedProperty terrainDataProp;

    public bool showSplatMaps = false;
    GUITableState splatMapTable;
    protected SerializedProperty splatHeights;

    protected virtual void OnEnable()
    {
        terrainProp = serializedObject.FindProperty("terrain");
        terrainDataProp = serializedObject.FindProperty("terrainData");
        splatMapTable = new GUITableState("splatMapTable");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawTerrainTextureParameters();
        EditorGUILayout.Space();
        DrawAdditionalButtons();

        serializedObject.ApplyModifiedProperties();
    }

    protected virtual void DrawTerrainTextureParameters()
    {
        EditorGUILayout.PropertyField(terrainProp);
        EditorGUILayout.PropertyField(terrainDataProp);

        ITexturable terrain = (ITexturable)target;

        EditorGUILayout.Space();

        showSplatMaps = EditorGUILayout.Foldout(showSplatMaps, "Splat Maps");
        if (showSplatMaps)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Splat Maps", EditorStyles.boldLabel);
            splatMapTable = GUITableLayout.DrawTable(splatMapTable, serializedObject.FindProperty("splatHeights"));

            GUILayout.Space(20);

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("+"))
            {
                terrain.AddNewSplatHeight();
            }

            if (GUILayout.Button("-"))
            {
                terrain.RemoveSplatHeights();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Apply Splatmaps"))
            {
                terrain.SplatMaps();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Reset Splatmaps"))
            {
                terrain.ResetAllTerrainLayers();
            }
        }
    }
    
    protected virtual void DrawAdditionalButtons() { }

}