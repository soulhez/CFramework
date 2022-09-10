using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock 
{
    public Vector3 pos;
    public Transform trans;
}
public class Room
{
    
    public Vector3 centerPos;
    public List<Room> linkRooms;
    public int width;
    public int height;
    public int layer;
    public List<Vector3> bottomCorners;
    public List<Vector3> upCorners;
    public Room(int width, int height, int layer)
    {
        this.width = width;
        this.height = height;
        this.layer = layer;
    }
    public void InitRoomData()
    {

    }
}
public class RoomFloor:MapBlock
{
    public bool isBorder=false;

}
public class Road
{
    public List<Room> linkRooms;
    public List<MapBlock> mapBlocks;
}

