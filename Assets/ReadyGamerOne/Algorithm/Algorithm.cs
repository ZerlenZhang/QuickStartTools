using System;
using System.Collections.Generic;
using ReadyGamerOne.Algorithm.Graph;
using UnityEngine;

namespace ReadyGamerOne.Algorithm
{
    public static class Algorithm
    {
        /// <summary>
        /// 二维三角剖分算法
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static List<Triangle> Delaunay2D(List<Vector2> vertices)
        {
            var pointList=new List<Point>();

            #region 初始化顶点列表

            foreach (var VARIABLE in vertices)
            {
                pointList.Add(PointMgr.GetPoint(VARIABLE));
            }
            pointList.Sort();
            
            #endregion


            #region 获取超级三角形

            float l = pointList[0].pos.x;
            float r = pointList[pointList.Count-1].pos.x;
            
            float u = pointList[0].pos.y;
            float d = pointList[0].pos.y;
            foreach (var VARIABLE in pointList)
            {
                if (VARIABLE.pos.y < d)
                    d = VARIABLE.pos.y;
                if (VARIABLE.pos.y > u)
                    u = VARIABLE.pos.y;
            }

            var width = r - l;
            var hight = u - d;
            if (width < hight)
                width = hight;
            var top = new Vector3((l + r) / 2, u + hight, 0);
            var left = new Vector3((l + r) / 2 - width - 5, d - 5, 0);
            var right = new Vector3((l + r) / 2 + width + 5, d - 5, 0);

            var superTriangle = TriangleMgr.GetTriangle(top, left, right);            

            
            #endregion

            
            
            var targetTriangles = new List<Triangle>();
            var tempTriangles = new List<Triangle>();
            
            //将超级三角形加入临时三角形队列
            tempTriangles.Add(superTriangle);
            targetTriangles.Add(superTriangle);
            
            
            for(var i=0;i<pointList.Count;i++)
            {
                var point = pointList[i];
                var edgeBuffer = new List<Line>();
                for(var j=0;j<tempTriangles.Count;j++)
                {
                    var triangle = tempTriangles[j];
                    var outCircle = triangle.GetOutCircle();
                    
                    //如果在园内，说明，当前三角形不是Delaunay三角形
                    if (outCircle.ContansPoint(point))
                    {
                        //将三边加入边缓存
                        foreach (var edge in triangle.Edges)
                        {
                            //判断是否重复
                            bool add = true;
                            foreach (var VARIABLE in edgeBuffer)
                            {
                                if (VARIABLE==edge)
                                    add = false;
                            }

                            if (add)
                                edgeBuffer.Add(edge);
                            else
                                edgeBuffer.Remove(edge);
                        }

                        //移除当前三角形
                        tempTriangles.RemoveAt(j);
                        j--;
                    }//如果点在外接圆右侧，说明当前三角形是Delaunay三角形
                    else if (point.pos.x > outCircle.CenterPos.x+outCircle.radius)
                    {
                        targetTriangles.Add(triangle);
                        tempTriangles.RemoveAt(j);
                        j--;
                    }

                }
                //当前点和缓存边相连构成三角形
                foreach (var edge in edgeBuffer)
                {
                    tempTriangles.Add(TriangleMgr.GetTriangle(point, edge));
                }      
                
            }
            
            //合并targetTriangles 和TempTriangles
            foreach (var t in tempTriangles)
            {
                if (targetTriangles.Contains(t) == false)
                    targetTriangles.Add(t);
            }
            
            //除去与超级三角形有关的三角形

            for(var i=0;i<targetTriangles.Count;i++)
            {
                var triangle = targetTriangles[i];
                var edges = triangle.Points;
                foreach (var superEdge in superTriangle.Points)
                {
                    if (edges.Contains(superEdge))
                    {
                        targetTriangles.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }

            return targetTriangles;


        }

        /// <summary>
        /// 克鲁斯卡尔算法最小生成树
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public static List<Line> Kruskal(List<Line> edges)
        {
            edges.Sort();
            var arrive = new Dictionary<int,int>();
            foreach (var edge in edges)
            {
                if (!arrive.ContainsKey(edge.pointIds[0]))
                    arrive.Add(edge.pointIds[0],0);
                if (!arrive.ContainsKey(edge.pointIds[1]))
                    arrive.Add(edge.pointIds[1], 0);
            }

            var finalList = new List<Line>();
            foreach (var edge in edges)
            {
                if (finalList.Count >= arrive.Count-1)
                    break;
                var points = edge.pointIds;
                var m = Find(arrive, points[0]);
                var n = Find(arrive, points[1]);
                if (m != n)
                {
                    arrive[m] = n;
                    finalList.Add(edge);
                }
            }

            return finalList;

        }

        #region Private

        private static int Find(Dictionary<int,int> arrive, int index)
        {
            try
            {
                while (arrive[index] > 0)
                {
                    index = arrive[index];
                }

                return index;
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("数组越界,index: "+index);
            }

        }        

        #endregion

    }
}