using System.Collections.Generic;

public interface IPathFinder 
{
    public List<Tile> FindPath(int startX, int startY, int targetX, int targetY);
}
