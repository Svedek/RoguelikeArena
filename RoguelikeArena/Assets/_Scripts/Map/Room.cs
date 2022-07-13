using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    private readonly float posMod = 60f;

    protected RoomDoor doorRight;
    protected RoomDoor doorUp;
    protected RoomDoor doorLeft;
    protected RoomDoor doorDown;

    protected Vector2 facing;

    protected Level level;
    private void Awake() {
        level = GetComponentInParent<Level>();
    }

    public void InitializeRoom(Vector2 position) {
        // Move to Location
        transform.position = position * posMod;
    }

    #region Generation
    private readonly float wallDistance = 20f;
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

        GameObject hallwayObject = Instantiate(level.hallway, transform);
        hallwayObject.transform.localPosition = dir * hallwayDistance;
        hallwayObject.transform.rotation = Quaternion.Euler(angle);
    }
    public void CreateWalls() {
        if (doorRight == null) {
            GameObject current = Instantiate(level.solidWall, transform);
            current.transform.localPosition = Vector2.right * wallDistance;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (doorUp == null) {
            GameObject current = Instantiate(level.solidWall, transform);
            current.transform.localPosition = Vector2.up * wallDistance;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        }
        if (doorLeft == null) {
            GameObject current = Instantiate(level.solidWall, transform);
            current.transform.localPosition = Vector2.left * wallDistance;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f));
        }
        if (doorDown == null) {
            GameObject current = Instantiate(level.solidWall, transform);
            current.transform.localPosition = Vector2.down * wallDistance;
            current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270f));
        }
    }
    public void CreateDoor(int dir) {
        switch (dir) {
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
        GameObject current = Instantiate(level.doorWall, transform);
        current.transform.localPosition = Vector2.right * wallDistance;
        current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0f));
        doorRight = current.GetComponent<RoomDoor>();
    }

    private void CreateUpDoor() {
        GameObject current = Instantiate(level.doorWall, transform);
        current.transform.localPosition = Vector2.up * wallDistance;
        current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f));
        doorUp = current.GetComponent<RoomDoor>();
    }
    private void CreateLeftDoor() {
        GameObject current = Instantiate(level.doorWall, transform);
        current.transform.localPosition = Vector2.left * wallDistance;
        current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f));
        doorLeft = current.GetComponent<RoomDoor>();
    }
    private void CreateDownDoor() {
        GameObject current = Instantiate(level.doorWall, transform);
        current.transform.localPosition = Vector2.down * wallDistance;
        current.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270f));
        doorDown = current.GetComponent<RoomDoor>();
    }
    #endregion

    #region Room Specific

    #endregion
}
