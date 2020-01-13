using UnityEngine;

namespace ReadyGamerOne.Algorithm.Graph
{
    public class Circle:BasicGraph
    {
        private int pointId;
        public Point Center => PointMgr.GetPoint(pointId);
        public readonly float radius;
        public Vector2 CenterPos => Center.pos;

        public bool ContansPoint(Vector2 pos)
        {
            return Vector2.Distance(Center.pos, pos) < radius;
        }
        public bool ContansPoint(Point pos)
        {
            return Vector2.Distance(Center.pos, pos.pos) < radius;
        }


        public void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(CenterPos, radius);
        }
        

        internal Circle(int centerPointId, float r)
        {
            this.pointId = centerPointId;
            this.radius = r;
        }
    }
}