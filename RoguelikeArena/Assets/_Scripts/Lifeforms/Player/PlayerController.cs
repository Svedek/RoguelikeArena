using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health {

    public static PlayerController Instance;

    #region Referances
    Camera cam;
    Rigidbody2D rb;
    void Awake() {
        // Set referance in referance manager
        if (Instance != null) {
            Destroy(gameObject);
        }
        Instance = this;

        // Get refferances
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody2D>();

        // Initialize Experience
        Initialize();
    }
    #endregion

    #region Player Stats
    private static class Stats {
        private static float[] increaseAmmount =  {
            1.25f, // damageMod
            0.8f, // attackSpeedMod

            1.2f, // moveMod
            1.5f, // defence
            1.5f, // maxHealth

            1.35f, // expMod
            1.5f, // goldMod
        };

        private static float[] stats = {
            1f, // damageMod
            1f, // attackSpeedMod

            1f, // moveMod
            1f, // defense
            5f, // maxHealth

            1f, // expMod
            1f, // goldMod
        };

        public static void ApplyUpgrade(int id) {
            stats[id] *= increaseAmmount[id];
        }

        #region Getters
        public static float GetDamageMod() => stats[0];
        public static float GetAttackSpeedMod() => stats[1];

        public static float GetMoveMod() => stats[2];
        public static float GetDefense() => stats[3];
        public static float GetMaxHealth() => stats[4];

        public static float GetExpMod() => stats[5];
        public static float GetGoldMod() => stats[6];
        #endregion
    }

#endregion

    #region Health
    [Header("Health")]
    [SerializeField] private float health;
    public void TakeDamage(float damage, Vector2 kbDir) {
        if ((health -= damage) <= 0) Die();
        // TAKE KNOCKBACK
    }
    private void Die() {
        // Perish
        print("Player Died");
    }
    #endregion

    private void Start() {
        // Give pistol
        GiveWeapon(0);
    }

    void Update() {
        GatherInput();

        CalculateMovement();

        HandleUpgrades();

        Attack();

        Move();
    }
    
    #region Input
    private FrameInput input;
    struct FrameInput {
        public float x;
        public float y;
        public bool rollPressed;
        public bool attackPressed;
        public bool interactPressed;
        public Vector2 mousePos;

        public int upgradeSelected; 
    }

    private void GatherInput() {
        var upgSel = Input.GetButtonDown("Upgrade1") ? 0 : -1;
        upgSel = Input.GetButtonDown("Upgrade2") ? 1 : upgSel;
        upgSel = Input.GetButtonDown("Upgrade3") ? 2 : upgSel;


        input = new FrameInput {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical"),
            rollPressed = Input.GetButtonDown("Roll"),
            attackPressed = Input.GetButton("Fire"),
            interactPressed = Input.GetButton("Interact"),
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition),
            upgradeSelected = upgSel,
        };
    }
    #endregion

    #region movement 
    [Header("Movement")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private float rollSpeedMod = 1.25f, rollTime = .2f, rollDelay = .5f;
    private bool rolling = false, rollAvailable = true;

    private Vector2 moveDir;
    private void CalculateMovement() {
        // Calculate direction
        moveDir = rolling ? moveDir : new Vector2(input.x, input.y);
        moveDir.Normalize();

        // Start roll
        if (input.rollPressed && rollAvailable) {
            Roll();
        }

        // Calculate modifiers
        var moveMod = baseSpeed * Stats.GetMoveMod();
        moveMod *= rolling ? rollSpeedMod : 1f;

        // Finalize moveDir
        moveDir *= moveMod;
    }

    private void Move() {
        rb.velocity = moveDir;
    }


    private void Roll() { // TODO invincibility durring roll + VFX for rolling
        rolling = true;
        rollAvailable = false;
        StartCoroutine(EndRoll());
    }

    private IEnumerator EndRoll() {
        print("poggy!");
        yield return new WaitForSeconds(rollTime);
        rolling = false;
        yield return new WaitForSeconds(rollDelay - rollTime);
        rollAvailable = true;
    }

    #endregion

    #region Attacks
    [Header("Attack")]
    [SerializeField] private BulletTrail bulletTrail;
    struct WeaponStats {
        public float fireRate; // delay in secs between shots
        public float damage;
        public float knockback;
    }

    WeaponStats weaponStats;

    private float lastAttack;
    private void Attack() {
        if (input.attackPressed && lastAttack + (weaponStats.fireRate * Stats.GetAttackSpeedMod()) <= Time.time) {
            // Handle bullet
            Fire();
            lastAttack = Time.time;

            // Handle player knockback
        }
    }

    private void Fire() {
        int layerMask = LayerMask.GetMask("Enemy", "Terrain");
        float maxRange = 20f;
        Vector2 startPos = transform.position;
        Vector2 fireDir = (input.mousePos - startPos).normalized;

        RaycastHit2D hitInfo = Physics2D.Raycast(startPos, fireDir, maxRange, layerMask);

        if (hitInfo) {
            // Apply hit
            Health target = hitInfo.transform.GetComponent<Health>();
            if (target != null) {
                target.TakeDamage(weaponStats.damage * Stats.GetDamageMod(), fireDir*weaponStats.knockback);
            }

            // Make trail
            bulletTrail.MakeTrail(startPos, hitInfo.point);

        } else {
            // Make trail
            bulletTrail.MakeTrail(startPos, startPos + fireDir * maxRange);
        }

    }

    private void GiveWeapon(int id) { // TODO   GOING TO ENTIRELY RE-DO weapons, data will be held in external weapon clases and handled within
                weaponStats.fireRate = 0.2f;
                weaponStats.damage = 1f;
                weaponStats.knockback = 10f;
    }
    private void DropWeapon() {
        
    }

    #endregion

    #region Initialization
    private void Initialize() {
        SetLevelExpRequirement();
    }

    private void Reset() {
        currentExp = gold = 0;
        level = 1;
        SetLevelExpRequirement();

        // TODO Reset stats!
    }
    #endregion

    #region Experience and Levels
    private float currentExp = 0f, requiredExp = 11f;
    private int level = 1, gold = 0;

    private bool upgradesActive = false;
    private int upgradePoints = 0;
    private int[] ids;

    private void HandleUpgrades() {
        if (input.upgradeSelected != -1 && upgradePoints > 0) {
            ApplyUpgrade();
        }
    }

    public void GainExperience(float experience) {
        Debug.Log("cXp: " + currentExp + " XpG: " + experience + " RXp: " + requiredExp);
        if ((currentExp += (experience * Stats.GetExpMod())) >= requiredExp) {
            // Handle Experience
            currentExp -= requiredExp;
            LevelUp();
            GainExperience(0f); // Allows for multiple level ups

        }
        // Update UI
        UIManager.Instance.UpdateExperience(currentExp, requiredExp, level);
    }
    private void LevelUp() {
        // Handle Experience
        ++level;
        SetLevelExpRequirement();

        // Allow to pick upgrade (small UI popup with prompts, not pausing game) + Heal
        ++upgradePoints;
        SetupLevelUpgrades();

        // UI Effect
    }
    private void SetLevelExpRequirement() {
        // (1,11) (2,16.2) (3, 22.5) (4,29.6) (5,37.4)
        requiredExp = Mathf.Pow(level, 1.7f) + (level * 3) + (7);
    }
    private void SetupLevelUpgrades() {
        ids = new int[3];
        Color[] colors = new Color[3];
        string[] text = new string[3];
        if (!upgradesActive) {
            for (int i = 0; i < 3; i++) {
                float val = Random.Range(0f, 100f);
                ids[i] = ValToID(val, i);
                colors[i] = upgradeColors[ids[i]];
                text[i] = upgradeText[ids[i]];
            }


            UIManager.Instance.SetUpgradeOptions(colors, text);

            upgradesActive = true;
        }
    }

    private void ApplyUpgrade() {
        var id = ids[input.upgradeSelected];
        Stats.ApplyUpgrade(id);
        upgradesActive = false;

        print("upgradePoints: " + upgradePoints + "   id: " + id);
        if (--upgradePoints > 0) {
            SetupLevelUpgrades();
        } else {
            UIManager.Instance.HideUpgradeOptions();
        }
    }
    #region Upgrade Data
    private static readonly Color[] upgradeColors = {
        new Color(255,0,0), // Damage +
        new Color(0,175,0), // Attack speed + 

        new Color(0,175,255), // Move speed +
        new Color(75,75,75), // Defence +
        new Color(150,75,75), // Max health 

        new Color(0,0,225), // Exp +
        new Color(175,175,0) // Gold +
        };
    private static readonly string[] upgradeText = {
        "+Dmg", // Damage +
        "+Atk\nSpd", // Attack speed + 

        "+Mov\nSpd", // Move speed +
        "+Def", // Defence +
        "+Max\nHp", // Max health 

        "+Exp", // Exp +
        "+$$$" // Gold +
        };

    // val defines the upgrade and id defines the pool val chooses from
    private static int ValToID(float val, int id) {
        switch (id) {
            case 0: // 1st upgrade - damage oriented
                switch (val) {
                    case float a when (a <= 50f): // Damage +
                        return 0;
                    case float a when (a <= 100f): // Attack speed +
                        return 1;
                }
                break;
            case 1: // 2nd upgrade - survivability oriented
                switch (val) {
                    case float a when (a <= 33.3f): // Move speed +
                        return 2;
                    case float a when (a <= 66.6f): // Defence +
                        return 3;
                    case float a when (a <= 100f): // Max health +
                        return 4;
                }
                break;
            case 2: // 3rd upgrade - util oriented
                switch (val) {
                    case float a when (a <= 50f): // Exp +
                        return 5;
                    case float a when (a <= 100f): // Gold +
                        return 6;
                }
                break;
        }

        return -1;
    }
    #endregion
    #endregion

    #region Gold
    public void GainGold(int addGold) {
        gold += (int)(addGold * Stats.GetGoldMod());

        // Update UI
    }

    public bool TryPurchace(int cost) {
        if (gold >= cost) {
            gold -= cost;
            return true;
        }
        return false;
    }
    #endregion

    #region Room Interaction
    [Header("Room Interaction")]
    [SerializeField] SpriteRenderer interactionPrompt;
    private Interactable currentInteraction;

    public void InteractionEnter(Interactable interaction) {
        ActivateInteractionPrompt();
        currentInteraction = interaction;
    }
    public void InteractionLeave(Interactable interaction) {
        if (currentInteraction == interaction) {
            currentInteraction = null;
            DisableInteractionPrompt();
        } 
    }

    private void Interact() {
        // Called upon interact key press
        if (currentInteraction != null) {
            currentInteraction.Interact();
        }
    }

    private void ActivateInteractionPrompt() {
        // TODO VFX
        interactionPrompt.enabled = false;
    }
    private void DisableInteractionPrompt() {
        interactionPrompt.enabled = true;
    }
    #endregion
}
