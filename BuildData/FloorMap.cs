using System.Collections.Generic;
using UnityEngine;

namespace BuildBuy {


    public class FloorMap {
        private const float SCALE = 1f;       // This might become a variable later, so including now
        private const float THICKNESS = 0.1f; // This might become a variable later, so including now

        public readonly Vector3 position;
        public readonly Vector2Int lotSize;
        public readonly Vector2Int lotSizeP1;
        public readonly Vector2 heights;
        // Tags for build item IDs
        public readonly int[,] vWallData;
        public readonly int[,] hWallData;
        public readonly int[,] gridVertexData;
        public readonly int[,] tileData;
        public readonly int[,] floorData;
        public readonly int[,] ceilingData;

        private List<GameObject> drawnObjects;


        public FloorMap(Vector3 location, int width, int depth, float altitude, float height) {
            position = location;
            lotSize = new Vector2Int(width, depth);
            lotSizeP1 = new Vector2Int(width + 1, depth + 1);
            heights = new Vector2(altitude, height);
            vWallData = new int[width + 1, depth];
            hWallData = new int[width, depth + 1];
            gridVertexData = new int[width + 1, depth + 1];
            tileData =  new int[width, depth];
            floorData =  new int[width, depth];
            ceilingData =  new int[width, depth];
        }


        public void DeleteAllDrawn() {
            for(int i = 0; i < drawnObjects.Count; i++) {
                GameObject.Destroy(drawnObjects[i]);
            }
            drawnObjects.Clear();
        }


        public void Redraw() {
            DeleteAllDrawn();
            DrawWalls();
        }


        public void DrawWalls() {
            // "Vertical" (for lack of better name) walls; those parallel to the Z axis
            for(int i = 0; i < lotSizeP1.x; i++)
                for(int j = 0; j < lotSize.y; j++) {
                    //For now, don't worry about extra value (what was I going to do with them anyway?!?)
                    if(vWallData[i,j] > 0) {
                        drawnObjects.Add(BuildBox(((float)i * SCALE) + position.x,
                                (((float)j + 0.5f) * SCALE) + position.z, THICKNESS, SCALE));
                    }
                }
            // Horizontal walls; those parallel to the X axis
            for(int i = 0; i < lotSize.x; i++)
                for(int j = 0; j < lotSizeP1.y; j++) {
                    //For now, don't worry about extra value (what was I going to do with them anyway?!?)
                    if(hWallData[i,j] > 0) {
                        drawnObjects.Add(BuildBox((((float)i + 0.5f) * SCALE) + position.x,
                                ((float)j * SCALE) + position.z, SCALE, THICKNESS));
                    }
                }
        }


        public GameObject BuildBox(float x, float z, float width, float length) {
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.localScale = new Vector3(width, heights.y, length);
            box.transform.localPosition = new Vector3(x, heights.x + (heights.y * 0.5f), z);
            return box;
       }




    }

}
