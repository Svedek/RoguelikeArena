using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStart : Room {

    public void SpawnPlayer() {
        if (PlayerController.Instance == null) {
            Instantiate(level.player, transform.position, Quaternion.identity);
        } else {
            PlayerController.Instance.transform.position = transform.position;
        }
    }


    #region clearing

    #endregion
}
