using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherEnemy : Enemy {

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
        moveDir = (PlayerController.Instance.transform.position - transform.position).normalized;
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
