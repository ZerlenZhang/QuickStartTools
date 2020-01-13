using System;
using System.Collections.Generic;
using System.Linq;
using ReadyGamerOne.Utility;
using UnityEngine;
using UnityEngine.Assertions;

namespace ReadyGamerOne.Algorithm.Graph
{
    public class Line:BasicGraph,IComparable<Line>,IDrawGizmos
    {
        public readonly int[] pointIds;
        
        public List<int> PointIDs => pointIds.ToList();
        public List<Point> Points
        {
            get
            {
                var ps = new List<Point>();
                ps.Add(PointMgr.GetPoint(pointIds[0]));
                ps.Add(PointMgr.GetPoint(pointIds[1]));
                return ps;
            }
        }

        public float Length
        {
            get
            {
                var points = Points;
                return Vector2.Distance(points[0].pos, points[1].pos);
            }
        }

        public void OnDrawGizmos()
        {
            var points = Points;
            Gizmos.DrawLine(points[0].pos,points[1].pos);
        }
        
        internal Line(int id1, int id2)
        {
            pointIds = new[] {id1, id2};
            Assert.IsTrue(pointIds[0]!=pointIds[1]);
        }

        public int CompareTo(Line other)
        {
            if (Length < other.Length)
                return -1;
            if (Length > other.Length)
                return 1;
            return 0;
        }
    }
}