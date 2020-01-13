using System.Collections.Generic;
using System.Linq;
using ReadyGamerOne.Utility;
using UnityEngine;

namespace ReadyGamerOne.Algorithm.Graph
{
    public class Triangle:BasicGraph,IDrawGizmos
    {
        public readonly int[] pointIds;

        public List<int> PointIds => pointIds.ToList();

        public List<Point> Points
        {
            get
            {
                var l=new List<Point>();
                foreach (var VARIABLE in pointIds)
                {
                    l.Add(PointMgr.GetPoint(VARIABLE));
                }

                return l;
            }
        }

        public List<Line> Edges
        {
            get
            {
                var l=new List<Line>();
                for (var i = 0; i < 3; i++)
                {
                    var j = (i + 1) % 3;
                    l.Add(LineMgr.GetEdge(pointIds[i],pointIds[j]));
                }

                return l;
            }
        }

        public Circle GetOutCircle()
        {
            var points = Points;
 
            var x1  =  points[0].pos.x; 
            var x2  =  points[1].pos.x;
            var x3  =  points[2].pos.x;            
            var y1  =  points[0].pos.y; 
            var y2  =  points[1].pos.y;
            var y3  =  points[2].pos.y;
 
            //求外接圆半径
            var a=Mathf.Sqrt( (x1-x2)*(x1-x2)+(y1-y2)*(y1-y2) );
            var b=Mathf.Sqrt( (x1-x3)*(x1-x3)+(y1-y3)*(y1-y3) );
            var c=Mathf.Sqrt( (x2-x3)*(x2-x3)+(y2-y3)*(y2-y3) );
            var p=(a+b+c)/2;
            var S=Mathf.Sqrt( p*(p-a)*(p-b)*(p-c) );
            var radius=a*b*c/(4*S);
 
            //求外接圆圆心
            var t1=x1*x1+y1*y1;
            var t2=x2*x2+y2*y2;
            var t3=x3*x3+y3*y3;
            var temp=x1*y2+x2*y3+x3*y1-x1*y3-x2*y1-x3*y2;
            var x=(t2*y3+t1*y2+t3*y1-t2*y1-t3*y2-t1*y3)/temp/2;
            var y=(t3*x2+t2*x1+t1*x3-t1*x2-t2*x3-t3*x1)/temp/2;

            return CircleMgr.GetCircle(new Vector2(x, y), radius);
        }


        public void OnDrawGizmos()
        {
            var points = Points;
            for (var i = 0; i < 3; i++)
            {
                var j = (i + 1) % 3;
                Gizmos.DrawLine(points[i].pos,points[j].pos);
            }
        }
        
        

        internal Triangle(int point1, int point2, int point3)
        {
            pointIds = new[] {point1, point2, point3};
        }
    }
}