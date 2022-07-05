using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoor : MonoBehaviour {

    [SerializeField] private GameObject door;


    public void OpenDoor() {
        door.SetActive(false);
    }
    public void CloseDoor() {
        door.SetActive(true);
    }
}
