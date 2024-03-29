using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public static ItemManager Instance;

    //TESTING 
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) GivePlayerWeapon(WeaponID.pistol);
        if (Input.GetKeyDown(KeyCode.Alpha2)) GivePlayerWeapon(WeaponID.smg);
        if (Input.GetKeyDown(KeyCode.Alpha3)) GivePlayerWeapon(WeaponID.magnum);
        if (Input.GetKeyDown(KeyCode.Alpha4)) GivePlayerWeapon(WeaponID.ar);
        if (Input.GetKeyDown(KeyCode.Alpha5)) GivePlayerWeapon(WeaponID.shotgun);
        if (Input.GetKeyDown(KeyCode.Alpha6)) GivePlayerWeapon(WeaponID.laserPistol);
        if (Input.GetKeyDown(KeyCode.Alpha7)) GivePlayerWeapon(WeaponID.gatling);
        if (Input.GetKeyDown(KeyCode.Alpha8)) GivePlayerWeapon(WeaponID.grenadeLauncher);
        if (Input.GetKeyDown(KeyCode.Alpha9)) GivePlayerWeapon(WeaponID.sniper);
        if (Input.GetKeyDown(KeyCode.Alpha0)) GivePlayerWeapon(WeaponID.laserGatling);
    }



    void Awake() {
        // Singleton Setup
        if (Instance != null) {
            Debug.LogError("Multiple instances of ItemManager!");
        }
        Instance = this;
    }

    #region Weapons
    [Header("Weapons")]
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private GameObject[] weaponItems;
    private WeaponID playerWeapon = WeaponID.pistol;

    // Used for initializing player with pistol
    public void GiveStartWeapon() {
        GivePlayerWeapon(WeaponID.pistol);
    }
    public void GivePlayerWeapon(WeaponID id) {
        playerWeapon = id;
        PlayerController.Instance.GiveGun(weapons[(int)id]);
    }

    public GameObject[] WeaponsToSpawn() {
        WeaponID[] ids = GetWeaponChildren(playerWeapon);
        if (ids == null) return null;
        GameObject[] ret = new GameObject[ids.Length];

        for (int i = 0; i < ret.Length; i++) {
            ret[i] = weaponItems[(int)ids[i]];
        }

        return ret;
    }

    private WeaponID[] GetWeaponChildren(WeaponID id) {
        switch(id) {
            // Tier 0
            case WeaponID.pistol:
                return new WeaponID[] {
                    WeaponID.smg,
                    WeaponID.magnum
                };

            // Tier 1
            case WeaponID.smg:
                return new WeaponID[] {
                    WeaponID.ar,
                    WeaponID.shotgun
                };
            case WeaponID.magnum:
                return new WeaponID[] {
                    WeaponID.shotgun,
                    WeaponID.laserPistol
                };

            // Tier 2
            case WeaponID.ar:
                return new WeaponID[] {
                    WeaponID.gatling
                };
            case WeaponID.shotgun:
                return new WeaponID[] {
                    WeaponID.grenadeLauncher
                };
            case WeaponID.laserPistol:
                return new WeaponID[] {
                    WeaponID.sniper
                };

            // Tier 3
            case WeaponID.gatling:
                return new WeaponID[] {
                    WeaponID.laserGatling
                };
            case WeaponID.grenadeLauncher:
                return new WeaponID[] {
                    WeaponID.rocketLauncher
                };
            case WeaponID.sniper:
                return new WeaponID[] {
                    WeaponID.railgun
                };

            // Tier 4
            case WeaponID.laserGatling:
            case WeaponID.rocketLauncher:
            case WeaponID.railgun:
                return null;
        }

        Debug.LogError("ItemManager.GetWeaponChildren invalid input");
        return null;
    }
    
    /*
    private class WeaponNode {
        public static WeaponNode Root {
            get {
                return CreateTree();
            }
        }

        public WeaponNode[] children { get; private set; }
        public WeaponID Id { get; private set; }
        public string Title { get; private set; }
        public string Desc { get; private set; }
        public int Cost { get; private set; }

        public static void ChangeWeapon(WeaponNode current, WeaponID id) {
            foreach(WeaponNode node in current.children) {
                if(node.Id == id) {
                    current = node;
                    break;
                }
            }

            Debug.LogError("ItemManager.WeaponNode.ChangeWeapon not found");
        }

        private WeaponNode(WeaponID id, string title, string desc, int cost) {
            Id = id;
            Title = title;
            Desc = desc;
            Cost = cost;
        }

        private static WeaponNode CreateTree() {
            // Tier 0
            WeaponNode root = new WeaponNode(WeaponID.pistol, "Pistol", "Its kina bad lol, but gun mode", 10);
            root.children = new WeaponNode[] {
                new WeaponNode(WeaponID.smg, "SMG", "++ Fire Rate", 50), // 50
                new WeaponNode(WeaponID.magnum, "Magnum", "+ Damage\n+ Pierce", 30) // 30
            };

            // Tier 1
            WeaponNode smg = root.children[0];
            WeaponNode magnum = root.children[1];
            smg.children = new WeaponNode[] {
                new WeaponNode(WeaponID.ar, "Assault Rifle", "+ Damage", 75), // 150
                new WeaponNode(WeaponID.shotgun, "Shotgun", "+ Bullets\n- Fire Rate", 50) // 100
            };
            magnum.children = new WeaponNode[] {
                new WeaponNode(WeaponID.shotgun, "Shotgun", "+ Bullets\n- Pierce", 70), // 100
                new WeaponNode(WeaponID.laserPistol, "Laser Pistol", "", 50) // 80
            };

            // Tier 2
            WeaponNode ar = smg.children[0];
            WeaponNode shotgun_smg = smg.children[1];
            WeaponNode shotgun_magnum = magnum.children[0];
            WeaponNode laserPistol = magnum.children[1];
            ar.children = new WeaponNode[] {
                new WeaponNode(WeaponID.gatling, "Gatling Gun", "", 100), // 150
            };
            shotgun_smg.children = shotgun_magnum.children = new WeaponNode[] {
                new WeaponNode(WeaponID.grenadeLauncher, "Grenade Launcher", "", 100), // 100
            };
            laserPistol.children = new WeaponNode[] {
                new WeaponNode(WeaponID.sniper, "Sniper Rifle", "", 100), // 100
            };

            // Tier 3
            WeaponNode gatling = ar.children[0];
            WeaponNode grenade = shotgun_smg.children[0];
            WeaponNode sniper = laserPistol.children[0];
            gatling.children = new WeaponNode[] {
                new WeaponNode(WeaponID.laserGatling, "Laser Gatling Gun", "", 100), // 150
            };
            grenade.children = new WeaponNode[] {
                new WeaponNode(WeaponID.rocketLauncher, "Rocket Launcher", "", 100), // 100
            };
            sniper.children = new WeaponNode[] {
                new WeaponNode(WeaponID.railgun, "Railgun", "", 100), // 100
            };

            // Tier 4
            
            WeaponNode laserGatling = gatling.children[0];
            WeaponNode rocket = grenade.children[0];
            WeaponNode railgun = sniper.children[0];

            laserGatling.children = rocket.children = railgun.children = new WeaponNode[0];
            
            return root;
        }
    }
    */
    public enum WeaponID {
        pistol, // -> smg, magnum

        smg, // -> ar, shotgun
        magnum, // -> shotgun, laserPistol

        ar, // -> gatling
        shotgun, // -> grenade
        laserPistol, // -> rocket

        gatling, // -> laserGatling
        grenadeLauncher, // -> rocketLauncher
        sniper, // -> railgun

        laserGatling,
        rocketLauncher,
        railgun,
    }
    #endregion

    #region Populate
    private const int minimumRelics = 0;
    public void PopulateItems(ShopStall[] stalls) {
        var toPopulate = stalls.Length;


        for (int i = 0; i < toPopulate; i++) { // TODO (right now it only populates w potions)
            int id = Random.Range(0, potions.Length);
            stalls[i].Initialize(potions[id]);
        }
    }
    #endregion

    #region Potions 
    [Header("Potions")]
    [SerializeField] GameObject[] potions;
    private enum PotionID {
        smolPot,
        medPot,
        largePot,
    }
    #endregion

    #region Relics
    [Header("Relics")]
    [SerializeField] GameObject[] relics;
    private static List<Relic.RelicID> relicsAvailable = ((Relic.RelicID[])System.Enum.GetValues(typeof(Relic.RelicID))).ToList();
    #endregion
}
