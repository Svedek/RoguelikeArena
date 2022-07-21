using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    public static WeaponManager Instance;

    void Awake() {
        // Singleton Setup
        if (Instance != null) {
            Debug.LogError("Multiple instances of WeaponManager!");
        }
        Instance = this;
    }


    public GameObject Pistol { get { return weapons[(int)WeaponID.pistol]; } }
    public GameObject SMG { get { return weapons[(int)WeaponID.smg]; } }
    public GameObject Magnum { get { return weapons[(int)WeaponID.magnum]; } }




    [SerializeField] private GameObject[] weapons;
    private enum WeaponID {
         pistol,

         smg,
         magnum,

         ar,
         shotgun,
         laserPistol,

         gatling,
         grenadeLauncher,
         sniper,

         laserGatling,
         rocketLauncher,
         railgun,
    }
}
