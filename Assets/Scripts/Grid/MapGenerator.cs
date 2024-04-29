#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum TileObjectType
{
    Stickman, Car2Tile, Car3Tile, Cone, Barrier
}
public enum YRotation
{
    R0 = 0, R90 = 90, R180 = 180, R270 = 270
}
public enum Mode
{
    Create, Erase
}
[Serializable]
public struct Vector2XZ
{
    public float X;
    public float Z;
    public Vector2XZ(float x, float z)
    {
        X = x;
        Z = z;
    }
}
public class TileDummy
{
    public Color Color;
    public int X;
    public int Z;
    public TileObjectDummy TileObjectDummy;
    public GameObject TileGameObject;
    public bool IsEmpty;
    public TileDummy(int x, int z, GameObject tileGameObject)
    {
        Color = Color.white;
        IsEmpty = true;
        X = x;
        Z = z;
        TileGameObject = tileGameObject;
    }
}
public class TileObjectDummy
{
    public List<TileDummy> CoveredTiles;
    public GameObject GameObject;
    public TileObjectDummy(List<TileDummy> coveredTiles, GameObject gameObject)
    {
        GameObject = gameObject;
        CoveredTiles = coveredTiles;
    }
}

[Serializable]
public class TileObjectMap
{
    public TileObjectType TileObjectType;
    public MatchColor MatchColor;
    public GameObject ObjectPrefab;
    public int CoveredTileCount;
    [Tooltip("Offset at 0 rotation")]
    public Vector3 Offsett;
    [Tooltip("Covered tiles at 0 rotation")]
    public Vector2XZ[] CoveredTiles;
}

public class MapGenerator : MonoBehaviour
{
    public bool isMapGenerated;
    public Mode Mode;
    [Header("General")]
    [SerializeField][Range(0, 5)] private int width;
    [SerializeField][Range(0, 100)] private int height;
    [SerializeField] private Vector3 origin;

    [Header("Tile")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private float tileSize;

    [Header("Road")]
    [SerializeField] private GameObject straightRoad;
    [SerializeField] private GameObject cornerRoad;
    [SerializeField] private GameObject connectiveRoad;
    [SerializeField] private float roadSize;
    [SerializeField] private int finalRoadCount;

    [Header("Terrain")]
    [SerializeField] private GameObject terrainPrefab;
    [SerializeField] private float terrainYOffset;

    [Header("Exit")]
    [SerializeField] private GameObject exitBarrierPrefab;
    [SerializeField] private float barrierZOffset;

    [SerializeField] private List<TileObjectMap> tileObjectMaps;
    [SerializeField] private MatchColor matchColor;
    [SerializeField] private TileObjectType tileObjectType;
    [SerializeField] private YRotation yRotation;
    private TileDummy[,] grid;
    public void GenerateMap()
    {
        GameObject currentTileParent = GameObject.Find("MG_Environment");

        if (currentTileParent != null)
        {
            Debug.LogWarning("Scene already has a map named \"MG_Environment\". Before creating new map clear current map");
            return;
        }

        isMapGenerated = true;
        GameObject tileParent = new GameObject("MG_Tiles");
        StringBuilder stringBuilder = new StringBuilder();
        grid = new TileDummy[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 position = new Vector3(i * tileSize, 0, j * tileSize) + origin;
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, tileParent.transform);
                stringBuilder.Append(i);
                stringBuilder.Append('_');
                stringBuilder.Append(j);
                tile.name = stringBuilder.ToString();
                stringBuilder.Clear();

                TileDummy tileDummy = new TileDummy(i, j, tile);
                grid[i, j] = tileDummy;
            }
        }

