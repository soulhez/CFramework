using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RandomMap3DBuilder : MonoBehaviour
{
    public Dictionary<Vector3,MapBlock> mapBlockDic;
    public List<MapBlock> mapBlocks;
    public List<Room> mapRooms;

    public void CreateMap(int seed)
    {
        //��������
        CreateRooms(seed);
        //��ɢ����
        DivideRooms(seed);
        //���ӷ���
        DivideRooms(seed);
        //����ͨ·
        CreateRoads();
    }
    public void CreateRooms(int seed)
    {
        Random randHeight = new Random(seed);
        int height = randHeight.Next(RandomMapData.heightMin, RandomMapData.heightMax);
        Random randWidth = new Random(seed+10);
        int width = randWidth.Next(RandomMapData.widthMin, RandomMapData.widthMax);
        Random randLayer = new Random(seed + 11);
        int layer = randWidth.Next(RandomMapData.layerMin, RandomMapData.layerMax);
        Room room = new Room(width, height, layer);

    }
    public void DivideRooms(int seed)
    {

    }

    public void LinkRooms()
    {

    }
    public void CreateRoads()
    {

    }
}
