using System.Collections.Generic;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public class SurfacePainter : MonoBehaviour {

        [SerializeField] GameObject visualizerEmpty;
        [SerializeField] BuildController controller;

        [SerializeField] LayerMask paintMask;
        [SerializeField] Camera playerEye;

        [SerializeField] Texture2D cursor;

        private readonly Vector2 cursorPos = new Vector2(8, 128);

        public Lot lot;
        public LotMap lotMap;
        public int story;


        void Update() {}


        void OnEnable() {
            Cursor.SetCursor(cursor, cursorPos, CursorMode.Auto);
        }


        void OnDisable() {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

    }

}