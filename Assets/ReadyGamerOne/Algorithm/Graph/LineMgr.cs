using System.Collections.Generic;
using UnityEngine;

namespace ReadyGamerOne.Algorithm.Graph
{
    public static class LineMgr
    {
        private static Dictionary<int, Line> id2Edges = new Dictionary<int, Line>();

        public static Line GetEdge(int id)
        {
            foreach (var VARIABLE in id2Edges)
            {
                if (VARIABLE.Key == id)
                    return VARIABLE.Value;
            }

            return null;
        }

        public static Line GetEdge(int pointId1, int pointId2)
        {
            foreach (var VARIABLE in id2Edges)
            {
                var e = VARIABLE.Value.PointIDs;
                if (e.Contains(pointId1) && e.Contains(pointId2))
                    return VARIABLE.Value;
            }

            var ne = new Line(pointId1, pointId2);
            id2Edges.Add(ne.id, ne);
            return ne;
        }

        public static Line GetEdge(Vector2 pos1, Vector2 pos2)
        {
            return GetEdge(PointMgr.GetPoint(pos1), PointMgr.GetPoint(pos2));
        }

        public static Line GetEdge(Point point1, Point point2)
        {
            return GetEdge(point1.id, point1.id);
        }
    }
}