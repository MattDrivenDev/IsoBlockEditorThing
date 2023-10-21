using System;
using System.Collections.Generic;
using System.Linq;

namespace IsoBlockEditor
{
    /// <summary>
    /// A* pathfinding algorithm.
    /// https://www.youtube.com/watch?v=mZfyt03LDH4
    /// </summary>
    public class PathFinder
    {
        public const int ORTHOGONAL_COST = 10;
        public const int DIAGONAL_COST = 14;

        IsoBlockyMappy _map;

        public PathFinder(IsoBlockyMappy map)
        {
            _map = map;
        }

        public List<IsoBlockyTile> FindPath(IsoBlockyTile start, IsoBlockyTile end)
        {
            var open = new List<IsoBlockyTile>();
            var closed = new HashSet<IsoBlockyTile>();
            var path = new List<IsoBlockyTile>();

            open.Add(start);

            while (open.Any())
            {
                var current = open[0];
                
                for(var i = 1; i < open.Count; i++)
                {
                    if (open[i].F < current.F || (open[i].F == current.F && open[i].H < current.H)) current = open[i];
                }

                open.Remove(current);
                closed.Add(current);

                if (current == end)
                {
                    var p = end;
                    while (p != start)
                    {
                        path.Add(p);
                        p = p.Parent;
                    }
                    path.Reverse();
                    return path;
                }

                var neighbors = _map.GetNeighbouringTiles(current);
                foreach (var neighbor in neighbors)
                {
                    if (!neighbor.IsActive || closed.Contains(neighbor)) continue;

                    var newCost = current.G + GetDistance(current, neighbor);
                    if (newCost < neighbor.G || !open.Contains(neighbor))
                    {
                        neighbor.G = newCost;
                        neighbor.H = GetDistance(neighbor, end);
                        neighbor.Parent = current;
                        if (!open.Contains(neighbor)) open.Add(neighbor);
                    }
                }
            }

            return path;
        }

        public int GetDistance(IsoBlockyTile start, IsoBlockyTile end)
        {
            var rowDistance = Math.Abs(start.Index.row - end.Index.row);
            var colDistance = Math.Abs(start.Index.column - end.Index.column);

            if (rowDistance > colDistance)
            {
                return DIAGONAL_COST * colDistance + ORTHOGONAL_COST * (rowDistance - colDistance);
            }
            else
            {
                return DIAGONAL_COST * rowDistance + ORTHOGONAL_COST * (colDistance - rowDistance);
            }
        }
    }
}
