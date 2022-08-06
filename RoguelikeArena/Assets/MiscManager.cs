using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This should all be organized out of this class later
public class MiscManager : MonoBehaviour {

    public static MiscManager Instance { get; private set; }

    void Awake() {
        // Singleton Setup
        if (Instance != null) {
            Debug.LogError("Multiple instances of MiscManager!");
        }
        Instance = this;
    }

    [field: SerializeField] public Transform BulletParent { get; private set; }
}
