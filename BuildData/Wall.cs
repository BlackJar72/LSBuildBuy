using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuildBuy {

    public class Wall : AHousePiece, ICubeMeshable {

        private float height;
        private List<IWallSegment> segments;
        private VertexPoint startPoint, endPoint;

        private List<GameObject> positiveSide;
        private List<GameObject> negativeSide;

        public float Height { get => height;  set { height =  value; } }


        /// <summary>
        /// This is used to create a wall between two VertexPoints.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Wall(VertexPoint start, VertexPoint end, Story story) {
            startPoint = start;
            endPoint = end;
            height = story.Height;
            startPoint.nextPoint = endPoint;
            endPoint.previousPoint = startPoint;
            startPoint.forward = endPoint.backward = this;
            segments = new List<IWallSegment>();
            segments.Add(new WallSegment(start, end, this));
        }


        #region Reqiored but unused methods
        /// <summary>
        /// Probably not the best way to handle this
        /// </summary>
        /// <returns></returns>
        [Obsolete("This should not be used; instead call ..?.. and then get the relevant stored data.")]
        public GameObject[] MeshIt() {
            List<GameObject[]> preout = new List<GameObject[]>();
            int sizeout = 2;
            preout.Add(startPoint.MeshIt());
            for(int i = 0; i < segments.Count; i++) {
                GameObject[] next = segments[i].MeshIt();
                preout.Add(next);
                sizeout += next.Length;
            }
            preout.Add(endPoint.MeshIt());
            int index = 0;
            GameObject[] output = new GameObject[sizeout];
            for(int i = 0; i < preout.Count; i++) {
                for(int j = 0; j < preout[i].Length; j++) {
                    output[index] = preout[i][j];
                }
            }
            return output; // FIXME!
        }
        #endregion



    }

}
