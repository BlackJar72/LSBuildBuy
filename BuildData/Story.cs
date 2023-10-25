using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuildBuy {

    [System.Serializable]
    public class Story {
        [SerializeField] int level;
        [SerializeField] float altitude;
        [SerializeField] float height;
        [SerializeField] List<VertexPoint> vertices = new List<VertexPoint>();
        [SerializeField] List<Wall> walls = new List<Wall>();
        [SerializeField] List<Room> rooms = new List<Room>();

        public int story => level;
        public int Level => level;
        public float Altitude => altitude;
        public float Height => height;
        public List<VertexPoint> Vertices => vertices;
        public List<Wall> Walls => walls;
        public List<Room> Rooms => rooms;


        public VertexPoint GetVertexAt(Vector3 point) {
            VertexPoint output = null;
            for(int i = 0; (output == null) && (i < vertices.Count); i++) {
                if(Vector3.Distance(vertices[i].location, point) < 0.1f) output = vertices[i];
            }
            return output;
        }


        public VertexPoint GetOrMakeVertexAt(Vector3 point) {
            VertexPoint output = null;
            for(int i = 0; (output == null) && (i < vertices.Count); i++) {
                if(Vector3.Distance(vertices[i].location, point) < 0.1f) output = vertices[i];
            }
            if(output == null) output = new VertexPoint(point, this);
            return output;
        }


    }
}