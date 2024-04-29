using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[CustomEditor(typeof(CarMovementController))]
public class CarMovementControllerEditor : Editor
{
    public static List<Type> CommandTypes = new List<Type>();
    private CarMovementController carMovementController;
    private bool showCommands = false;
    private bool showInstructions = false;
    private string[] excludingProperties = new string[] { "EditorScripting" };
    private int index = 0;
    private void OnEnable()
    {
        carMovementController = target as CarMovementController;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject, excludingProperties);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (Application.isPlaying)
        {
            carMovementController.EditorScripting = EditorGUILayout.Toggle("Editor Scripting", carMovementController.EditorScripting);
        }

        index = EditorGUILayout.IntSlider(new GUIContent("List Index", "Operations which are related with commands will be made on the list at this index"), index, 0, carMovementController.ExitPaths.Count - 1);
        showCommands = EditorGUILayout.Foldout(showCommands, "Add Command");

        if (showCommands)
        {
            foreach (Type type in CommandTypes)
            {
                if (GUILayout.Button(type.Name))
                {
                    var command = Activator.CreateInstance(type) as TransformCommandBase;
                    carMovementController.AddCommond(command, index);
                }
            }
        }

        if (carMovementController.EditorScripting && Application.isPlaying)
        {
            showInstructions = EditorGUILayout.Foldout(showInstructions, "Instructions");

            if (showInstructions)
            {
                if (GUILayout.Button("Execute Commands"))
                {
                    carMovementController.RunCommandsEditor(index);
                }
            }
            if (showInstructions)
            {
                if (GUILayout.Button("Stop Commands"))
                {
                    if (carMovementController.CancellationTokenSource != null)
                    {
                        carMovementController.CancellationTokenSource.Cancel();
                    }
                }
            }
            if (showInstructions)
            {
                if (GUILayout.Button("Reset Transform"))
                {
                    carMovementController.ResetTransform();
                }
            }
        }
    }


    [DidReloadScripts]
    private static void GetTypes()
    {
        var assamblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assamblies.SelectMany(assambly => assambly.GetTypes());
        var filteredTypes = types.Where(type => type.IsSubclassOf(typeof(TransformCommandBase)));
        CommandTypes = filteredTypes.ToList();
    }

}
