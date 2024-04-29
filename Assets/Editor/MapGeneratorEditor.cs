using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    private MapGenerator mapGenerator;
    private SerializedProperty widthProperty;
    private SerializedProperty heightProperty;
    private SerializedProperty originProperty;
    private SerializedProperty tilePrefabProperty;
    private SerializedProperty tileSizeProperty;
    private SerializedProperty straightRoadProperty;
    private SerializedProperty cornerRoadProperty;
    private SerializedProperty connectiveRoadProperty;
    private SerializedProperty roadSizeProperty;
    private SerializedProperty finalRoadCountProperty;
    private SerializedProperty terrainPrefabProperty;
    private SerializedProperty terrainYOffsetProperty;
    private SerializedProperty tileObjectMapsProperty;
    private SerializedProperty modeProperty;
    private SerializedProperty matchColorProperty;
    private SerializedProperty tileObjectTypeProperty;
    private SerializedProperty yRotationProperty;
    private SerializedProperty exitBarrierPrefabProperty;
    private SerializedProperty barrierZOffsetProperty;



    private void OnEnable()
    {
        mapGenerator = target as MapGenerator;
        widthProperty = serializedObject.FindProperty("width");
        heightProperty = serializedObject.FindProperty("height");
        originProperty = serializedObject.FindProperty("origin");
        tilePrefabProperty = serializedObject.FindProperty("tilePrefab");
        tileSizeProperty = serializedObject.FindProperty("tileSize");
        straightRoadProperty = serializedObject.FindProperty("straightRoad");
        cornerRoadProperty = serializedObject.FindProperty("cornerRoad");
        connectiveRoadProperty = serializedObject.FindProperty("connectiveRoad");
        roadSizeProperty = serializedObject.FindProperty("roadSize");
        finalRoadCountProperty = serializedObject.FindProperty("finalRoadCount");
        terrainPrefabProperty = serializedObject.FindProperty("terrainPrefab");
        terrainYOffsetProperty = serializedObject.FindProperty("terrainYOffset");
        tileObjectMapsProperty = serializedObject.FindProperty("tileObjectMaps");
        modeProperty = serializedObject.FindProperty("Mode");
        matchColorProperty = serializedObject.FindProperty("matchColor");
        tileObjectTypeProperty = serializedObject.FindProperty("tileObjectType");
        yRotationProperty = serializedObject.FindProperty("yRotation");
        exitBarrierPrefabProperty = serializedObject.FindProperty("exitBarrierPrefab");
        barrierZOffsetProperty = serializedObject.FindProperty("barrierZOffset");

    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawSettings();
        EditorGUILayout.Space(20);
        DrawButtons();
        DrawPaintSettings();
        DrawGrid();
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawSettings()
    {
        if (!mapGenerator.isMapGenerated)
        {
            EditorGUILayout.PropertyField(widthProperty);
            EditorGUILayout.PropertyField(heightProperty);
            EditorGUILayout.PropertyField(originProperty);

            EditorGUILayout.PropertyField(tilePrefabProperty);
            EditorGUILayout.PropertyField(tileSizeProperty);

            EditorGUILayout.PropertyField(straightRoadProperty);
            EditorGUILayout.PropertyField(cornerRoadProperty);
            EditorGUILayout.PropertyField(connectiveRoadProperty);
            EditorGUILayout.PropertyField(roadSizeProperty);
            EditorGUILayout.PropertyField(finalRoadCountProperty);

            EditorGUILayout.PropertyField(exitBarrierPrefabProperty);
            EditorGUILayout.PropertyField(barrierZOffsetProperty);

            EditorGUILayout.PropertyField(terrainPrefabProperty);
            EditorGUILayout.PropertyField(terrainYOffsetProperty);
        }
    }
    private void DrawButtons()
    {
        if (!mapGenerator.isMapGenerated)
        {
            if (GUILayout.Button("Generate Map"))
            {
                mapGenerator.GenerateMap();
            }
        }

        if (mapGenerator.isMapGenerated)
        {
            if (GUILayout.Button("Clear Map"))
            {
                mapGenerator.ClearMap();
            }
        }
    }
    private void DrawPaintSettings()
    {
        if (mapGenerator.isMapGenerated)
        {
            EditorGUILayout.Space(40);
            EditorGUILayout.PropertyField(tileObjectMapsProperty);
            EditorGUILayout.Space(30);
            GUI.backgroundColor = Color.red;
            EditorGUILayout.PropertyField(modeProperty);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(tileObjectTypeProperty);
            EditorGUILayout.PropertyField(matchColorProperty);
            EditorGUILayout.PropertyField(yRotationProperty);
        }
    }
    private void DrawGrid()
    {

        if (mapGenerator.isMapGenerated)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.fontSize = 12;
            style.fontStyle = FontStyle.Bold;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);

            for (int y = heightProperty.intValue - 1; y >= 0; y--)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                for (int x = 0; x < widthProperty.intValue; x++)
                {
                    string buttonName = x + "_" + y;

                    TileDummy tileDummy = mapGenerator.GetTile(x, y);
                    if (tileDummy != null) GUI.backgroundColor = tileDummy.Color;

                    if (GUILayout.Button(buttonName, style, GUILayout.Width(50), GUILayout.Height(50)))
                    {
                        if (mapGenerator.Mode == Mode.Create)
                        {
                            mapGenerator.Create(x, y);
                        }
                        else if (mapGenerator.Mode == Mode.Erase)
                        {
                            mapGenerator.Erase(x, y);
                        }
                    }
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
