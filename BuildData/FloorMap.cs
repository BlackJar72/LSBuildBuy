using System.Collections.Generic;
using UnityEngine;
using static BuildBuy.BuildConstants;


namespace BuildBuy {


    public struct MapVertex {
        public int itemID; // The ID of whatever feature is positioned there; only one allowed, 0 = nothing
        public DirectionFlags walls; // Flags showing which sides are connected to wall segments
    }


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
        public readonly MapVertex[,] gridVertexData;
        public readonly int[,] tileData;
        public readonly int[,] floorData;
        public readonly int[,] ceilingData;

        private List<GameObject> drawnObjects;

        private Lot lot;

        /**************************************************************************************************************/
        //                                                                                                            //
        // These game objects are good for organization, but setting them innactive is not a good way to hide objects //
        // on floors that should not bee seen since that would inactivate the scripts as well, turning off all games  //
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
            gridVertexData = new MapVertex[width + 1, depth + 1];
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


        /// <summary>
        /// Switch between fullsized and short wall representation, to allow for a through the wall view.
        /// </summary>
        public void SetWallViewMode() {
            if(lot.shortWallView) walls.transform.localScale = shortWallScale;
            else walls.transform.localScale = fullWallScale;
        }


        /// <summary>
        /// Converts a world position to the correct grid coordinate.
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public Vector2Int GridPosFromWorldPos(Vector3 worldPos) {
            return new Vector2Int(Mathf.RoundToInt(worldPos.x - position.x), Mathf.RoundToInt(worldPos.z - position.z));
        }


        /// <summary>
        /// Deletes all the game objects created previously created.
        /// Important as preparation for Redraw().
        /// </summary>
        public void DeleteAllDrawn() {
            for(int i = 0; i < drawnObjects.Count; i++) {
                GameObject.Destroy(drawnObjects[i]);
            }
            drawnObjects.Clear();
            // TODO: Delete the other parts
        }


        /// <summary>
        /// Refresh the visible and physical representation of the floorplan by first deleting the current one
        /// and then rebuilding it from the underlying data structure.  This is essential for updating the floorplan
        /// after changes have been made.
        /// </summary>
        public void Redraw() {
            DeleteAllDrawn();
            DrawWalls();
            DrawFloors();
            // TODO: Draw the other parts
        }


        /// <summary>
        /// Runs through the walls data in the underlying data structure in order to add all walls
        /// (by calling the BuildWallSection() method, and then adds those wall to the collection.
        /// </summary>
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


        /// <summary>
        /// Runs through the floors data in the underlying data structure in order to add all the floors while
        /// adding the to collection.  Works much like DrawWalls(), calling the BuildFloorSection() method for
        /// each section of the grid with a floor.
        /// </summary>
        public void DrawFloors() {
            for(int i = 0; i < lotSize.x; i++)
                for(int j = 0; j < lotSize.y; j++) {
                    if(floorData[i,j] > 0) {
                        drawnObjects.Add(BuildFloorSection(((float)i + 0.5f) * SCALE + position.x,
                                (((float)j + 0.5f) * SCALE + position.z), SCALE, SCALE));
                    }
                }
        }


        /// <summary>
        /// Actually build the a wall section; this is done by simply adding a cube and then scaling and
        /// positioning it appropriately.
        ///
        /// Note, this needs to be replaced and depricated (or removed) with something that:
        /// (1) produces proper corners, and...
        /// (2) has two sides with separate meshes and materials, so that each side can be painted differently.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// FIXME / TODO: Simple boxes (cube primitives) are not an appropriate repressentation for walls beyong proof-of-concept.
        public GameObject BuildWallSegment(float x, float z, float width, float length) {
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.localScale = new Vector3(width, heights.y, length);
            box.transform.localPosition = new Vector3(x, heights.x + (heights.y * 0.5f), z);
            box.transform.parent = walls.transform;
            box.GetComponent<MeshRenderer>().material = BuildRegistries.Mats.DefaultWall.Mat;
            box.name = "Wall Segment";
            return box;
       }


