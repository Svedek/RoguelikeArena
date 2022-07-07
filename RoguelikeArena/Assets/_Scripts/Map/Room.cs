using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    private readonly float posMod = 60f;

    public void InitializeRoom(Vector2 position) {
        // Move to Location
        transform.position = position * posMod;
    }

    #region Generation
    public void CreateHallway(Vector2 dir, Room last) {

    }
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
    public void CreateDoor(int dir) {

    }
    #endregion
    #region f
    #endregion
}
