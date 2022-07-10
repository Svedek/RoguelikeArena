using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStart : Room {

    [SerializeField] GameObject player;

    public void SpawnPlayer() {
        if (PlayerController.Instance == null) {
            Instantiate(player, transform.position, Quaternion.identity);
        } else {
            PlayerController.Instance.transform.position = transform.position;
        }
    }


    #region clearing

    #endregion
}
