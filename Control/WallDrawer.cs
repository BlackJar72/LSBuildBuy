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
        [SerializeField] BuildController controller;
        [SerializeField] LayerMask groundMask;

        const float thickness = 0.1f;
        const float toEdge = thickness * 0.5f;

        public bool drawing = false;
        private Vector3 startPoint, endPoint;
        private GameObject selectedObject;


        void OnEnable() {
            lotMap = lot.buildMap;
            ACameraControl.LeftMouseUp += EndDrawing;
            ACameraControl.LeftMouseDown += StartDraw;
            ACameraControl.LeftMouseClick += CancelDraw;
            controller.SetLayerMask(groundMask);
        }


        void OnDisable() {
            ACameraControl.LeftMouseUp -= EndDrawing;
            ACameraControl.LeftMouseDown -= StartDraw;
            ACameraControl.LeftMouseClick -= CancelDraw;
            controller.CameraController.ResetLayerMask();
        }


        public void PlaceAtLocation(Transform location, LotMap lot, int story) {
            this.story = story;
            this.lotMap = lot;
            visualizer.transform.position = transform.position = start = end = location.position;
            visualizer.transform.rotation = transform.rotation = Quaternion.identity;
            visualizer.transform.localScale .Set(thickness, lot.Stories[story].heights.y, thickness); // FIXME
        }


        public void Hide() {
            visualizer.SetActive(false);
        }


        public void Remove() {
            Hide();
            gameObject.SetActive(false);
        }


        public Vector3 RoundV3(Vector3 vector) {
            return new Vector3(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y), Mathf.RoundToInt(vector.z));
        }


        public void StartDraw(RaycastHit hit) {
            if(lotMap.IsInMap(hit.point.RoundToInt())) {
                drawing = true;
                startPoint = RoundV3(hit.point);
                selectedObject = controller.CameraController.GetCursorObject();
                controller.SetAAlignMode(true, startPoint);
            }
        }


        public void CancelDraw(RaycastHit hit) {
            //Debug.Log("CancelDraw");
            drawing = false;
            startPoint = hit.point.RoundToInt();
            selectedObject = controller.CameraController.GetCursorObject();
            controller.SetAAlignMode(false, startPoint);
        }


        public void EndDrawing(RaycastHit hit) {
            if (drawing && lotMap.IsInMap(visualizer.transform.position)) {
                endPoint = visualizer.transform.position;
                Vector2Int starting = lotMap.GridPosFromWorldPos(startPoint);
                Vector2Int ending = lotMap.GridPosFromWorldPos(endPoint);
                Change change = new Change(BuildPiece.WALL, BuildOp.ADD, 0, starting, ending, story);
                Debug.Log(change.ToString());
                lotMap.Stories[story].AddComponent(change);
            }
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





    }

}