        /// <summary>
        /// Actually create a floor section; this is done by simply adding a cube and then scaling and
        /// positioning it appropriately.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <param name="width"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public GameObject BuildFloorSection(float x, float z, float width, float length) {
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.transform.localScale = new Vector3(width, 0.1f, length);
            box.transform.localPosition = new Vector3(x, heights.x - 0.049f, z);
            box.transform.parent = floors.transform;
            if(level < 1) {
                box.GetComponent<MeshRenderer>().material = BuildRegistries.Mats.DefaultFloor1.Mat;
            } else {
                box.GetComponent<MeshRenderer>().material = BuildRegistries.Mats.DefaultFloor2.Mat;
            }
            box.name = "Floor Section";
            return box;
        }


        /// <summary>
        /// Misleadingly named, this handles all changes by adding, removing, or changing the underlying
        /// data structure based and Change packets created by player action.  Technically, this looks at
        /// the type of change requested and then calls appropriate methods for that change.
        /// </summary>
        /// <param name="change"></param>
        public void AddComponent(Change change) {
            if(change.operation == BuildOp.ADD) {
                switch (change.type) {
                    case BuildPiece.ALL:
                        Debug.LogError("ERROR! ADD should not be used with ALL");
                        break;
                    case BuildPiece.WALL:
                        AddWall(change);
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
                    case BuildPiece.ALL:
                        EraseArea(change);
                        break;
                    case BuildPiece.WALL:
                        RemoveWall(change);
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


        /// <summary>
        /// Removes everything from an area in the underlying data structure
        ///
        /// Actually, this just sorts out the proper beginings and ends from less predictable
        /// player input, and the lets EraseAreaHelper do the real work.
        /// </summary>
        /// <param name="change"></param>
        private void EraseArea(Change change) {
            if(change.start.x < change.end.x) {
                if(change.start.y < change.end.y) {
                    EraseAreaHelper(change.start.x, change.end.x, change.start.y, change.end.y);
                } else {
                    EraseAreaHelper(change.start.x, change.end.x, change.end.y, change.start.y);
                }
            } else {
                if(change.start.y < change.end.y) {
                    EraseAreaHelper(change.end.x, change.start.x, change.start.y, change.end.y);
                } else {
                    EraseAreaHelper(change.end.x, change.start.x, change.end.y, change.start.y);
                }
            }
        }


        /// <summary>
        /// Removes everything from an area in the underlying data structur
        ///
        /// This is a helper that does the real work, so as to avoid a lot of duplicate code inside the if
        /// statements by assume start are always low and ends are always high.
        /// </summary>
        /// <param name="startx"></param>
        /// <param name="endx"></param>
        /// <param name="starty"></param>
        /// <param name="endy"></param>
        private void EraseAreaHelper(int startx, int endx, int starty, int endy) {
            for(int i = startx; i < endx; i++) {
                for(int j = starty; j < endy; j++) {
                    hWallData[i, j] = vWallData[i, j] = floorData[i, j] = 0;
                }
                hWallData[i, endy] = 0;
            }
            for(int j = starty; j < endy; j++) {
                vWallData[endx, j] = 0;
            }
            for(int i = startx + 1; i < endx; i++) {
                for(int j = starty; j < endy; j++) {
                    gridVertexData[i,j].walls  = 0;
                    gridVertexData[i,j].itemID = 0;
                }
                gridVertexData[i, endy].walls &= ~DirectionFlags.S;
                gridVertexData[i, starty].walls &= ~DirectionFlags.N;
                gridVertexData[i, starty].itemID = gridVertexData[i, starty].itemID = 0;
            }
            for(int j = starty; j < endy; j++) {
                gridVertexData[endx, j].walls &= ~DirectionFlags.E;
                gridVertexData[startx, j].walls &= ~DirectionFlags.W;
                gridVertexData[startx, j].itemID = gridVertexData[endx, j].itemID = 0;
            }
        }


        /// <summary>
        /// Adds a wall to the underlying data structure.
        /// </summary>
        /// <param name="change"></param>
        private void AddWall(Change change) {
            if(change.start.x == change.end.x) { // Drawing along he Z axis
                if(change.start.y < change.end.y) {
                    for (int i = change.start.y; i < change.end.y; i++) {
                        vWallData[change.start.x, i] = Mathf.Max(1, vWallData[change.start.x, i]);
                        gridVertexData[change.start.x, i].walls |= DirectionFlags.N;
                        gridVertexData[change.start.x, i + 1].walls |= DirectionFlags.S;
                    }
                } else {
                    for (int i = change.end.y; i < change.start.y; i++) {
                        vWallData[change.start.x, i] = Mathf.Max(1, vWallData[change.start.x, i]);
                        gridVertexData[change.start.x, i].walls |= DirectionFlags.N;
                        gridVertexData[change.start.x, i + 1].walls |= DirectionFlags.S;
                    }
                }
            } else {
                if(change.start.x < change.end.x) {
                    for (int i = change.start.x; i < change.end.x; i++) {
                        hWallData[i, change.start.y] = Mathf.Max(1, vWallData[change.start.x, i]);
                        gridVertexData[change.start.x, i].walls |= DirectionFlags.W;
                        gridVertexData[change.start.x, i + 1].walls |= DirectionFlags.E;
                    }
                } else {
                    for (int i = change.end.x; i < change.start.x; i++) {
                        hWallData[i, change.start.y] = Mathf.Max(1, vWallData[change.start.x, i]);
                        gridVertexData[change.start.x, i].walls |= DirectionFlags.W;
                        gridVertexData[change.start.x, i + 1].walls |= DirectionFlags.E;
                    }
                }
            }
        }


        /// <summary>
        /// Removes a wall from the underlying data structure
        /// </summary>
        /// <param name="change"></param>
        private void RemoveWall(Change change) {
            if(change.start.x == change.end.x) { // Drawing along he Z axis
                if(change.start.y < change.end.y) {
                    for (int i = change.start.y; i < change.end.y; i++) {
                        vWallData[change.start.x, i] = 0;
                        gridVertexData[change.start.x, i].walls &= ~DirectionFlags.N;
                        gridVertexData[change.start.x, i + 1].walls &= ~DirectionFlags.S;
                    }
                } else {
                    for (int i = change.end.y; i < change.start.y; i++) {
                        vWallData[change.start.x, i] = 0;
                        gridVertexData[change.start.x, i].walls &= ~DirectionFlags.N;
                        gridVertexData[change.start.x, i + 1].walls &= ~DirectionFlags.S;
                    }
                }
            } else {
                if(change.start.x < change.end.x) {
                    for (int i = change.start.x; i < change.end.x; i++) {
                        hWallData[i, change.start.y] = 0;
                        gridVertexData[change.start.x, i].walls &= ~DirectionFlags.W;
                        gridVertexData[change.start.x, i + 1].walls &= ~DirectionFlags.E;
                    }
                } else {
                    for (int i = change.end.x; i < change.start.x; i++) {
                        hWallData[i, change.start.y] = 0;
                        gridVertexData[change.start.x, i].walls &= ~DirectionFlags.W;
                        gridVertexData[change.start.x, i + 1].walls &= ~DirectionFlags.E;
                    }
                }
            }
        }



        // Question: Is this even needed for anything now?!?
        /// <summary>
        /// Sets, removes, or modifies a wall in the underlying data structure; this is the old way of handling it.
        /// </summary>
        /// <param name="change"></param>
        /// <param name="value"></param>
        /// FIXME??? Question: Is this even needed for anything now?!? (Probably not...?)
        private void SetWall(Change change, int value) {
            if(change.start.x == change.end.x) { // Drawing along he Z axis
                if(change.start.y < change.end.y) {
                    for (int i = change.start.y; i < change.end.y; i++) {
                        vWallData[change.start.x, i] = value;
                        if(value == 0) {
                            gridVertexData[change.start.x, i].walls &= ~DirectionFlags.N;
                            gridVertexData[change.start.x, i + 1].walls &= ~DirectionFlags.S;
                        } else {
                            gridVertexData[change.start.x, i].walls |= DirectionFlags.N;
                            gridVertexData[change.start.x, i + 1].walls |= DirectionFlags.S;
                        }
                    }
                } else {
                    for (int i = change.end.y; i < change.start.y; i++) {
                        vWallData[change.start.x, i] = value;
                        if(value == 0) {
                            gridVertexData[change.start.x, i].walls &= ~DirectionFlags.N;
                            gridVertexData[change.start.x, i + 1].walls &= ~DirectionFlags.S;
                        } else {
                            gridVertexData[change.start.x, i].walls |= DirectionFlags.N;
                            gridVertexData[change.start.x, i + 1].walls |= DirectionFlags.S;
                        }
                    }
                }
            } else {
                if(change.start.x < change.end.x) {
                    for (int i = change.start.x; i < change.end.x; i++) {
                        hWallData[i, change.start.y] = value;
                        if(value == 0) {
                            gridVertexData[change.start.x, i].walls &= ~DirectionFlags.W;
                            gridVertexData[change.start.x, i + 1].walls &= ~DirectionFlags.E;
                        } else {
                            gridVertexData[change.start.x, i].walls |= DirectionFlags.W;
                            gridVertexData[change.start.x, i + 1].walls |= DirectionFlags.E;
                        }
                    }
                } else {
                    for (int i = change.end.x; i < change.start.x; i++) {
                        hWallData[i, change.start.y] = value;
                        if(value == 0) {
                            gridVertexData[change.start.x, i].walls &= ~DirectionFlags.W;
                            gridVertexData[change.start.x, i + 1].walls &= ~DirectionFlags.E;
                        } else {
                            gridVertexData[change.start.x, i].walls |= DirectionFlags.W;
                            gridVertexData[change.start.x, i + 1].walls |= DirectionFlags.E;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Change the value for a single section of wall.  This is intended to be used for placing doors and
        /// windows, by setting the ID (array int value) to one representing the door or window.
        /// </summary>
        /// <param name="change"></param>
        private void SetWallSection(Change change) {
            // TODO: How do I represent or change this; it doesn't mesh well with the system of I've set up so far.
            // Also, how and where do I take care of placing the accompanying prefab; setting this only would produce
            // a wall section with an appropriate hole, but the actual door or window also needs to be added.
        }


        /// <summary>
        /// Adds a floor to the underlying data structure
        /// </summary>
        /// <param name="change"></param>
        /// <param name="value"></param>
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
