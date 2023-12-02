using System.Collections.Generic;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public class SurfacePainter : MonoBehaviour {

        [SerializeField] GameObject visualizer;
        [SerializeField] BuildController controller;

        [SerializeField] LayerMask paintMask;
        [SerializeField] Camera playerEye;

        public Lot lot;
        public LotMap lotMap;
        public int story;


        void Update() {
            gameObject.transform.LookAt(playerEye.transform);
        }

    }

}