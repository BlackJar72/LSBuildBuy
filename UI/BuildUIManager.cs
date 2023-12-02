using System.Collections.Generic;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public class BuildUIManager : MonoBehaviour {
        [SerializeField] GameObject drawUI;
        [SerializeField] GameObject paintUI;


        void Start() {

        }


        public void SetDrawMode() {
            drawUI.SetActive(true);
            paintUI.SetActive(false);
        }


        public void SetPaintMode() {
            drawUI.SetActive(false);
            paintUI.SetActive(true);
        }

    }

}