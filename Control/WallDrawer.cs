using kfutils;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public class WallDrawer : MonoBehaviour {
        public Vector3 start;
        public Vector3 end;
        public LotMap lot;
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
            this.lot = lot;
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


        public void StartDraw(RaycastHit hit) {
            //Debug.Log("StartDraw");
            drawing = true;
            startPoint = hit.point.RoundToInt();
            selectedObject = controller.CameraController.GetCursorObject();
            controller.SetAAlignMode(true, startPoint);
        }


        public void CancelDraw(RaycastHit hit) {
            //Debug.Log("CancelDraw");
            drawing = false;
            startPoint = hit.point.RoundToInt();
            selectedObject = controller.CameraController.GetCursorObject();
            controller.SetAAlignMode(false, startPoint);
        }


        public void EndDrawing(RaycastHit hit) {
            //Debug.Log("EndDrawing");
            if (drawing) {
                endPoint = visualizer.transform.position;
                Vector2Int starting = new Vector2Int((int)startPoint.x, (int)startPoint.z);
                Vector2Int ending = new Vector2Int((int)endPoint.x, (int)endPoint.z);
                Change change = new Change(BuildPiece.WALL, BuildOp.ADD, 0, starting, ending, story);
                Debug.Log(change.ToString());
            }
            drawing = false;
            controller.SetAAlignMode(false, endPoint);
        }


    }

}