using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy {

    void Update() {
        CalculateMove();
        CalculateAttack();
    }
    void FixedUpdate() {
        Move();
    }

    #region Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float preferedDistance;
    [SerializeField] private float preferedDistanceTolerance;
    [SerializeField] private float preferedDistanceSlowDistance; // Greater than preferedDistanceTolerance
    [SerializeField] private float sideMovementWeight; // TODO multiplied by -1 to switch direction (on colision)
    private Vector2 moveDir;
    private float distanceToPlayer;
    private void CalculateMove() {
        // Calculate vector in line with player
        Vector2 playerVector = PlayerController.Instance.transform.position - transform.position;
        var normPV = playerVector.normalized; // nessesary for 
        distanceToPlayer = playerVector.magnitude;
        var dist = distanceToPlayer - preferedDistance;

        if (Mathf.Abs(dist) <= preferedDistanceTolerance) {
            playerVector = Vector2.zero;
        } else {
            playerVector *= dist; // Potentially changes the direction of playerVector
            playerVector.Normalize();
            if (Mathf.Abs(dist) < preferedDistanceSlowDistance) {
                playerVector *= Mathf.Abs(dist) / preferedDistanceSlowDistance;
            }
        }

        // Calculate vector perpendicular to previous vector
        Vector2 sideVector = new Vector2(-normPV.y, normPV.x); // points clockwise, perpendicular to playerVector
        sideVector *= sideMovementWeight;


        // Combine vectors
        moveDir = playerVector + sideVector;
        moveDir.Normalize();
        moveDir *= moveSpeed;

    }

    private void Move() {
        rb.AddForce(moveDir, ForceMode2D.Force);
    }

    private static readonly int enemyLayer = 8, terrainLayer = 9, playerLayer = 10;
    private void OnCollisionEnter2D(Collision2D collision) {
        var colLayer = collision.gameObject.layer;
        if (colLayer == enemyLayer || colLayer == terrainLayer || colLayer == playerLayer) {
            sideMovementWeight *= -1;
        }
    }
    #endregion

    #region attack
    [Header("Attack")]
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackRange;

    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpawnOffset;
    private float nextAttack;
    
    private void CalculateAttack() {
        if (distanceToPlayer <= attackRange) {
            if (nextAttack <= Time.time) {
                Shoot();
                nextAttack = Time.time + attackDelay;
            }
        }
    }

    
    private void Shoot() {
        Vector3 playerVector = PlayerController.Instance.transform.position - transform.position;
        Quaternion bulletRotation = Quaternion.LookRotation(Vector3.forward, playerVector);

        Instantiate(bullet, transform.position + playerVector.normalized * bulletSpawnOffset, bulletRotation, MiscManager.Instance.BulletParent);
        //EnemyBullet shot = Instantiate(bullet, transform.position + playerVector.normalized * bulletSpawnOffset, bulletRotation, MiscManager.Instance.BulletParent).GetComponent<EnemyBullet>();
    }

    #endregion
}
