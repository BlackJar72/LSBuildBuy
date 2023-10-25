using kfutils;
using UnityEngine;

namespace BuildBuy {

    /// New Plan
    ///
    /// Walls for building stories will be defined in terms of points between which line segments are drawn.  Lines
    /// (technically segments and meaning walls) can be understood the connection between two points.  When a new
    /// line is drawn it can be tested against existing lines for intersection, and if and interesction is found
    /// a new vertext point would then be added at the intersection and made the neighbor of the relevant endpoint.
    ///
    /// Geometry can be generated based on creating a tall, thin box around the line, while the angle betwen it and the
    /// last segment can be used to adjust the mesh vertices.
    ///
    /// Vertex point define segments, which, can define rooms when inclosed.
    ///
    /// It is tempting to use a room first / only placement system, whith any extra lines not defining rooms, though
    /// this might wsee awkward, and certainly seem unexpected, to players of existing games.  Finding a good way of
    /// detecting complete rooms (loops) would be good.  For now, rooms will be defined first, and the area of used
    /// to convert tiles to being inside the room.
    ///
    /// I REALLY SHOULD LIMIT MYSELF TO RECTANGULAR WALLS, AND TREAT IT AS A HOBBY PROJECT, NOT SOMETHING TO COMPETE
    /// WITH PARALIVES AND LIFE BY YOU (AND THE SIMS).  JUST MAKE IT AND HAVE IT WORK!!!!!
    ///


    public class VertexPoint : AHousePiece, ICubeMeshable  {
        // the actual point in space this vertex represents, i.e., it position
        public Vector3Int location;
        public float height; // FIXME? Is this even needed?

        // Fixed the adjacent vertices
        public VertexPoint nextPoint = null;
        public VertexPoint rightPoint = null;
        public VertexPoint previousPoint = null;
        public VertexPoint leftPoint = null;

        // Next, the connecting edges
        public Wall forward = null;
        public Wall right = null;
        public Wall backward = null;
        public Wall left = null;

        private GameObject[] geometry = null;


        public VertexPoint(Vector3 location, Story story) {
            this.location = location.RoundToInt();
            height = story.Height;
            MeshIt();
        }


        public GameObject[] MeshIt() {
            if(geometry == null) {
                geometry = new GameObject[1];
                geometry[0] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                geometry[0].transform.position = location + (Vector3.up * (height * 0.5f));
                geometry[0].transform.localScale = new Vector3(WallSegment.THICKNESS, height, WallSegment.THICKNESS);
                geometry[0].name = "Wall Vertex";
                geometry[0].AddComponent<PieceTag>().init(this, PieceType.vertex);
            }
            return geometry;
        }


    }
}