using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Health {

    public static PlayerController Instance;

    #region Referances
    [Header("Referances")]
    [SerializeField] private Transform weaponParent;
    private Camera cam;
    protected override void Awake() {
        base.Awake();

        // Set referance in referance manager
        if (Instance != null) {
            Destroy(gameObject);
        }
        Instance = this;

        // Get refferances
        cam = GetComponentInChildren<Camera>();

        // Initialize Experience
        Initialize();
    }
    #endregion

    #region Player Stats
    private static float[] increaseAmmount =  {
            1.25f, // damageMod
            0.8f, // attackSpeedMod

            1.2f, // moveMod
            1.5f, // defence
            1.5f, // maxHealth

            1.35f, // expMod
            1.5f, // goldMod
        };

    public static float[] stats { get; } = {
            1f, // damageMod
            1f, // attackSpeedMod

            1f, // moveMod
            1f, // defense
            5f, // maxHealth

            1f, // expMod
            1f, // goldMod
        };

    public enum StatID {
        damage,
        attackSpeed,

        moveSpeed,
        defense,
        maxHealth,

        expMod,
        goldMod,
    }

    public static void ApplyUpgrade(StatID id) {
        stats[(int)id] *= increaseAmmount[(int)id];
    }

    #endregion

    #region Health
    public override bool TakeDamage(float damage, Vector2 kbDir) {
        if ((health -= damage) <= 0) Die();
        // TODO TAKE KNOCKBACK

        // Update UI
        UIManager.Instance.UpdateHealth(health, stats[(int) StatID.maxHealth]);


        return true;
    }
    protected override void Die() {
        // TODO Perish
        print("Player Died");
    }
    #endregion

    private void Start() {
        ItemManager.Instance.GiveStartWeapon();
    }

    void Update() {
        GatherInput();

        CalculateMovement();

        CalculateActions(); // Input not used in movement/attack gets processessed here

        HandleWeapon(); // Attacks and otherwise handles weapon movement

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
            interactPressed = Input.GetButtonDown("Interact"),
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
        var moveMod = baseSpeed * stats[(int)StatID.moveSpeed];
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

    #region Actions
    private void CalculateActions() {
        // TODO Move roll Into here

        if (input.upgradeSelected != -1 && upgradePoints > 0) {
            HandleUpgrade();
        }

        if (input.interactPressed) {
            Interact();
        }

    }

    #endregion

    #region Attack and Weapon
    // [Header("Attack")]
    public float WeaponDamage {
        get {
            return weapon.Damage * stats[(int)StatID.damage];
        }
    }
    private Weapon weapon;

    private float lastAttack;
    private void HandleWeapon() {
        // Make weapon follow cursor
        weaponParent.rotation = Quaternion.LookRotation(Vector3.forward, (Vector3)input.mousePos - weaponParent.position);

        // Handle shooting
        if (input.attackPressed && lastAttack + (weapon.attackSpeed * stats[(int)StatID.attackSpeed]) <= Time.time) {

            // Handle bullet
            weapon.Shoot();
            lastAttack = Time.time;

            // TODO Handle player knockback
        }
    }

    private const float gunPosOffset = 1f;
    public void GiveGun(GameObject gun) {
        GameObject gunObject = Instantiate(gun, weaponParent);
        gunObject.transform.localPosition += Vector3.up * gunPosOffset;
        if (weapon != null) {
            Destroy(weapon.gameObject);
        }
        weapon = gunObject.GetComponent<Weapon>();
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
    private StatID[] ids;

    public void GainExperience(float experience) {
        Debug.Log("cXp: " + currentExp + " XpG: " + experience + " RXp: " + requiredExp);
        currentExp += experience * stats[(int)StatID.expMod];
        while (currentExp >= requiredExp) {
            // Handle Experience
            currentExp -= requiredExp;
            LevelUp();

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
        ids = new StatID[3];
        Color[] colors = new Color[3];
        string[] text = new string[3];
        if (!upgradesActive) {
            ids = RandID();
            for (int i = 0; i < 3; i++) {
                //ids[i] = RandID(i);
                colors[i] = upgradeColors[(int)ids[i]];
                text[i] = upgradeText[(int)ids[i]];
            }


            UIManager.Instance.SetUpgradeOptions(colors, text);

            upgradesActive = true;
        }
    }

    private void HandleUpgrade() {
        var id = ids[input.upgradeSelected];
        ApplyUpgrade(id);
        upgradesActive = false;

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
    private static StatID RandID(int id) {
        int val = Random.Range(0, 100); // 0-99
        switch (id) {
            case 0: // 1st upgrade - damage oriented
                switch (val) {
                    case int a when (a < 50): // Damage +
                        return StatID.damage;
                    case int a when (a < 100): // Attack speed +
                        return StatID.attackSpeed;
                }
                break;
            case 1: // 2nd upgrade - survivability oriented
                switch (val) {
                    case int a when (a < 33): // Move speed +
                        return StatID.moveSpeed;
                    case int a when (a < 66): // Defence +
                        return StatID.defense;
                    case int a when (a < 100): // Max health +
                        return StatID.maxHealth;
                }
                break;
            case 2: // 3rd upgrade - util oriented
                switch (val) {
                    case int a when (a < 50): // Exp +
                        return StatID.expMod;
                    case int a when (a < 100): // Gold +
                        return StatID.goldMod;
                }
                break;
        }

        Debug.LogError("Misuse of ValToId: " + id + "   " + val);
        return (StatID)(-1);
    }

    private static StatID[] RandID() {
        StatID[] ret = new StatID[3];

        for (int i = 0; i < ret.Length; i++) {
            int val = Random.Range(0, 100); // 0-99
            switch (val) {
                case int a when (a < 20): // Damage +
                    ret[i] = StatID.damage;
                    break;
                case int a when (a < 35): // Attack speed +
                    ret[i] = StatID.attackSpeed;
                    break;
                case int a when (a < 50): // Move speed +
                    ret[i] = StatID.moveSpeed;
                    break;
                case int a when (a < 65): // Defence +
                    ret[i] = StatID.defense;
                    break;
                case int a when (a < 80): // Max health +
                    ret[i] = StatID.maxHealth;
                    break;
                case int a when (a < 90): // Exp +
                    ret[i] = StatID.expMod;
                    break;
                case int a when (a < 100): // Gold +
                    ret[i] = StatID.goldMod;
                    break;
            }

            // If repeat, re-pick
            for (int j = i-1; j >= 0; j--) {
                if (ret[i] == ret[j]) {
                    --i;
                    break;
                }
            }
        }

        return ret;
    }
    #endregion
    #endregion

    #region Gold
    public void GainGold(int addGold) {
        gold += (int)(addGold * stats[(int)StatID.goldMod]);

        // Update UI
        UIManager.Instance.UpdateGold(gold);
    }

    public bool TryPurchace(int cost) {
        if (gold >= cost) {
            gold -= cost;
            UIManager.Instance.UpdateGold(gold);
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
        interactionPrompt.enabled = true;
    }
    private void DisableInteractionPrompt() {
        interactionPrompt.enabled = false;
    }
    #endregion

    #region Shop Interaction
    public void UsePotion(int potency) {
        var hpGain = (potency / 100f) * stats[(int)StatID.maxHealth];
        health = health + hpGain > stats[(int)StatID.maxHealth] ? stats[(int)StatID.maxHealth] : health + hpGain;
        UIManager.Instance.UpdateHealth(health, stats[(int)StatID.maxHealth]);
    }
    #endregion
}
