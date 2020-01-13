using System.Collections.Generic;
using UnityEngine;

namespace ReadyGamerOne.Algorithm.Graph
{
    public static class TriangleMgr
    {
        private static Dictionary<int, Triangle> id2Trianges = new Dictionary<int, Triangle>();

        public static Triangle GetTriangle(int id)
        {
            foreach (var VARIABLE in id2Trianges)
            {
                if (VARIABLE.Key == id)
                    return VARIABLE.Value;
            }

            return null;
        }

        public static Triangle GetTriangle(int pointid1, int pointid2, int pointid3)
        {
            foreach (var VARIABLE in id2Trianges)
            {
                var ids = VARIABLE.Value.PointIds;
                if (ids.Contains(pointid1)
                    && ids.Contains(pointid2)
                    && ids.Contains(pointid3))
                    return VARIABLE.Value;
            }

            var nt = new Triangle(pointid1, pointid2, pointid3);
            id2Trianges.Add(nt.id, nt);
            return nt;
        }
        public static Triangle GetTriangle(Point p1, Point p2, Point p3)
        {
            return GetTriangle(p1.id, p2.id, p3.id);
        }

        public static Triangle GetTriangle(Vector2 pos1, Vector2 pos2, Vector2 pos3)
        {
            return GetTriangle(
                PointMgr.GetPoint(pos1),
                PointMgr.GetPoint(pos2),
                PointMgr.GetPoint(pos3));
        }

        public static Triangle GetTriangle(Point p, Line line)
        {
            return GetTriangle(p.id, line.pointIds[0], line.pointIds[1]);
        }
        
    }
}