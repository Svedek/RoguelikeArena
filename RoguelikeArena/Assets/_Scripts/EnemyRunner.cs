using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunner : MonoBehaviour, Enemy {

    // Base stats
    readonly int gold = 3;
    readonly float exp = 2f;



    #region Referances
    private PlayerController player;
    private Rigidbody2D rb;
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        player = ReferanceManager.player;
    }
    #endregion

    #region Health
    [Header("Health")]
    [SerializeField]private float health;
    [SerializeField] private float kbMod;
    public void TakeDamage(float damage, Vector2 knockback) {
        if ((health -= damage) <= 0) Die();
        rb.AddForce(knockback * kbMod, ForceMode2D.Impulse);
    }
    private void Die() {
        ReferanceManager.enemyManager.EnemyDied(this);
        PlayerManager.GainExperience(exp);
        PlayerManager.GainGold(gold);
        Destroy(gameObject);
    }
    #endregion

    void Update() {
        CalculateMove();
    }
    void FixedUpdate() {
        Move();
    }

    #region Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    private Vector2 moveDir;
    private void CalculateMove() {
        moveDir = (player.transform.position - transform.position).normalized;
        moveDir *= moveSpeed;
    }

    private void Move() {
        rb.AddForce(moveDir, ForceMode2D.Force);
    }
    #endregion

    #region attack
    [Header("Attack")]
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerController play = collision.gameObject.GetComponent<PlayerController>();
        if (play != null) {
            play.TakeDamage(damage, moveDir.normalized * knockback);
        }
    }
    #endregion
}
