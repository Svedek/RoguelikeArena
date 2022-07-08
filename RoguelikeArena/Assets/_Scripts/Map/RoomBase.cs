using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBase : Room {
    #region Room function + minimap

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null) {
            // Lock doors (and ensure player is in room)
            CloseDoors();

            // Start room (spawn enemies)
            var scaling = Random.Range(0, 7);
            ReferanceManager.enemyManager.SpawnEnemies(scaling, this, facing);

            // Add room to minimap

            // Remove collider
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void RoomCleared() {
        OpenDoors();
    }

    private void CloseDoors() {
        if (doorRight != null) doorRight.CloseDoor();
        if (doorUp != null) doorUp.CloseDoor();
        if (doorLeft != null) doorLeft.CloseDoor();
        if (doorDown != null) doorDown.CloseDoor();
    }
    private void OpenDoors() {
        if (doorRight != null) doorRight.OpenDoor();
        if (doorUp != null) doorUp.OpenDoor();
        if (doorLeft != null) doorLeft.OpenDoor();
        if (doorDown != null) doorDown.OpenDoor();
    }

    public void EnemiesCleared() {
        OpenDoors();
    }
    #endregion


}
