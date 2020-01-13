using System;
using System.Collections;
using System.Collections.Generic;
using ReadyGamerOne.Algorithm.Graph;
using ReadyGamerOne.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace ReadyGamerOne.Algorithm
{
    internal class RoomInfo
    {
        #region Static

        internal enum PosType
        {
            Cross,
            Ymid,
            Xmid,
            Null
        }

        public static bool Cross(RoomInfo roomInfo1, RoomInfo roomInfo2)
        {
            var notCross = roomInfo1.Left > roomInfo2.Right
                           || roomInfo1.Up < roomInfo2.Down
                           || roomInfo1.Down > roomInfo2.Up
                           || roomInfo1.Right < roomInfo2.Left;
            return !notCross;

        }

        public static PosType GetPointPosType(RoomInfo room1, RoomInfo room2)
        {
            var maxLeft = Mathf.Max(room1.Left, room2.Left);
            var minRight = Mathf.Min(room1.Right, room2.Right);
            var maxDown = Mathf.Max(room1.Down, room2.Down);
            var minUp = Mathf.Min(room1.Up, room2.Up);
            
            
            var midPoint = 0.5f * (room1.Pos + room2.Pos);

            var xOk = midPoint.x < minRight && midPoint.x > maxLeft;
            var yOk = midPoint.y > maxDown && midPoint.y < minUp;

            if (Cross(room1, room2))
                return PosType.Cross;


            if (yOk)
                return PosType.Ymid;
            if (xOk)
                return PosType.Xmid;            
            return PosType.Null;

        }
        

        #endregion
        
        public int id;
        public GameObject obj;
        public readonly float GridSize;

        #region Properties

        #region 方位
        public float Left => Pos.x - GridSize * Size.x/2;
        public float Right => Pos.x + GridSize * Size.x/2;
        public float Up => Pos.y + GridSize * Size.y/2;
        public float Down => Pos.y - GridSize * Size.y/2;
        

        #endregion

        public Vector2Int Size { get;}
        public Vector3 Pos
        {
            get { return obj.transform.position; }
            set { obj.transform.position = value; }
        }            
        public BoxCollider2D BoxCollider2D { get; private set; }
        public Rigidbody2D Rigidbody2D { get; private set; }
        public Rigidbody2D AddRigidbody2D()
        {
            if (Rigidbody2D == null)
            {
                Rigidbody2D = obj.AddComponent<Rigidbody2D>();
                Rigidbody2D.gravityScale = 0;
                Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            return Rigidbody2D;
        }        

        #endregion

        private List<RoomInfo> closedRooms=new List<RoomInfo>();

        /// <summary>
        /// 添加临近房间
        /// </summary>
        /// <param name="roomInfo"></param>
        public void AddClosedRoom(RoomInfo roomInfo)
        {
            if (closedRooms.Contains(roomInfo) == false)
                closedRooms.Add(roomInfo);
        }

        /// <summary>
        /// 这个点在房间范围内
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool IsPointIn(Vector2 point)
        {
            return point.x > Left 
                   && point.x < Right
                   && point.y > Down
                   && point.y < Up;
        }
        
        public RoomInfo(int id, string name,float gridSize, Vector2Int size,Vector2 pos,Transform parent)
        {
            this.id = id;
            this.GridSize = gridSize;
            this.Size = size;
            obj = new GameObject(name);
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
            BoxCollider2D = obj.AddComponent<BoxCollider2D>();
            BoxCollider2D.size = new Vector2(gridSize * size.x, gridSize * size.y);
        }
    }
    public class RandomMap:MonoBehaviour
    {
        public Transform roomParent;
        public bool ShowMap = true;
        public bool ShowTriangles = true;
        public bool ShowMinTree = true;
        public bool ShowRooms = true;
        public Grid grid;
        public Tilemap tilemap;
        public TileBase tile;
        public TileBase pathTile;
        public Rect mapRange;

        public Vector2Int mapWidthRange;
        public Vector2Int mapHightRange;
    
        public int roomCount;
        public Vector2Int roomWidthRange;
        public Vector2Int roomHightRange;

        public int pathWidth = 3;
        
        private List<RoomInfo> _roomInfos = new List<RoomInfo>();
        public float GridSize;
        private List<Triangle> triangles;
        private List<Line> minTreeLines;
        private Dictionary<Point, RoomInfo> point2Room = new Dictionary<Point, RoomInfo>();


        [ContextMenu("全搞定")]
        private void WorkAll()
        {
            StartCoroutine(FixCoorinate(() =>
            {
                DrawTiles();
                Trianglute();
                MinTree();
                ConnectRooms();
            }));
        }
        
        #region 生成房屋

        private void GenerateMap()
        {
            mapRange = new Rect(Vector2.zero, new Vector2(
                GridSize* Random.Range(mapWidthRange.x, mapWidthRange.y),
                GridSize*Random.Range(mapHightRange.x, mapHightRange.y)));
        }

        private void GenerateRoom()
        {
            for (var i = 0; i < roomCount; i++)
            {
                var room = new RoomInfo(i, "Room_" + i, 
                    GridSize,
                    new Vector2Int(
                        Random.Range(roomWidthRange.x, roomWidthRange.y),
                        Random.Range(roomHightRange.x, roomHightRange.y)),
                    mapRange.position + new Vector2(
                        GridSize*Random.Range(0,mapRange.width),
                        GridSize*Random.Range(0,mapRange.height)),
                        roomParent);
                _roomInfos.Add(room);
            }
        }
        

        #endregion

        #region 调整坐标和显示

        [ContextMenu("添加刚体")]
        private void AddRigidBody()
        {
            StartCoroutine(FixCoorinate(null));
        }

        [ContextMenu("画上砖块")]
        private void DrawTiles()
        {
            foreach (var VARIABLE in _roomInfos)
            {
                var box = VARIABLE.BoxCollider2D;
                Vector3[] corners;
                box.GetCorners(out corners);
                for (var i = 0; i < 4; i++)
                {
                    var j = (i + 1) % 4;
                    var start = corners[i];
                    var end = corners[j];
                    var step = (end - start).normalized*0.5f;
                    for (var p = start; Vector2.Distance(p, end) >= 0.1f; p += step)
                    {
                        var pos = new Vector3Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.y), 0);
                        if(null==tilemap.GetTile(pos))
                        {
                            tilemap.SetTile(pos, tile);
                        }
                    }
                }
            }
        }

        private IEnumerator FixCoorinate(Action endCall)
        {
            foreach (var VARIABLE in _roomInfos)
            {
                VARIABLE.AddRigidbody2D();
            }

            while (!AllRoomSleep())
            {
                yield return new WaitForFixedUpdate();
            }

            Debug.Log("开始坐标休整");
            foreach (var VARIABLE in _roomInfos)
            {
                VARIABLE.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                VARIABLE.Pos=new Vector3( GridSize * ((int)(VARIABLE.Pos.x/GridSize)),
                                              GridSize * ((int)(VARIABLE.Pos.y/GridSize)),
                                              VARIABLE.Pos.z);
                //Point和room映射
                point2Room.Add(PointMgr.GetPoint(VARIABLE.Pos), VARIABLE);
            }
            
            Debug.Log("坐标休整完成");
            endCall?.Invoke();
        }

        private bool AllRoomSleep()
        {
            foreach (var VARIABLE in _roomInfos)
            {
                if (!VARIABLE.Rigidbody2D.IsSleeping())
                    return false;
            }

            return true;
        }        

        #endregion

        #region 三角剖分

        [ContextMenu("三角剖分")]
        private void Trianglute()
        {
            var posList=new List<Vector2>();
            foreach (var VARIABLE in _roomInfos)
            {
                posList.Add(VARIABLE.Pos);
            }
            triangles = Algorithm.Delaunay2D(posList);
        }

        #endregion

        #region 最小生成树


        [ContextMenu("最小生成树")]
        private void MinTree()
        {
            var list =new List<Line>();
            foreach (var VARIABLE in triangles)
            {
                list.AddRange(VARIABLE.Edges);
            }

            minTreeLines = Algorithm.Kruskal(list);
        }

        #endregion

        #region 绘制走廊
        [ContextMenu("绘制走廊")]
        private void ConnectRooms()
        { 
            foreach (var line in minTreeLines)
            {
                #region 房屋互相联系起来

                var points = line.pointIds;
                var p1 = PointMgr.GetPoint(points[0]);
                var p2 = PointMgr.GetPoint(points[1]);
                var room1 = point2Room[p1];
                var room2 = point2Room[p2];
                room1.AddClosedRoom(room2);
                room2.AddClosedRoom(room1);                

                #endregion

                var posType = RoomInfo.GetPointPosType(room1, room2);

                print(room1.obj.name+"  "+room2.obj.name+"  "+ posType);
                
                switch (posType)
                {
                    case RoomInfo.PosType.Cross:
                        break;
                    case RoomInfo.PosType.Null:

                        var dir = room2.Pos - room1.Pos;
                        
                        
                        break;
                    case RoomInfo.PosType.Ymid:
                        var heightRange = new Vector2(
                            Mathf.Max(room1.Down, room2.Down)+GridSize*pathWidth/2,
                            Mathf.Min(room1.Up, room2.Up)-GridSize*pathWidth/2);
                        var minRight = Mathf.Min(room1.Right, room2.Right);
                        var maxLeft = Mathf.Max(room1.Left, room2.Left);
                        var height = Random.Range(heightRange.x, heightRange.y);
                        var hh = height + GridSize * pathWidth / 2;
                        var lh = height - GridSize * pathWidth / 2;
                        Debug.Log(heightRange);
                        Debug.Log(minRight);
                        Debug.Log(maxLeft);
                        for (var i = minRight; i < maxLeft; i += 0.5f * GridSize)
                        {
                            tilemap.SetTile(new Vector3Int(Mathf.RoundToInt(i), Mathf.RoundToInt(hh), 0), pathTile);
                            tilemap.SetTile(new Vector3Int(Mathf.RoundToInt(i), Mathf.RoundToInt(lh), 0), pathTile);
                        }
                        break;
                    case RoomInfo.PosType.Xmid:
                        var widthRange = new Vector2(
                            Mathf.Max(room1.Left, room2.Left)+GridSize*pathWidth/2,
                            Mathf.Min(room1.Right, room2.Right)-GridSize*pathWidth/2);
                        var minUp = Mathf.Min(room1.Up, room2.Up);
                        var maxDown = Mathf.Max(room1.Down, room2.Down);
                        var width = Random.Range(widthRange.x, widthRange.y);
                        var lw = width - GridSize * pathWidth / 2;
                        var rw = width + GridSize * pathWidth / 2;
                        for (var i = minUp; i < maxDown; i += 0.5f * GridSize)
                        {
                            tilemap.SetTile(new Vector3Int(Mathf.RoundToInt(lw), Mathf.RoundToInt(i), 0), pathTile);
                            tilemap.SetTile(new Vector3Int(Mathf.RoundToInt(rw), Mathf.RoundToInt(i), 0), pathTile);
                        }
                        break;
                }


















            }
            
            
            
        }

        #endregion

        private void Clear()
        {
            tilemap.ClearAllTiles();
            point2Room.Clear();
            triangles?.Clear();
            minTreeLines?.Clear();
            foreach (var VARIABLE in _roomInfos)
            {
                DestroyImmediate(VARIABLE.obj);
            }
            _roomInfos.Clear();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(ShowMap)
                GizmosUtil.DrawRect(mapRange);
            if (ShowRooms)
            {
                Gizmos.color=Color.yellow;
                foreach (var VARIABLE in _roomInfos)
                {
                    Handles.Label(VARIABLE.Pos, VARIABLE.obj.name);
                    GizmosUtil.DrawBoxCollider2D(VARIABLE.BoxCollider2D);
                }
            }

            if (null == triangles)
                return;
            if (ShowTriangles)
            {
                Gizmos.color=Color.red;
                foreach (var VARIABLE in triangles)
                {
                    VARIABLE.OnDrawGizmos();
                }
            }

            if (null == minTreeLines)
                return;
            if (ShowMinTree)
            {
                Gizmos.color=Color.cyan;
                foreach (var VARIABLE in minTreeLines)
                {
                    VARIABLE.OnDrawGizmos();
                }                
            }

        }
#endif


        [ContextMenu("再来一遍")]
        private void Start()
        {
            Clear();
            GridSize = grid.cellSize.x;
            GenerateMap();
            GenerateRoom();
        }

        
    }
}