        GenerateStraightRoads(grid);
        GenerateCornerRoads(grid);
        GenerateFinalRoad(grid);
        GenerateTerrain();
        ParentAll();
    }
    public void ClearMap()
    {
        GameObject parent = GameObject.Find("MG_Environment");

        if (parent != null)
        {
            isMapGenerated = false;
            grid = null;
            DestroyImmediate(parent);
        }
        else
        {
            Debug.LogWarning("There is no map inside current scene. First create one.");
        }
    }
    public void Create(int x, int y)
    {
        TileObjectMap map = GetTileObjectMap();

        if (map == null)
        {
            Debug.LogWarning("Specified object not found");
            return;
        }

        List<TileDummy> coveredTiles = GetCoveredTiles(x, y, map, yRotation);

        if (map.CoveredTileCount > 1)
        {
            if (coveredTiles == null || coveredTiles.Count <= 0)
            {
                Debug.LogWarning("No space avaliable");
            }
            else
            {
                GenerateTileObject(x, y, coveredTiles, map);
            }
        }
        else
        {
            GenerateTileObject(x, y, coveredTiles, map);
        }
    }
    public void Erase(int x, int z)
    {
        TileDummy tileDummy = GetTile(x, z);

        if (tileDummy.TileObjectDummy == null)
        {
            Debug.LogWarning("Selected tile does not have object");
        }
        else
        {
            TileObjectDummy tileObjectDummy = tileDummy.TileObjectDummy;

            foreach (TileDummy tile in tileObjectDummy.CoveredTiles)
            {
                tile.TileObjectDummy = null;
                tile.Color = Color.white;
                tile.IsEmpty = true;
            }

            DestroyImmediate(tileObjectDummy.GameObject);
        }
    }
    public TileDummy GetTile(int x, int z)
    {
        if (x < 0 || x >= width || z < 0 || z >= height) return null;
        if (grid == null) return null;
        return grid[x, z];
    }
    private void GenerateStraightRoads(TileDummy[,] grid)
    {
        GameObject roadParent = new GameObject("MG_Roads");

        int constant = 0;
        Vector3 eulerRotation = new Vector3(0, 90, 0);

        for (int i = 0; i < width; i++)
        {
            Vector3 tilePosition = grid[i, constant].TileGameObject.transform.position;
            Vector3 roadPosition = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z - (tileSize * 0.5f + roadSize * 0.5f));
            Instantiate(straightRoad, roadPosition, Quaternion.Euler(eulerRotation), roadParent.transform);
        }

        constant = height - 1;

        for (int i = 0; i < width; i++)
        {
            Vector3 tilePosition = grid[i, constant].TileGameObject.transform.position;
            Vector3 roadPosition = new Vector3(tilePosition.x, tilePosition.y, tilePosition.z + (tileSize * 0.5f + roadSize * 0.5f));
            Instantiate(straightRoad, roadPosition, Quaternion.Euler(eulerRotation), roadParent.transform);
        }

        eulerRotation = Vector3.zero;
        constant = 0;

        for (int i = 0; i < height; i++)
        {
            Vector3 tilePosition = grid[constant, i].TileGameObject.transform.position;
            Vector3 roadPosition = new Vector3(tilePosition.x - (tileSize * 0.5f + roadSize * 0.5f), tilePosition.y, tilePosition.z);
            Instantiate(straightRoad, roadPosition, Quaternion.Euler(eulerRotation), roadParent.transform);
        }

        constant = width - 1;

        for (int i = 0; i < height; i++)
        {
            Vector3 tilePosition = grid[constant, i].TileGameObject.transform.position;
            Vector3 roadPosition = new Vector3(tilePosition.x + (tileSize * 0.5f + roadSize * 0.5f), tilePosition.y, tilePosition.z);
            Instantiate(straightRoad, roadPosition, Quaternion.Euler(eulerRotation), roadParent.transform);
        }
    }
    private void GenerateCornerRoads(TileDummy[,] grid)
    {
        GameObject roadParent = GameObject.Find("MG_Roads");
        float offset = tileSize * 0.5f + roadSize * 0.5f;
        Vector3 eulerRotation = new Vector3(0, 90, 0);

        Vector3 rightTopTile = grid[width - 1, height - 1].TileGameObject.transform.position;
        Vector3 rightTopRoad = new Vector3(rightTopTile.x + (offset), rightTopTile.y, rightTopTile.z + offset);
        Instantiate(cornerRoad, rightTopRoad, Quaternion.Euler(eulerRotation), roadParent.transform);

        eulerRotation = new Vector3(0, 180, 0);
        Vector3 rightBottomTile = grid[width - 1, 0].TileGameObject.transform.position;
        Vector3 rightBottomRoad = new Vector3(rightBottomTile.x + (offset), rightBottomTile.y, rightBottomTile.z - offset);
        Instantiate(cornerRoad, rightBottomRoad, Quaternion.Euler(eulerRotation), roadParent.transform);

        eulerRotation = new Vector3(0, 270, 0);
        Vector3 leftBottomTile = grid[0, 0].TileGameObject.transform.position;
        Vector3 leftBottomRoad = new Vector3(leftBottomTile.x - (offset), leftBottomTile.y, leftBottomTile.z - offset);
        Instantiate(cornerRoad, leftBottomRoad, Quaternion.Euler(eulerRotation), roadParent.transform);

        eulerRotation = new Vector3(0, 0, 0);
        Vector3 lefttTopTile = grid[0, height - 1].TileGameObject.transform.position;
        Vector3 lefttTopRoad = new Vector3(lefttTopTile.x - (offset), lefttTopTile.y, lefttTopTile.z + offset);
        Instantiate(connectiveRoad, lefttTopRoad, Quaternion.Euler(eulerRotation), roadParent.transform);
    }
    private void GenerateFinalRoad(TileDummy[,] grid)
    {
        GameObject roadParent = GameObject.Find("MG_Roads");
        float offset = tileSize * 0.5f + roadSize * 0.5f;
        Vector3 eulerRotation = new Vector3(0, 0, 0);
        Vector3 lefttTopTile = grid[0, height - 1].TileGameObject.transform.position;
        Vector3 lefttTopRoad = new Vector3(lefttTopTile.x - (offset), lefttTopTile.y, lefttTopTile.z + offset);

        for (int i = 1; i <= finalRoadCount; i++)
        {
            Vector3 nextRoadPosition = new Vector3(lefttTopRoad.x, lefttTopRoad.y, lefttTopRoad.z + i * roadSize);
            Instantiate(straightRoad, nextRoadPosition, Quaternion.Euler(eulerRotation), roadParent.transform);
        }

        Vector3 exitBarrierPosition = lefttTopTile + new Vector3(0, 0, barrierZOffset);
        Instantiate(exitBarrierPrefab, exitBarrierPosition, exitBarrierPrefab.transform.rotation, roadParent.transform);
    }
    private void GenerateTerrain()
    {
        GameObject terrainParent = new GameObject("MG_Terrain");
        Vector3 position = new Vector3(origin.x, origin.y + terrainYOffset, origin.z);
        Instantiate(terrainPrefab, position, Quaternion.identity, terrainParent.transform);
    }
    private void ParentAll()
    {
        GameObject roadParent = GameObject.Find("MG_Roads");
        GameObject tileParent = GameObject.Find("MG_Tiles");
        GameObject terrainParent = GameObject.Find("MG_Terrain");
        GameObject baseParent = new GameObject("MG_Environment");
        roadParent.transform.parent = baseParent.transform;
        tileParent.transform.parent = baseParent.transform;
        terrainParent.transform.parent = baseParent.transform;
    }
    private TileObjectMap GetTileObjectMap()
    {
        foreach (TileObjectMap tileObjectMap in tileObjectMaps)
        {
            if (tileObjectMap.MatchColor == matchColor && tileObjectMap.TileObjectType == tileObjectType)
            {
                return tileObjectMap;
            }
        }
        return null;
    }
    private List<TileDummy> GetCoveredTiles(int x, int z, TileObjectMap map, YRotation rotation)
    {
        List<TileDummy> tiles = new List<TileDummy>();

        foreach (Vector2XZ coveredTiles in map.CoveredTiles)
        {
            TileDummy tile;
            Vector2XZ relativeVector = GetRelativeVector(coveredTiles.X, coveredTiles.Z, rotation);
            float tileX = x + relativeVector.X;
            float tileZ = z + relativeVector.Z;
            tile = GetTile((int)tileX, (int)tileZ);
            if (tile == null) return null;
            tiles.Add(tile);
        }

        return tiles;
    }
    private Vector2XZ GetRelativeVector(float x, float z, YRotation rotation)
    {
        float X = 0;
        float Z = 0;

        switch (rotation)
        {
            case YRotation.R0:
                X = x;
                Z = z;
                break;
            case YRotation.R90:
                X = z;
                Z = -x;
                break;
            case YRotation.R180:
                X = -x;
                Z = -z;
                break;
            case YRotation.R270:
                X = -z;
                Z = x;
                break;
        }

        Vector2XZ result = new Vector2XZ(X, Z);
        return result;
    }
    private void GenerateTileObject(int x, int z, List<TileDummy> coveredTiles, TileObjectMap map)
    {
        coveredTiles.Add(GetTile(x, z));

        if (!AllEmpty(coveredTiles))
        {
            Debug.LogWarning("Covered tiles already has an object. Before creating new one try to delete existing object");
            return;
        }

        Vector2XZ relativeOffsetV2 = GetRelativeVector(map.Offsett.x, map.Offsett.z, yRotation);
        Vector3 relativeOffsetV3 = new Vector3(relativeOffsetV2.X, map.Offsett.y, relativeOffsetV2.Z);
        Vector3 position = grid[x, z].TileGameObject.transform.position + relativeOffsetV3;
        Vector3 rotation = new Vector3(0, (int)yRotation, 0);
        GameObject obj = Instantiate(map.ObjectPrefab, position, Quaternion.Euler(rotation));
        TileObjectDummy tileObjectDummy = new TileObjectDummy(coveredTiles, obj);

        foreach (TileDummy tileDummy in coveredTiles)
        {
            tileDummy.TileObjectDummy = tileObjectDummy;
            Color color = ConvertToColor(matchColor);
            tileDummy.Color = color;
            tileDummy.IsEmpty = false;
        }
    }
    private bool AllEmpty(List<TileDummy> tiles)
    {
        foreach (TileDummy tileDummy in tiles)
        {
            if (tileDummy.IsEmpty == false) return false;
        }
        return true;
    }
    private Color ConvertToColor(MatchColor matchColor)
    {
        switch (matchColor)
        {
            case MatchColor.Black:
                return Color.black;
            case MatchColor.Red:
                return Color.red;
            case MatchColor.Blue:
                return Color.blue;
            case MatchColor.Orange:
                return new Color32(255, 178, 102, 255);
            case MatchColor.Purple:
                return new Color32(178, 102, 255, 255);
            case MatchColor.Pink:
                return new Color32(255, 170, 190, 255);
            case MatchColor.Green:
                return Color.green;
            case MatchColor.Yellow:
                return Color.yellow;
        }
        return Color.gray;
    }
}
#endif