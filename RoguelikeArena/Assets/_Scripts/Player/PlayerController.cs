using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, Health {


    #region Referances
    Camera cam;
    Rigidbody2D rb;
    void Awake() {
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody2D>();

        // Set referance in referance manager
        ReferanceManager.player = this;
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
        public Vector2 mousePos;
    }

    private void GatherInput() {
        input = new FrameInput {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical"),
            rollPressed = Input.GetButtonDown("Roll"),
            attackPressed = Input.GetButton("Fire"),
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition)
        };
    }
    #endregion

    #region movement 
    [Header("Movement")]
    [SerializeField] float moveSpeed;

    private Vector2 moveDir;
    private void CalculateMovement() {
        // Calculate direction
        moveDir = new Vector2(input.x, input.y);
        moveDir.Normalize();

        // Calculate modifiers
        var moveMod = moveSpeed;

        // Finalize moveDir
        moveDir *= moveMod;
    }

    private void Move() {
        rb.velocity = moveDir;
    }

    private void Roll() {

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
        if (input.attackPressed && lastAttack + weaponStats.fireRate <= Time.time) {
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
                target.TakeDamage(weaponStats.damage, fireDir*weaponStats.knockback);
            }

            // Make trail
            bulletTrail.MakeTrail(startPos, hitInfo.point);

        } else {
            // Make trail
            bulletTrail.MakeTrail(startPos, startPos + fireDir * maxRange);
        }

    }

    private void GiveWeapon(int id) { // GOING TO ENTIRELY RE-DO weapons, data will be held in external weapon clases and handled within
                weaponStats.fireRate = 0.2f;
                weaponStats.damage = 1f;
                weaponStats.knockback = 10f;
    }
    private void DropWeapon() {
        
    }



    #endregion
}
