using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    
    // For optimization, remove unnessesary fields
    private Position[,] arena; // ?
    private Dictionary<Position, Room> map; // ?
    private Position startPos; // ?
    private int roomCount; // ?

    public void Initialize(int floor, float scaling) {
        InitiateArena(floor, scaling);
    }

    private readonly float startVarriance = 0.6f; // Determines % away from center that start can spawn
    private readonly int maxRooms = 16;

    public class Position {
        public int x;
        public int y;
        public Position last;

        public Position(int x, int y) {
            this.x = x;
            this.y = y;
            this.last = null;
        }
        public Position(int x, int y, Position last) {
            this.x = x;
            this.y = y;
            this.last = last;
        }
    }

    [SerializeField] private GameObject roomBase, roomShop, roomExit, roomStart;
    // depth - floor depth
    // scaling - difficulty scaling
    private void InitiateArena(int depth, float scaling) {
        // Calculate bounds
        var rooms = 5 + (int)(depth + Mathf.Sqrt(scaling));
        roomCount = rooms >= maxRooms ? maxRooms : rooms;

        var bounds = 1 +  2 * (int) Mathf.Sqrt(roomCount);

        // Find Start room
        int halfRoom = bounds / 2;
        var startMod = halfRoom * startVarriance;
        Position startPosition = new Position(halfRoom, halfRoom);
        startPosition.x += (int)(Random.Range(-1f,1f) * startMod);
        startPosition.y += (int)(Random.Range(-1f, 1f) * startMod);

        startPos = startPosition;

        // Create map and ensure enough endpoints exist (else re-create)
        //Position[,] arena;
        ArrayList endPositions;
        do {
            arena = new Position[bounds, bounds];
            // print("roomCount: " + roomCount);
            // print("bounds: " + bounds);
            // print("startPos x: " + startPos.x);
            // print("startPos y: " + startPos.y);

            endPositions = new ArrayList(bounds ^ 2);
            ArrayList toPlace = new ArrayList(bounds ^ 2) {
                startPos
            };


            for (int i = 0; i < roomCount; i++) {
                int index = Random.Range(0, toPlace.Count);
                Position current = (Position) toPlace.ToArray()[index];
                toPlace.RemoveAt(index);
                // print("i: " + i + ", toPlaceCount: " + toPlace.Count + ", index: " + index + ", pos = " + current.x + "," + current.y);

                if (arena[current.x,current.y] == null) {
                    arena[current.x, current.y] = current;
                    endPositions.Add(current);
                    endPositions.Remove(current.last);

                    for (int j = 0; j < 4; j++) {
                        Position offset = j switch {
                            0 => new Position(current.x + 1, current.y, current),
                            1 => new Position(current.x, current.y + 1, current),
                            2 => new Position(current.x - 1, current.y, current),
                            3 => new Position(current.x, current.y - 1, current),
                            // not possible
                            _ => new Position(-1, -1, null)
                        };

                        if ((offset.x >= 0 && offset.x < bounds) && // x in bounds
                            (offset.y >= 0 && offset.y < bounds) && // y in bounds
                            (arena[offset.x,offset.y] == null) // position not already taken
                            ) {
                            toPlace.Add(offset);
                        }
                    }
                } else {
                    i--;
                }
            } 
        } while (endPositions.Count < 2);


        // Find special rooms
        // Exit Room
        int temp = Random.Range(0, endPositions.Count);
        // print("Exit index: " + temp);
        Position exitPos = (Position)endPositions.ToArray()[temp];
        endPositions.RemoveAt(temp); // remove so no 2 spec rooms are assigned the same position

        // Shop Room
        temp = Random.Range(0, endPositions.Count); // BUG SHOP AND EXIT HAVE SAME POSITION
        // print("Shop index: " + temp);
        Position shopPos = (Position)endPositions.ToArray()[temp];
        endPositions.RemoveAt(temp);

        // Add special rooms back for generation
        endPositions.Add(exitPos);
        endPositions.Add(shopPos);


        // Create base rooms

        // Dictionary<Position, Room> map = new Dictionary<Position, Room>();
        map = new Dictionary<Position, Room>();

        var endPositionArray = endPositions.ToArray();

        // print("endPositionArray len: " + endPositionArray.Length);
        for (int i = 0; i < endPositionArray.Length; i++) {
            Position current = (Position) endPositionArray[i];
            while (current != null && !map.ContainsKey(current)) {
                GameObject currentRoomObject;
                Room currentRoom;
                // Special rooms
                if (current == exitPos) {
                    currentRoomObject = Instantiate(roomExit, transform);
                    currentRoom = currentRoomObject.AddComponent<RoomExit>();
                } else if (current == shopPos) {
                    currentRoomObject = Instantiate(roomShop, transform);
                    currentRoom = currentRoomObject.AddComponent<RoomShop>();
                } else if (current == startPos) {
                    currentRoomObject = Instantiate(roomStart, transform);
                    currentRoom = currentRoomObject.AddComponent<RoomStart>();
                } else {
                    currentRoomObject = Instantiate(roomBase, transform);
                    currentRoom = currentRoomObject.AddComponent<RoomBase>();
                }

                currentRoom.InitializeRoom(new Vector2(current.x, current.y));

                map.Add(current, currentRoom);

                current = current.last;
            }
        }

        // Create Hallways and Doors
        var positionCollection = map.Keys;
        foreach (Position current in positionCollection) {
            // Make hallway and doorWall
            if (current.last != null) {
                Vector2 dir = new Vector2(current.last.x - current.x, current.last.y - current.y);

                Room temp1 = map[current.last];
                if (temp1 == null) Debug.LogError("should not happen");

                map[current].CreateHallway(dir, map[current.last]);
            }
        }

        // Create Walls
        var roomCollection = map.Values;
        foreach (Room current in roomCollection) {
            current.CreateWalls();
        }

        // Construct minimap??

        // Finalizing / Create player
        RoomStart start = (RoomStart) map[startPos];
        start.SpawnPlayer();
    }


    #region Level Util
    [Header("Level Util")]
    public GameObject player;
    public GameObject solidWall, doorWall, hallway;
    #endregion
}
