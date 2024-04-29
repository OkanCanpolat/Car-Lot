using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder : IPathFinder
{
    private const int MOVEMENT_COST = 10;
    private List<Tile> openList;
    private List<Tile> closeList;
    private GridSystem gridSystem;

    public AStarPathFinder(GridSystem gridSystem)
    {
        this.gridSystem = gridSystem;
    }

    public List<Tile> FindPath(int startX, int startY, int targetX, int targetY)
    {
        Tile startTile = gridSystem.GetTile(startX, startY);
        Tile endTile = gridSystem.GetTile(targetX, targetY);

        openList = new List<Tile>() { startTile };
        closeList = new List<Tile>();

        for (int i = 0; i < gridSystem.Width; i++)
        {
            for (int j = 0; j < gridSystem.Height; j++)
            {
                Tile currentTile = gridSystem.GetTile(i, j);
                currentTile.GCost = int.MaxValue;
                currentTile.Parent = null;
                currentTile.FCost = currentTile.GCost + currentTile.HCost;
            }
        }

        startTile.GCost = 0;
        startTile.HCost = CalculateDistanceCost(startTile, endTile);
        startTile.FCost = startTile.GCost + startTile.HCost;

        while (openList.Count > 0)
        {
            Tile currentTile = GetLowestFCostTile(openList);

            if (currentTile == endTile)
            {
                return GeneratePath(endTile);
            }

            openList.Remove(currentTile);
            closeList.Add(currentTile);

            foreach (Tile neighbour in GetNeighbours(currentTile))
            {
                if (closeList.Contains(neighbour)) continue;
                if (neighbour.IsBlocked)
                {
                    closeList.Add(neighbour);
                    continue;
                }

                int tempGCost = currentTile.GCost + CalculateDistanceCost(currentTile, neighbour);

                if (tempGCost < neighbour.GCost)
                {
                    neighbour.Parent = currentTile;
                    neighbour.GCost = tempGCost;
                    neighbour.HCost = CalculateDistanceCost(neighbour, endTile);
                    neighbour.FCost = neighbour.HCost + neighbour.GCost;
                }

                if (!openList.Contains(neighbour))
                {
                    openList.Add(neighbour);
                }
            }
        }

        return null;
    }

    private int CalculateDistanceCost(Tile a, Tile b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        return xDistance * MOVEMENT_COST + yDistance * MOVEMENT_COST;
    }
    private Tile GetLowestFCostTile(List<Tile> tiles)
    {
        Tile lowest = tiles[0];

        foreach (Tile tile in tiles)
        {
            if (tile.FCost < lowest.FCost)
            {
                lowest = tile;
            }
        }

        return lowest;
    }
    private List<Tile> GeneratePath(Tile endTile)
    {
        List<Tile> tiles = new List<Tile>() { endTile };

        Tile currentTile = endTile;

        while (currentTile.Parent != null)
        {
            tiles.Add(currentTile.Parent);
            currentTile = currentTile.Parent;
        }

        tiles.Reverse();

        return tiles;
    }
    private List<Tile> GetNeighbours(Tile source)
    {
        List<Tile> neighbours = new List<Tile>();

        if (source.X - 1 >= 0)
        {
            neighbours.Add(gridSystem.GetTile(source.X - 1, source.Y));
        }
        if (source.X + 1 < gridSystem.Width)
        {
            neighbours.Add(gridSystem.GetTile(source.X + 1, source.Y));
        }
        if (source.Y + 1 < gridSystem.Height)
        {
            neighbours.Add(gridSystem.GetTile(source.X, source.Y + 1));
        }
        if (source.Y - 1 >= 0)
        {
            neighbours.Add(gridSystem.GetTile(source.X, source.Y - 1));
        }

        return neighbours;
    }
}
