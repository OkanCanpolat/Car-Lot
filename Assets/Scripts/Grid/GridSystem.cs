using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public struct TileContainer
{
    public List<Tile> Rows;
}
public class GridSystem : MonoBehaviour
{
    public TileStateMachine StateMachine;
    [SerializeField] private float tileSize;
    [SerializeField] private List<TileContainer> columns;
    [SerializeField] private bool automaticTileAdjusment;
    [SerializeField] private int width;
    [SerializeField] private int height;
    
    #region Getter/Setter
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public float TileSize { get => tileSize; set => tileSize = value; }
    public bool AutomaticTileAdjusment { get => automaticTileAdjusment; set => automaticTileAdjusment = value; }
    #endregion

    [Inject]
    public void Construct(TileStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    private void Awake()
    {
        if (automaticTileAdjusment)
        {
            AdjustTilesByName();
        }

        else
        {
            width = columns.Count;
            height = columns[0].Rows.Count;
        }

        SetTileCoordinates();
    }
    public Tile GetTile(int x, int y)
    {
        if (!IsInsideGrid(x, y)) return null;

        return columns[x].Rows[y];
    }
    public Tile GetTile(Vector3 position)
    {
        foreach(TileContainer tileContainer in columns)
        {
            foreach(Tile tile in tileContainer.Rows)
            {
                float minX = tile.transform.position.x - tileSize * 0.5f;
                float maxX = tile.transform.position.x + tileSize * 0.5f;
                float minZ = tile.transform.position.z - tileSize * 0.5f;
                float maxZ = tile.transform.position.z + tileSize * 0.5f;

                if(position.x < maxX && position.x > minX && position.z > minZ && position.z < maxZ)
                {
                    return tile;
                }
            }
        }
        return null;
    }
    public List<Tile> GetTile(Bounds bounds)
    {
        float minX = bounds.min.x;
        float maxX = bounds.max.x;
        float minZ = bounds.min.z;
        float maxZ = bounds.max.z;

        List<float> xCoordinates = new List<float>();
        float xCounter = minX;

        while(xCounter < maxX)
        {
            xCoordinates.Add(xCounter);
            xCounter += tileSize;
        }

        List<float> zCoordinates = new List<float>();
        float zCounter = minZ;

        while (zCounter < maxZ)
        {
            zCoordinates.Add(zCounter);
            zCounter += tileSize;
        }

        List<Tile> boundTiles = new List<Tile>();

        if(xCoordinates.Count > 0 && zCoordinates.Count > 0)
        {
            foreach(float xCoord in xCoordinates)
            {
                foreach(float zCoord in zCoordinates)
                {
                    Tile tile = GetTile(new Vector3(xCoord, 0, zCoord));
                    boundTiles.Add(tile);
                }
            }

            return boundTiles;
        }

        else
        {
            return null;
        }
    }
    public bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
    private void SetTileCoordinates()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Tile tile = GetTile(i, j);
                tile.X = i;
                tile.Y = j;
            }
        }
    }
    private void AdjustTilesByName()
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        char splitChar = '_';

        columns = new List<TileContainer>();

        for (int i = 0; i < width; i++)
        {
            TileContainer container = new TileContainer();
            container.Rows = new List<Tile>();
            columns.Add(container);

            for (int j = 0; j < height; j++)
            {
                columns[i].Rows.Add(null);
            }
        }

        foreach (Tile tile in tiles)
        {
            string tileName = tile.name;
            string[] splitedName = tileName.Split(splitChar);
            int x = int.Parse(splitedName[0]);
            int y = int.Parse(splitedName[1]);
            columns[x].Rows[y] = tile;
        }
    }
}
