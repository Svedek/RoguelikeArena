using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditorScript : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Print Scaling")) {
            Debug.Log(((GameManager)target).Scaling);
        }
        if (GUILayout.Button("+5 Scaling")) {
            ((GameManager)target).IncreaseScaling(5f);
        }
        if (GUILayout.Button("-5 Scaling")) {
            ((GameManager)target).IncreaseScaling(-5f);
        }
    }
}