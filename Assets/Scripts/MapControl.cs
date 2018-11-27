using UnityEngine;
using Random = System.Random;

public class MapControl : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    enum TileType : byte
    {
        Wall = 0,
        Enemy,
        Key,
        Door
    }

    private readonly Vector2Int RoomCount = new Vector2Int(4, 6),
        RoomSizeX = new Vector2Int(1, 4),
        RoomSizeY = new Vector2Int(1, 4);

    TileType[,] makeMap(int floor)
    {
        TileType[,] map = new TileType[13, 13];
        makeWallMap(map);
        makeObjectMap(map, floor);
        return map;
    }

    private void makeObjectMap(TileType[,] map, int floor)
    {
    }

    private void makeWallMap(TileType[,] map)
    {
        Random random = new Random();
        int rootCount = random.Next(RoomCount.x, RoomCount.y);
        for (int i = 0; i < rootCount; i++)
        {
            
        }
    }
}