using kfutils;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public class WallDrawer : MonoBehaviour {
        public Vector3 start;
        public Vector3 end;
        public Story story;

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


        public void PlaceAtLocation(Transform location, Story story) {
            this.story = story;
            visualizer.transform.position = transform.position = start = end = location.position;
            visualizer.transform.rotation =  transform.rotation = Quaternion.identity;
            visualizer.transform.localScale .Set(thickness, story.Height, thickness);
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
            startPoint = hit.point;
            selectedObject = controller.CameraController.GetCursorObject();
            controller.SetAAlignMode(true, startPoint.RoundToInt());
        }


        public void CancelDraw(RaycastHit hit) {
            //Debug.Log("CancelDraw");
            drawing = false;
            startPoint = hit.point;
            selectedObject = controller.CameraController.GetCursorObject();
            controller.SetAAlignMode(false, startPoint);
        }


        public void EndDrawing(RaycastHit hit) {
            //Debug.Log("EndDrawing");
            if(drawing) {
                endPoint = hit.point;
                // FIXME: Only create new vertices when there is none for the point in quesstion!
                GameObject endObject = controller.CameraController.GetCursorObject();
                VertexPoint startVert = new VertexPoint(controller.selectedStart, story);
                VertexPoint endVert = new VertexPoint(controller.LastPointedAt, story);
                Wall wall = new Wall(startVert, endVert, story);
                // FIXME: Only add these if no such vertex is in the list, or better yet see above...
                story.Vertices.Add(startVert);
                story.Vertices.Add(endVert);
                wall.MeshIt(); // FIXME when there is a better, more integrated way to do this.
            }
            drawing = false;
            controller.SetAAlignMode(false, endPoint.RoundToInt());
        }


    }
}