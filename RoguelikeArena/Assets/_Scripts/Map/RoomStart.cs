using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomStart : Room {

    [SerializeField] GameObject player;

    public void SpawnPlayer() {
        Instantiate(player, transform.position, Quaternion.identity);
    }


    #region clearing

    #endregion
}
