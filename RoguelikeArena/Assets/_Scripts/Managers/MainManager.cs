using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour {

    private static MainManager instance;
    public static MainManager Instance { 
        get {
            if (instance == null)
                instance = new MainManager();
            return instance;
        }
    }


}
