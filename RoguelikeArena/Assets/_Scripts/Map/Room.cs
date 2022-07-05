using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Room {
    public void InitializeRoom(Vector2 position);
    #region Generation
    public void CreateHallway(Vector2 dir, Room last);
    public void CreateWalls();
    public void CreateDoor(int dir);
    #endregion
    #region f
    #endregion
}
