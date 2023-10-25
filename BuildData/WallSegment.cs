using System;
using UnityEngine;

namespace BuildBuy {


    public class WallSegment : AHousePiece, IWallSegment {
        public const float THICKNESS = 0.2f;
        public const float HALF_THICKNESS = THICKNESS * 0.5f;
        public const float Q_THICKNESS = HALF_THICKNESS * 0.5f;

        private Vector3 startPoint;
        private Vector3 endPoint;
        private WallSegmentPosition position;
        private Wall parent;

        private GameObject collision;
        private GameObject[] sides = new GameObject[2];

        public Vector3 StartPoint { get => startPoint; set { startPoint = value; } }
        public Vector3 EndPoint { get => endPoint; set { endPoint = value; } }

        public Wall Parent { get => parent; set { parent = value; } }

        public WallSegmentPosition Position { get => position; set { position = value; } }

        public GameObject Collision => collision;
        public GameObject[] Sides => sides;


        /// <summary>
        /// For creating new segments at arbitrary locations, typically when dividing a wall (e.g., adding something
        /// like a door or window into it).
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        public WallSegment(Vector3 startPoint, Vector3 endPoint, Wall parent, int position = 3) {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.parent = parent;
            this.position = (WallSegmentPosition)position;
        }


        /// <summary>
        /// For creating new segments at arbitrary locations, typically when dividing a wall (e.g., adding something
        /// like a door or window into it).
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        public WallSegment(Vector3 startPoint, Vector3 endPoint, Wall parent, WallSegmentPosition position) {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.parent = parent;
            this.position = position;
        }


        /// <summary>
        /// For the initial creation of the first segmant in a new wall.
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        public WallSegment(VertexPoint startPoint, VertexPoint endPoint, Wall parent,
                               WallSegmentPosition position = WallSegmentPosition.whole) {
            // FIXME:  This (0.1f) should be a public constant somewhere
            Vector3 fromVert = Vector3.Normalize(endPoint.location - startPoint.location) * 0.1f;
            this.startPoint = startPoint.location + fromVert;
            this.endPoint = endPoint.location - fromVert;
            this.parent = parent;
            this.position = position;
        }


        // FIXME, FIXME, FIXME!!! Don't get lazy with cubes, use proper, custom procedural meshes!!!
        public GameObject[] MeshIt() {
            float length = Vector3.Distance(endPoint, startPoint);
            Vector3 startToEnd = (endPoint - startPoint);
            Vector3 wallDir = startToEnd.normalized;
            Vector3 sideways = Vector3.Cross((endPoint - startPoint),  Vector3.up);
            Vector3 heightVector = Vector3.up * parent.Height;
            sideways.Normalize();
            sideways *= 0.1f;

            Vector3 center = StartPoint + (startToEnd * 0.5f) + (heightVector * 0.5f);
            if((position & WallSegmentPosition.start) > 0) {
                center += (wallDir * Q_THICKNESS);
                length -= HALF_THICKNESS;
            }
            if((position & WallSegmentPosition.end) > 0) {
                center -= (wallDir * Q_THICKNESS);
                length -= HALF_THICKNESS;
            }
            collision = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //sides[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //sides[1] = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if(Mathf.RoundToInt(startPoint.x) == Mathf.RoundToInt(endPoint.x)) {
                collision.transform.position = center;
                collision.transform.localScale = new Vector3(THICKNESS, heightVector.y, length);
                collision.name = "Wall Segment Hitbox";

            } else if(Mathf.RoundToInt(startPoint.z) == Mathf.RoundToInt(endPoint.z)) {
                collision.transform.position = center;
                collision.transform.localScale = new Vector3(length, heightVector.y, THICKNESS);
                collision.name = "Wall Segment Hitbox";

            }
            collision.AddComponent<PieceTag>().init(this, PieceType.segment);
            //sides[0].AddComponent<PieceTag>().init(this, PieceType.segment);
            //sides[1].AddComponent<PieceTag>().init(this, PieceType.segment);
            return sides;
        }


        public void Delete() {
            if(sides != null) {
                GameObject.Destroy(sides[0]);
                GameObject.Destroy(sides[1]);
            }
        }

    }
}