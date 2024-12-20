using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveAndLoad))]
public class SaveAndLoadEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default Inspector fields
        DrawDefaultInspector();

        // Get a reference to the target object
        SaveAndLoad saveAndLoad = (SaveAndLoad)target;

        // Add a button to the Inspector
        if (GUILayout.Button("Quick Load"))
        {
            saveAndLoad.Load();
        }
    }
}