using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemManager))]
public class ItemManagerTesting : Editor {

    private readonly string[] toolbarOptions = {
        "Pis",
        "SMG",
        "Mag",
        "AR",
        "Shot",
        "LazP",
        "Gat",
        "Grnd",
        "Snpr",
        "LazG",
        "Rckt",
        "Rail"

    };
    int i = 0;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GUILayout.Space(10f);
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        GUILayout.Label("Give weapons", style);

        int temp;
        if (i != (temp = GUILayout.Toolbar(i, toolbarOptions))) {
            i = temp;
            ItemManager.Instance.GivePlayerWeapon((ItemManager.WeaponID)i);
        }
    }
}
