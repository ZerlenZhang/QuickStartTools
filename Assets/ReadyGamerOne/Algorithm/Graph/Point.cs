using System;
using UnityEngine;

namespace ReadyGamerOne.Algorithm.Graph
{
    public class Point:BasicGraph,IComparable<Point>
    {
        protected bool Equals(Point other)
        {
            return pos.Equals(other.pos);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            return pos.GetHashCode();
        }

        public Vector2 pos;
        internal Point(Vector2 pos)
        {
            this.pos = pos;
        }
        
        
        public static bool operator==(Point point, Vector2 position)
        {
            return Vector2.Distance(position, point.pos) < ConstDefine.MinDis;
        }

        public static bool operator !=(Point point, Vector2 position)
        {
            return !(point == position);
        }

        public int CompareTo(Point other)
        {
            if (pos.x < other.pos.x)
                return -1;
            if (pos.x > other.pos.x)
                return 1;
            return 0;
        }
    }
}