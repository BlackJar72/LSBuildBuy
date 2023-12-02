using kfutils;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public class WallDrawer : MonoBehaviour {
        public Vector3 start;
        public Vector3 end;
        public Lot lot;
        public LotMap lotMap;
        public int story;

        [SerializeField] GameObject visualizer;
        [SerializeField] Pencil visualFlipper;
        [SerializeField] BuildController controller;

        [SerializeField] LayerMask groundMask;

        const float thickness = 0.1f;
        const float toEdge = thickness * 0.5f;

        public bool drawing = false;
        private Vector3 startPoint, endPoint;
        private GameObject selectedObject;

        public bool eraseMode;
        private IDrawAction[] draws;
        private int currentDraw = 0;


        void Awake() {
            draws = new IDrawAction[3];
            draws[0] = new DrawRoom();
            draws[1] = new DrawWall();
            draws[2] = new DrawFloor();
        }


        void OnEnable() {
            lotMap = lot.buildMap;
            groundMask = lot.GroudMask;
            ACameraControl.LeftMouseUp += EndDrawing;
            ACameraControl.LeftMouseDown += StartDraw;
            ACameraControl.LeftMouseClick += CancelDraw;
            controller.SetLayerMask(groundMask);
            visualizer.SetActive(true);
        }


        void OnDisable() {
            ACameraControl.LeftMouseUp -= EndDrawing;
            ACameraControl.LeftMouseDown -= StartDraw;
            ACameraControl.LeftMouseClick -= CancelDraw;
            controller.CameraController.ResetLayerMask();
            visualizer.SetActive(false);
        }


        void Update() {
            if(Input.GetKeyUp(KeyCode.T)) {
                ToggleEraseMode();
            }
            if(Input.GetKeyUp(KeyCode.F)) {
                currentDraw = (currentDraw + 1) % draws.Length;
                draws[currentDraw].OnSelected(this);
            }
        }


        public void PlaceAtLocation(Transform location, LotMap lot, int story) {
            this.story = story;
            this.lotMap = lot;
            visualizer.transform.position = transform.position = start = end = location.position;
            visualizer.transform.rotation = transform.rotation = Quaternion.identity;
            visualizer.transform.localScale.Set(thickness, lot.Stories[story].heights.y, thickness); // FIXME
        }


        public void Hide() {
            visualizer.SetActive(false);
        }


        public void Remove() {
            Hide();
            gameObject.SetActive(false);
        }


        // This is probably not needed as Vector3.RoundToInt() does what I want after all.
        public static Vector3 RoundV3(Vector3 vector) {
            return new Vector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
        }


        public void StartDraw(RaycastHit hit) {
            if(visualizer.activeSelf && lotMap.IsInMap(hit.point.RoundToInt())) {
                draws[currentDraw].StartDraw(hit, this);
            }
        }


        public void CancelDraw(RaycastHit hit) {
            draws[currentDraw].CancelDraw(hit, this);
        }


        public void EndDrawing(RaycastHit hit) {
            if (visualizer.activeSelf && drawing && lotMap.IsInMap(visualizer.transform.position)) {
                draws[currentDraw].EndDrawing(hit, this);
            } else CancelDraw(hit);
            drawing = false;
            controller.SetAAlignMode(false, endPoint);
        }


        // FIXME: Does nothing (probably undone elsewhere)
        public void ShowVisualizer() {
            Cursor.visible = false;
            visualizer.SetActive(true);
        }


        // FIXME: Does nothing (probably undone elsewhere)
        public void HideVisualizer() {
            Cursor.visible = true;
            visualizer.SetActive(false);
        }


        public void ToggleEraseMode() {
            if(visualFlipper.Flip()) {
                eraseMode = !eraseMode;
            }
        }


        public void SetBuildMode(int mode) {
            currentDraw = mode;
        }


        public void SetEraseMode(bool mode) {
            if(mode != eraseMode) visualFlipper.Flip();
            eraseMode = mode;
        }




        #region Draw Actions
        /**************************************************************************************************************/
        /*                              DRAW ACTIONS                                                                  */
        /**************************************************************************************************************/
        // Yes, I'm abriviating "parent" (containing object) to just "p"; if your someone else reading this somehow,
        // some day, deal with it.  I know its not the "clean code" ideal of verbose descriptiveness, but its much
        // less of a pain in the butt.

        private interface IDrawAction {
            void OnSelected(WallDrawer p);
            void StartDraw(RaycastHit hit, WallDrawer p);
            void CancelDraw(RaycastHit hit, WallDrawer p);
            void EndDrawing(RaycastHit hit, WallDrawer p);
        }

        /// <summary
        /// A base calss for other draw actions
        /// </summary>
        private class ADrawLinear : IDrawAction {

            public virtual void OnSelected(WallDrawer p) {
                p.drawing = false;
                p.controller.SetAAlignMode(false, p.startPoint);
            }

            public virtual void StartDraw(RaycastHit hit, WallDrawer p) {
                p.drawing = true;
                p.startPoint = RoundV3(hit.point);
                p.selectedObject = p.controller.CameraController.GetCursorObject();
                p.controller.SetAAlignMode(true, p.startPoint);
            }

            public virtual void CancelDraw(RaycastHit hit, WallDrawer p) {
                //Debug.Log("CancelDraw");
                p.drawing = false;
                p.startPoint = hit.point.RoundToInt();
                p.selectedObject = p.controller.CameraController.GetCursorObject();
                p.controller.SetAAlignMode(false, p.startPoint);

            }

            public virtual void EndDrawing(RaycastHit hit, WallDrawer p) {
                CancelDraw(hit, p);
            }
        }

        /// <summary
        /// A base calss for other draw actions
        /// </summary>
        private class ADrawArea : IDrawAction {

            public virtual void OnSelected(WallDrawer p) {
                p.drawing = false;
                p.controller.SetAAlignMode(false, p.startPoint);
            }

            public virtual void StartDraw(RaycastHit hit, WallDrawer p) {
                p.drawing = true;
                p.startPoint = RoundV3(hit.point);
                p.selectedObject = p.controller.CameraController.GetCursorObject();
                p.controller.SetAAlignMode(false, p.startPoint);
            }

            public virtual void CancelDraw(RaycastHit hit, WallDrawer p) {
                //Debug.Log("CancelDraw");
                p.drawing = false;
                p.startPoint = hit.point.RoundToInt();
                p.selectedObject = p.controller.CameraController.GetCursorObject();
                p.controller.SetAAlignMode(false, p.startPoint);

            }

            public virtual void EndDrawing(RaycastHit hit, WallDrawer p) {
                CancelDraw(hit, p);
            }
        }


        /// <summary>
        /// For drawing walls
        /// </summary>
        private class DrawWall : ADrawLinear {
            public override void EndDrawing(RaycastHit hit, WallDrawer p) {
                p.endPoint = p.visualizer.transform.position;
                Vector2Int starting = p.lotMap.GridPosFromWorldPos(p.startPoint);
                Vector2Int ending = p.lotMap.GridPosFromWorldPos(p.endPoint);
                Change change;
                if(p.eraseMode) {
                    change = new Change(BuildPiece.WALL, BuildOp.REMOVE, 0, starting, ending, p.story);
                } else {
                    change = new Change(BuildPiece.WALL, BuildOp.ADD, 0, starting, ending, p.story);
                }
                //Debug.Log(change.ToString());
                p.lotMap.Stories[p.story].AddComponent(change);
            }
        }


        private class DrawRoom : ADrawArea {
            private DrawFloor floor = new DrawFloor();

            public override void EndDrawing(RaycastHit hit, WallDrawer p) {
                p.endPoint = p.visualizer.transform.position;
                Vector2Int starting = p.lotMap.GridPosFromWorldPos(p.startPoint);
                Vector2Int ending = p.lotMap.GridPosFromWorldPos(p.endPoint);
                Vector2Int c1 = new Vector2Int(starting.x, ending.y);
                Vector2Int c2 = new Vector2Int(ending.x, starting.y);
                if (p.eraseMode) {
                    Change change = new Change(BuildPiece.ALL, BuildOp.REMOVE, 0, starting, ending, p.story);
                    p.lotMap.Stories[p.story].AddComponent(change);
                } else {
                    DrawOneWall(starting, c1, p);
                    DrawOneWall(starting, c2, p);
                    DrawOneWall(c1, ending, p);
                    DrawOneWall(c2, ending, p);
                    floor.EndDrawing(hit, p);
                }
            }

            private void DrawOneWall(Vector2Int starting, Vector2Int ending, WallDrawer p) {
                Change change = new Change(BuildPiece.WALL, BuildOp.ADD, 0, starting, ending, p.story);
                p.lotMap.Stories[p.story].AddComponent(change);
            }
        }


        private class DrawFloor : ADrawArea {
            public override void EndDrawing(RaycastHit hit, WallDrawer p) {
                p.endPoint = p.visualizer.transform.position;
                Vector2Int starting = p.lotMap.GridPosFromWorldPos(p.startPoint);
                Vector2Int ending = p.lotMap.GridPosFromWorldPos(p.endPoint);
                Change change;
                if(p.eraseMode) {
                    change = new Change(BuildPiece.FLOOR, BuildOp.REMOVE, 0, starting, ending, p.story);
                } else {
                    change = new Change(BuildPiece.FLOOR, BuildOp.ADD, 0, starting, ending, p.story);
                }
                //Debug.Log(change.ToString());
                p.lotMap.Stories[p.story].AddComponent(change);
            }
        }

        #endregion

    }

}