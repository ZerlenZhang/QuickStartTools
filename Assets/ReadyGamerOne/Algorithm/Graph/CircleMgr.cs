using System.Collections.Generic;
using UnityEngine;

namespace ReadyGamerOne.Algorithm.Graph
{
    public class CircleMgr
    {
        private static Dictionary<int, Circle> id2Circles = new Dictionary<int, Circle>();

        public static Circle GetCircle(int id)
        {
            foreach (var VARIABLE in id2Circles)
            {
                if (VARIABLE.Key == id)
                    return VARIABLE.Value;
            }

            return null;
        }


        public static Circle GetCircle(int pointId, float r)
        {
            foreach (var VARIABLE in id2Circles)
            {
                if (VARIABLE.Value.Center.id == pointId
                    && Mathf.Abs(VARIABLE.Value.radius - r) < ConstDefine.MinDis)
                {
                    return VARIABLE.Value;
                }
            }

            var nc = new Circle(pointId, r);
            id2Circles.Add(nc.id, nc);
            return nc;
        }

        public static Circle GetCircle(Point point, float r)
        {
            return GetCircle(point.id, r);
        }

        public static Circle GetCircle(Vector2 pos, float r)
        {
            return GetCircle(PointMgr.GetPoint(pos), r);
        }
        
    }
}