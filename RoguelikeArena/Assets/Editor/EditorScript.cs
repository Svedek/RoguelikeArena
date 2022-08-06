using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class EditorScript : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        if (GUILayout.Button("Give Exp")) {
            ((PlayerController)target).GainExperience(10000f);
        }
    }
}
