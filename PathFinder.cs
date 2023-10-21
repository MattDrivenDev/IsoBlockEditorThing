using System;
using System.Collections.Generic;
using System.Linq;

namespace IsoBlockEditor
{
    public class PathFinder
    {
        IsoBlockyMappy _map;

        public PathFinder(IsoBlockyMappy map)
        {
            _map = map;
        }

        public List<IsoBlockyTile> FindPath(IsoBlockyTile start, IsoBlockyTile end)
        {
            var open = new List<IsoBlockyTile>();
            var closed = new HashSet<IsoBlockyTile>();

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
                    throw new NotImplementedException("Path found, but not yet implemented.");
                }

                var neighbors = _map.GetNeighbouringTiles(current);
            }

            throw new NotImplementedException();
        }
    }
}
