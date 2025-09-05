using EditorGUITable;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerlinTerrain))]
public class PerlinTerrainEditor : BaseTerrainEditor
{
    SerializedProperty perlinXScale;
    SerializedProperty perlinYScale;
    SerializedProperty perlinXOffset;
    SerializedProperty perlinYOffset;

    SerializedProperty perlinOctaves;
    SerializedProperty perlinPersistance;
    SerializedProperty perlinHeightScale;
    SerializedProperty perlinRidgeConstant;

    GUITableState perlinParameterTable;
    SerializedProperty perlinParameters;

    bool showPerlin = false;
    bool showMultiplePerlin = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        perlinXScale = serializedObject.FindProperty("perlinXScale");
        perlinYScale = serializedObject.FindProperty("perlinYScale");
        perlinXOffset = serializedObject.FindProperty("perlinXOffset");
        perlinYOffset = serializedObject.FindProperty("perlinYOffset");
        perlinOctaves = serializedObject.FindProperty("perlinOctaves");
        perlinPersistance = serializedObject.FindProperty("perlinPersistance");
        perlinHeightScale = serializedObject.FindProperty("perlinHeightScale");
        perlinRidgeConstant = serializedObject.FindProperty("perlinRidgeConstant");

        perlinParameterTable = new GUITableState("perlinParameterTable");
        perlinParameters = serializedObject.FindProperty("perlinParameters");
    }

    protected override void DrawTerrainParameters()
    {
        base.DrawTerrainParameters();
        

        showPerlin = EditorGUILayout.Foldout(showPerlin, "Perlin Settings");
        if (showPerlin)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Perlin Noise", EditorStyles.boldLabel);

            EditorGUILayout.Slider(perlinXScale, 0, 1, new GUIContent("X Scale"));
            EditorGUILayout.Slider(perlinYScale, 0, 1, new GUIContent("Y Scale"));
            EditorGUILayout.IntSlider(perlinXOffset, 0, 10000, new GUIContent("X Offset"));
            EditorGUILayout.IntSlider(perlinYOffset, 0, 10000, new GUIContent("Y Offset"));
            EditorGUILayout.Space();

            GUILayout.Label("Brownian Perlin Settings", EditorStyles.boldLabel);
            EditorGUILayout.IntSlider(perlinOctaves, 1, 10, new GUIContent("Octaves"));
            EditorGUILayout.Slider(perlinPersistance, .1f, 10, new GUIContent("Persistance"));
            EditorGUILayout.Slider(perlinHeightScale, 0, 1, new GUIContent("Height Scale"));
            EditorGUILayout.Space();

            GUILayout.Label("Ridge Perlin Settings", EditorStyles.boldLabel);
            EditorGUILayout.Slider(perlinRidgeConstant, 0, 1, new GUIContent("Ridge Constant"));
        }

        EditorGUILayout.Space();
        showMultiplePerlin = EditorGUILayout.Foldout(showMultiplePerlin, "Multiple Perlin Settings");
        if (showMultiplePerlin)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Multiple Perlin Noise", EditorStyles.boldLabel);

            PerlinTerrain terrain = (PerlinTerrain)target;
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                terrain.AddNewPerlin();
            }
            if (GUILayout.Button("-"))
            {
                terrain.RemovePerlin();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            perlinParameterTable = GUITableLayout.DrawTable(perlinParameterTable,
                                        serializedObject.FindProperty("perlinParameters"));
            GUILayout.Space(20);
        }
    }

    protected override void DrawAdditionalButtons()
    {
        PerlinTerrain terrain = (PerlinTerrain)target;

        if (GUILayout.Button("Generate Terrain + Perlin Fractal"))
        {
            terrain.GeneratePerlinTerrain(true);
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Terrain + Perlin Multiple"))
        {
            terrain.GenerateMultiplePerlinTerrain();
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Terrain + Perlin Multiple + Ridge"))
        {
            terrain.GenerateRidgeNoiseTerrain();
        }
        EditorGUILayout.Space();
    }
}