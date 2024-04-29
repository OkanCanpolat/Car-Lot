using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridSystem))]
public class GridSystemEditor : Editor
{
    private GUIContent adjustmentContent = new GUIContent("Auto Adjustment", "Enable to perform automatic tile adjustment by name. Disable to perform manually. Use it with Map Generator");
    private string[] exludeProperties = new string[] { "width", "height", "automaticTileAdjusment" };
    private SerializedProperty widthProperty;
    private SerializedProperty heightProperty;


    private void OnEnable()
    {
        widthProperty = serializedObject.FindProperty("width");
        heightProperty = serializedObject.FindProperty("height");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();

        GridSystem gridSystem = target as GridSystem;

        EditorGUILayout.Space();
        gridSystem.AutomaticTileAdjusment = EditorGUILayout.Toggle(adjustmentContent, gridSystem.AutomaticTileAdjusment);

        if (gridSystem.AutomaticTileAdjusment)
        {
            gridSystem.TileSize = EditorGUILayout.FloatField("Tile Size", gridSystem.TileSize);
            widthProperty.intValue = EditorGUILayout.IntField("Width", widthProperty.intValue);
            heightProperty.intValue = EditorGUILayout.IntField("Height", heightProperty.intValue);
        }
        else
        {
            DrawPropertiesExcluding(serializedObject, exludeProperties);
            serializedObject.ApplyModifiedProperties();
        }

        serializedObject.ApplyModifiedProperties();
    }

}
