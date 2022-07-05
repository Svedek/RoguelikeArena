using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBase : MonoBehaviour, Room {
    private readonly float posMod = 60f;

    private Vector2 facing;

    private RoomDoor doorRight;
    private RoomDoor doorUp;
    private RoomDoor doorLeft;
    private RoomDoor doorDown;

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

    #region Generation
    public void InitializeRoom(Vector2 position) {

        // Move to Location
        transform.position = position * posMod;
    }

    [SerializeField] GameObject solidWall, doorWall;
    private readonly float wallDistance = 20f;

    public void CreateWalls() {
        if (doorRight == null) {
            GameObject current = Instantiate(solidWall, transform);
            current.transform.localPosition = Vector2.right * wallDistance;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (doorUp == null) {
            GameObject current = Instantiate(solidWall, transform);
            current.transform.localPosition = Vector2.up * wallDistance;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        }
        if (doorLeft == null) {
            GameObject current = Instantiate(solidWall, transform);
            current.transform.localPosition = Vector2.left * wallDistance;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f));
        }
        if (doorDown == null) {
            GameObject current = Instantiate(solidWall, transform);
            current.transform.localPosition = Vector2.down * wallDistance;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270f));
        }
    }

    [SerializeField] GameObject hallway;
    private readonly float hallwayDistance = 30f;
    // Creates hallway and both associating doorways
    public void CreateHallway(Vector2 dir, Room last) {
        Vector3 angle;

        switch (dir.x, dir.y) {
            case (1, 0): // Right
                angle = new Vector3(0, 0, 0);
                CreateRightDoor();
                last.CreateDoor(2);
                facing = Vector2.right;
                break;
            case (0, 1): // Up
                angle = new Vector3(0, 0, 90f);
                CreateUpDoor();
                last.CreateDoor(3);
                facing = Vector2.up;
                break;
            case (-1, 0): // Left
                angle = new Vector3(0, 0, 180f);
                CreateLeftDoor();
                last.CreateDoor(0);
                facing = Vector2.left;
                break;
            case (0, -1): // Down
                angle = new Vector3(0, 0, 270f);
                CreateDownDoor();
                last.CreateDoor(1);
                facing = Vector2.down;
                break;
            default:
                Debug.LogError("CreateHallwayError");
                angle = Vector3.zero;
                facing = Vector2.zero;
                break;
        }

        GameObject hallwayObject = Instantiate(hallway, transform);
        hallwayObject.transform.localPosition = dir * hallwayDistance;
        hallwayObject.transform.rotation = Quaternion.Euler(angle);
    }

    public void CreateDoor(int dir) {
        switch(dir) {
            case 0:
                CreateRightDoor();
                break;
            case 1:
                CreateUpDoor();
                break;
            case 2:
                CreateLeftDoor();
                break;
            case 3:
                CreateDownDoor();
                break;
            default:
                Debug.LogError("Incorrect usage of CreateDoor");
                break;
        }
    }

    private void CreateRightDoor() {
        GameObject current = Instantiate(doorWall, transform);
        current.transform.localPosition = Vector2.right * wallDistance;
        current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
        doorRight = current.GetComponent<RoomDoor>();
    }

    private void CreateUpDoor() {
        GameObject current = Instantiate(doorWall, transform);
        current.transform.localPosition = Vector2.up * wallDistance;
        current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        doorUp = current.GetComponent<RoomDoor>();
    }
    private void CreateLeftDoor() {
        GameObject current = Instantiate(doorWall, transform);
        current.transform.localPosition = Vector2.left * wallDistance;
        current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f));
        doorLeft = current.GetComponent<RoomDoor>();
    }
    private void CreateDownDoor() {
        GameObject current = Instantiate(doorWall, transform);
        current.transform.localPosition = Vector2.down * wallDistance;
        current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270f));
        doorDown = current.GetComponent<RoomDoor>();
    }
    #endregion

}
