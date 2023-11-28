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

        private Lot lot;

        private GameObject mapContainer;


        public FloorMap(Vector3 location, int width, int depth, float altitude, float height, Lot parent) {
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
            drawnObjects = new List<GameObject>();
            lot = parent;
            mapContainer = new GameObject();
            mapContainer.transform.parent = lot.transform;
            mapContainer.transform.localPosition = Vector3.zero;
            mapContainer.name = "Architecture";
        }


        public Vector2Int GridPosFromWorldPos(Vector3 worldPos) {
            return new Vector2Int(Mathf.RoundToInt(worldPos.x - position.x), Mathf.RoundToInt(worldPos.z - position.z));
        }


        public void DeleteAllDrawn() {
            for(int i = 0; i < drawnObjects.Count; i++) {
                GameObject.Destroy(drawnObjects[i]);
            }
            drawnObjects.Clear();
            // TODO: Delete the other parts
        }


        public void Redraw() {
            DeleteAllDrawn();
            DrawWalls();
            // TODO: Draw the other parts
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
            box.transform.parent = mapContainer.transform;
            box.name = "Wall Segment";
            return box;
       }


        public void AddComponent(Change change) {
            if(change.operation == BuildOp.ADD) {
                switch (change.type) {
                    case BuildPiece.WALL:
                        AddWall(change);
                        break;
                    case BuildPiece.FLOOR:
                        //TODO
                        break;
                    case  BuildPiece.CEILING:
                        //TODO
                        break;
                    default:
                        //In case more BuildPiece types are added but I forget to include them here
                        Debug.LogError("ERROR! FloorMap.AddComponent(Change) recieved unknown change");
                        break;
                        ChangeStack.Changes.Push(change);
                }
            } else { //TODO!!!
                switch (change.type) {
                    case BuildPiece.WALL:
                        RemoveWall(change);
                        break;
                    case BuildPiece.FLOOR:
                        //TODO
                        break;
                    case  BuildPiece.CEILING:
                        //TODO
                        break;
                    default:
                        //In case more BuildPiece types are added but I forget to include them here
                        Debug.LogError("ERROR! FloorMap.AddComponent(Change) recieved unknown change");
                        break;
                        ChangeStack.Changes.Push(change);
                }
            }
            Redraw();
        }



        public void AddWall(Change change) {
            if(change.start.x == change.end.x) { // Drawing along he Z axis
                if(change.start.y < change.end.y) {
                    for (int i = change.start.y; i < change.end.y; i++) {
                        vWallData[change.start.x, i] = 1;
                    }
                } else {
                    for (int i = change.end.y; i < change.start.y; i++) {
                        vWallData[change.start.x, i] = 1;
                    }
                }
            } else {
                if(change.start.x < change.end.x) {
                    for (int i = change.start.x; i < change.end.x; i++) {
                        hWallData[i, change.start.y] = 1;
                    }
                } else {
                    for (int i = change.end.x; i < change.start.x; i++) {
                        hWallData[i, change.start.y] = 1;
                    }
                }
            }
        }



        public void RemoveWall(Change change) {
            if(change.start.x == change.end.x) { // Drawing along he Z axis
                if(change.start.y < change.end.y) {
                    for (int i = change.start.y; i < change.end.y; i++) {
                        vWallData[change.start.x, i] = 0;
                    }
                } else {
                    for (int i = change.end.y; i < change.start.y; i++) {
                        vWallData[change.start.x, i] = 0;
                    }
                }
            } else {
                if(change.start.x < change.end.x) {
                    for (int i = change.start.x; i < change.end.x; i++) {
                        hWallData[i, change.start.y] = 0;
                    }
                } else {
                    for (int i = change.end.x; i < change.start.x; i++) {
                        hWallData[i, change.start.y] = 0;
                    }
                }
            }
        }




    }

}
