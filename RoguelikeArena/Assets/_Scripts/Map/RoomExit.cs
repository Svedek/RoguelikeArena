using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExit : MonoBehaviour, Room {
    private readonly float posMod = 60f;

    private RoomDoor doorRight;
    private RoomDoor doorUp;
    private RoomDoor doorLeft;
    private RoomDoor doorDown;


    public void InitializeRoom(Vector2 position) {

        // Move to Location
        transform.position = position * posMod;
    }

    #region Generation
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
                break;
            case (0, 1): // Up
                angle = new Vector3(0, 0, 90f);
                CreateUpDoor();
                last.CreateDoor(3);
                break;
            case (-1, 0): // Left
                angle = new Vector3(0, 0, 180f);
                CreateLeftDoor();
                last.CreateDoor(0);
                break;
            case (0, -1): // Down
                angle = new Vector3(0, 0, 270f);
                CreateDownDoor();
                last.CreateDoor(1);
                break;
            default:
                Debug.LogError("CreateHallwayError");
                angle = Vector3.zero;
                break;
        }

        GameObject hallwayObject = Instantiate(hallway, transform);
        hallwayObject.transform.localPosition = dir * hallwayDistance;
        hallwayObject.transform.rotation = Quaternion.Euler(angle);
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

    #region clearing

    #endregion
}
