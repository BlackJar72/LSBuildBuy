using System.Collections.Generic;
using UnityEngine;

namespace BuildBuy {


    public class FloorMap {
        private const float SCALE = 1f;       // This might become a variable later, so including now
        private const float THICKNESS = 0.1f; // This might become a variable later, so including now
        private static readonly Vector3 fullWallScale  = new Vector3(1.0f, 1.0f, 1.0f);
        private static readonly Vector3 shortWallScale = new Vector3(1.0f, 0.1f, 1.0f);

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

        /**************************************************************************************************************/
        //                                                                                                            //
        // These game objects are good for organization, but setting them innactive is not a good way to hide objects //
        // one floors that should not bee seen since that would inactivate the scripts as well, turning off all games //
        // logic as though they were not there.  Instead, all game objects that need hiding need to have a special    //
        // hide function that deactivate only the mesh renderer (or skinned mesh renders) so as to make them invisible//
        // while still active in the scene.                                                                           //
        //                                                                                                            //
        // Also, characters will need to be handled separately and differently from stationary objects, as they can   //
        // move between levels and thus must be handled as part of there own update or through similar means.         //
        //                                                                                                            //
        /**************************************************************************************************************/

        private int level;
        private GameObject storyContainer;
        private GameObject mapContainer;
        private GameObject walls;
        private GameObject floors;
        private GameObject ceilings;
        private GameObject miscellaneous;
        private GameObject furniture;
        private GameObject characters;


        public FloorMap(Vector3 location, int width, int depth, float altitude, float height, Lot parent, int story) {
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
            level = story;

            storyContainer = new GameObject();
            storyContainer.transform.parent = lot.transform;
            storyContainer.transform.localPosition = Vector3.zero;
            storyContainer.name = "Level " + level;

            mapContainer = new GameObject();
            mapContainer.transform.parent = storyContainer.transform;
            mapContainer.transform.localPosition = Vector3.zero;
            mapContainer.name = "Architecture";

            walls = new GameObject();
            walls.transform.parent = mapContainer.transform;
            walls.transform.localPosition = Vector3.zero;
            walls.name = "Walls";

            floors = new GameObject();
            floors.transform.parent = mapContainer.transform;
            floors.transform.localPosition = Vector3.zero;
            floors.name = "Floors";

            ceilings = new GameObject();
            ceilings.transform.parent = mapContainer.transform;
            ceilings.transform.localPosition = Vector3.zero;
            ceilings.name = "Ceilings";

            miscellaneous = new GameObject();
            miscellaneous.transform.parent = mapContainer.transform;
            miscellaneous.transform.localPosition = Vector3.zero;
            miscellaneous.name = "Miscellaneous";

            furniture = new GameObject();
            furniture.transform.parent = storyContainer.transform;
            furniture.transform.localPosition = Vector3.zero;
            furniture.name = "Furniture";

            characters = new GameObject();
            characters.transform.parent = storyContainer.transform;
            characters.transform.localPosition = Vector3.zero;
            characters.name = "Characters";
        }


        /// <summary>
        /// This is to be called whenever this floor is shifted too or otherway made visible, by the calling script;
        /// as this is not a game object it will never be called by the engine!
        /// </summary>
        /// <param name="show"></param>
        public void OnEnable() {
            SetWallViewMode();
            ShowLevel(true);
        }


        /// <summary>
        /// This is to be called whenever this floor is made invisible, by the calling script; as this is not a
        /// a game object it will never be called by the engine!
        /// </summary>
        /// <param name="show"></param>
        public void OnDisable() {
            ShowLevel(false);
        }


        /// <summary>
        /// This is to hide or show the level when moving up and down
        /// </summary>
        /// <param name="show"></param>
        public void ShowLevel(bool show) {
            // TODO: Unseen game objects need to have their renderers inactivated, not everything!!!
        }


        public void SetWallViewMode() {
            if(lot.shortWallView) walls.transform.localScale = shortWallScale;
            else walls.transform.localScale = fullWallScale;
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
            DrawFloors();
            // TODO: Draw the other parts
        }


