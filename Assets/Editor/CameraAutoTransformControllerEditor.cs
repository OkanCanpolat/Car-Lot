using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraAutoTransformController))]
public class CameraAutoTransformControllerEditor : Editor
{
    private bool editorScripting;
    private CameraAutoTransformController cameraAutoTransformController;
    private SerializedProperty widthProperty;
    private SerializedProperty heightProperty;
    private SerializedProperty midTileProperty;
    private SerializedProperty lastXTileProperty;

    private GUIContent midTileGUIContent = new GUIContent("Mid Tile", "Assign tile at [width/2, height/2]");
    private GUIContent lastXTileGUIContent = new GUIContent("Last X Tile", "Assign tile at last column and first row. Like [3,0], [7,0] etc");
    private void OnEnable()
    {
        cameraAutoTransformController = target as CameraAutoTransformController;
        widthProperty = serializedObject.FindProperty("width");
        heightProperty = serializedObject.FindProperty("height");
        midTileProperty = serializedObject.FindProperty("midTile");
        lastXTileProperty = serializedObject.FindProperty("lastXTile");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        editorScripting = EditorGUILayout.Toggle("Editor Scripting", editorScripting);

        if (editorScripting)
        {
            EditorGUI.indentLevel++;
            widthProperty.intValue = EditorGUILayout.IntField("Width", widthProperty.intValue);
            heightProperty.intValue = EditorGUILayout.IntField("Height", heightProperty.intValue);
            midTileProperty.objectReferenceValue = EditorGUILayout.ObjectField(midTileGUIContent, midTileProperty.objectReferenceValue, typeof(Tile), true) as Tile;
            lastXTileProperty.objectReferenceValue = EditorGUILayout.ObjectField(lastXTileGUIContent, lastXTileProperty.objectReferenceValue, typeof(Tile), true) as Tile;

            EditorGUILayout.Space();

            if (GUILayout.Button("Reposition Camera"))
            {
                cameraAutoTransformController.RepositionCamera();
            }
            EditorGUI.indentLevel = 0;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
