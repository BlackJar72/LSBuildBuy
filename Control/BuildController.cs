using kfutils;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public class BuildController : MonoBehaviour {
        // FIXME / TODO: Replace this with the my simulation camera controller
        [SerializeField] Camera playerEye;
        [SerializeField] GameObject visualRepresentation;

        [SerializeField] EventConverter eventConverter;
        [SerializeField] ModeSwitch modeSwitch;
        [SerializeField] ACameraControl cameraController;

        public Vector2 realPosition;
        public Vector2Int gridPosition;

        public LayerMask groundPlainMask;

        private Vector3? pointedAt;
        private Vector3 lastPointedAt;
        private bool beActive = false, wasActive = false;

        private bool aalignMode = false;
        public Vector3 selectedStart;

        public Vector3 LastPointedAt => lastPointedAt;

        public Vector3 SelectedStart => selectedStart;

        public ACameraControl CameraController => cameraController;

        // Start is called before the first frame update
        void Start() {
            // FIXME / TODO: Set this differently, and set transform based on this + story altitude
            realPosition = new Vector2(transform.position.x, transform.position.z);
            gridPosition = new Vector2Int(Mathf.RoundToInt(realPosition.x), Mathf.RoundToInt(realPosition.y));
            visualRepresentation.SetActive(modeSwitch.CurrentMode.GetCursorLocation() != null);
        }


        // Update is called once per frame
        void Update() {
            if (aalignMode) MoveAALign();
            else MoveNormal();
        }


        private void MoveNormal() {
            pointedAt = modeSwitch.CurrentMode.GetCursorLocation();
            beActive = (pointedAt != null);
            if (beActive) {
                lastPointedAt = pointedAt.Value;
                //Debug.Log(pointedAt);
                realPosition.Set(lastPointedAt.x, lastPointedAt.z);
                gridPosition.Set(Mathf.RoundToInt(lastPointedAt.x), Mathf.RoundToInt(lastPointedAt.z));
                //lastPointedAt.x = gridPosition.x; lastPointedAt.z = gridPosition.y;
                transform.position = lastPointedAt;
                visualRepresentation.SetActive(true);
            }
            if (beActive != wasActive) visualRepresentation.SetActive(beActive);
            wasActive = beActive;

        }


        /// <summary>
        /// FUCK OF THE FUCK OF THE BUTFUCKED FUCK!!!
        /// </summary>
            // FUCK OF THE FUCK OF THE BUTTFUCKED FUCK!!!
        private void MoveAALign() {
            pointedAt = modeSwitch.CurrentMode.GetCursorLocation();
            beActive = (pointedAt != null);
            if (beActive) {
                lastPointedAt = pointedAt.Value;
                if (Mathf.Abs(lastPointedAt.x - selectedStart.x) < Mathf.Abs(lastPointedAt.z - selectedStart.z)) {
                    lastPointedAt.x = selectedStart.x;
                } else {
                    lastPointedAt.z = selectedStart.z;
                }
                realPosition.Set(lastPointedAt.x, lastPointedAt.z);
                gridPosition.Set(Mathf.RoundToInt(lastPointedAt.x), Mathf.RoundToInt(lastPointedAt.z));
                transform.position = lastPointedAt;
                visualRepresentation.SetActive(true);
            }
            if (beActive != wasActive) visualRepresentation.SetActive(beActive);
            wasActive = beActive;
        }


        void OnEnable() {
            ModeSwitch.modeChanged += SwitchCamMode;
        }


        void OnDisable() {
            ModeSwitch.modeChanged -= SwitchCamMode;
        }


        private void SwitchCamMode(ACameraControl mode) {
            cameraController = mode;
        }


        public void SetAAlignMode(bool aalign, Vector3 start) {
            aalignMode = aalign;
            selectedStart = start;
        }


        public void SetLayerMask(LayerMask mask) {
            cameraController.SetLayerMask(mask);
        }
    }

}