        public void DrawWalls() {
            // "Vertical" (for lack of better name) walls; those parallel to the Z axis
            for(int i = 0; i < lotSizeP1.x; i++)
                for(int j = 0; j < lotSize.y; j++) {
                    //For now, don't worry about extra value (what was I going to do with them anyway?!?)
                    if(vWallData[i,j] > 0) {
                        drawnObjects.Add(BuildWallSegment(((float)i * SCALE) + position.x,
                                (((float)j + 0.5f) * SCALE) + position.z, THICKNESS, SCALE));
                    }
                }
            // Horizontal walls; those parallel to the X axis
            for(int i = 0; i < lotSize.x; i++)
                for(int j = 0; j < lotSizeP1.y; j++) {
                    //For now, don't worry about extra value (what was I going to do with them anyway?!?)
                    if(hWallData[i,j] > 0) {
                        drawnObjects.Add(BuildWallSegment((((float)i + 0.5f) * SCALE) + position.x,
                                ((float)j * SCALE) + position.z, SCALE, THICKNESS));
                    }
                }
        }


        public void DrawFloors() {
            for(int i = 0; i < lotSize.x; i++)
                for(int j = 0; j < lotSize.y; j++) {
                    if(floorData[i,j] > 0) {
                        drawnObjects.Add(BuildFloorSection(((float)i + 0.5f) * SCALE + position.x,
                                (((float)j + 0.5f) * SCALE + position.z), SCALE, SCALE));
                    }
                }
        }


        public GameObject BuildWallSegment(float x, float z, float width, float length) {
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.localScale = new Vector3(width, heights.y, length);
            box.transform.localPosition = new Vector3(x, heights.x + (heights.y * 0.5f), z);
            box.transform.parent = walls.transform;
            box.GetComponent<MeshRenderer>().material = BuildRegistries.Mats.DefaultWall.Mat;
            box.name = "Wall Segment";
            return box;
       }


        public GameObject BuildFloorSection(float x, float z, float width, float length) {
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.localScale = new Vector3(width, 0.1f, length);
            box.transform.localPosition = new Vector3(x, heights.x - 0.049f, z);
            box.transform.parent = floors.transform;
            if(level < 1) {
                box.GetComponent<MeshRenderer>().material = BuildRegistries.Mats.DefaultFloor1.Mat;
            } else {
                box.GetComponent<MeshRenderer>().material = BuildRegistries.Mats.DefaultFloor1.Mat;
            }
            box.name = "Floor Section";
            return box;
        }


        public void AddComponent(Change change) {
            if(change.operation == BuildOp.ADD) {
                switch (change.type) {
                    case BuildPiece.WALL:
                        SetWall(change, 1);
                        break;
                    case BuildPiece.FLOOR:
                        SetFloor(change, 1);
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
                        SetWall(change, 0);
                        break;
                    case BuildPiece.FLOOR:
                        SetFloor(change, 0);
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



        private void SetWall(Change change, int value) {
            if(change.start.x == change.end.x) { // Drawing along he Z axis
                if(change.start.y < change.end.y) {
                    for (int i = change.start.y; i < change.end.y; i++) {
                        vWallData[change.start.x, i] = value;
                    }
                } else {
                    for (int i = change.end.y; i < change.start.y; i++) {
                        vWallData[change.start.x, i] = value;
                    }
                }
            } else {
                if(change.start.x < change.end.x) {
                    for (int i = change.start.x; i < change.end.x; i++) {
                        hWallData[i, change.start.y] = value;
                    }
                } else {
                    for (int i = change.end.x; i < change.start.x; i++) {
                        hWallData[i, change.start.y] = value;
                    }
                }
            }
        }


        private void SetFloor(Change change, int value) {
            if(change.start.x < change.end.x) {
                if(change.start.y < change.end.y) {
                    for(int i = change.start.x; i < change.end.x; i++)
                        for(int j = change.start.y; j < change.end.y; j++) {
                            floorData[i,j] = value;
                        }
                } else {
                    for(int i = change.start.x; i < change.end.x; i++)
                        for(int j = change.end.y; j < change.start.y; j++) {
                            floorData[i,j] = value;
                        }
                }
            } else {
                if(change.start.y < change.end.y) {
                    for(int i = change.end.x; i < change.start.x; i++)
                        for(int j = change.start.y; j < change.end.y; j++) {
                            floorData[i,j] = value;
                        }
                } else {
                    for(int i = change.end.x; i < change.start.x; i++)
                        for(int j = change.end.y; j < change.start.y; j++) {
                            floorData[i,j] = value;
                        }
                }
            }
        }




    }

}
