using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] private float damage;
    [SerializeField] private float velocity;
    [SerializeField] private float bulletTime;
    private float endTime;

    public void InitializeBullet() {

    }

    private void Start() {
        GetComponent<Rigidbody2D>().velocity = transform.rotation.eulerAngles.normalized * velocity;
        endTime = Time.time + bulletTime;
    }

    private void Update() {
        if (Time.time >= endTime) {
            DestroyBullet();
        }
    }

    [SerializeField] private int ignoreLayer;
    private readonly static int enemyLayer = 8, terrainLayer = 9, playerLayer = 10;
    protected virtual void OnTriggerEnter2D(Collider2D collision) { // TODO
        switch (ignoreLayer) {
            case 8:
                if (collision.gameObject.layer == 10) {
                    DestroyBullet();
                }
                break;
            case 9:
                break;
            case 10:
                if (collision.gameObject.layer == 10) {
                    DestroyBullet();
                }
                break;
            default:
                break;
        }
    }


    protected virtual void DestroyBullet() {
        Destroy(gameObject);
    }
}
