using System.Collections.Generic;
using UnityEngine;

namespace ReadyGamerOne.Algorithm.Graph
{
    public static class PointMgr
    {
        private static Dictionary<int, Point> id2Points = new Dictionary<int, Point>();
        
        public static Point GetPoint(int id)
        {
            if (id2Points.ContainsKey(id))
                return id2Points[id];
            return null;
        }

        public static Point GetPoint(Vector2 pos)
        {
            foreach (var VARIABLE in id2Points)
            {
                if (VARIABLE.Value==pos)
                    return VARIABLE.Value;
            }

            var point = new Point(pos);
            id2Points.Add(point.id, point);
            return point;
        }
    }
}