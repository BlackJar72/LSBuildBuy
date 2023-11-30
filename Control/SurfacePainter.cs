using System.Collections.Generic;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public class SurfacePainter : MonoBehaviour {

        [SerializeField] GameObject visualizer;
        [SerializeField] Pencil visualFlipper;
        [SerializeField] BuildController controller;

        [SerializeField] LayerMask paintMask;
        [SerializeField] Camera playerEye;

        public Vector3 start;
        public Vector3 end;
        public Lot lot;
        public LotMap lotMap;
        public int story;


        void Update() {
            gameObject.transform.LookAt(playerEye.transform);
        }

    }

}