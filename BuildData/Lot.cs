using System.Collections.Generic;
using System;
using UnityEngine;


namespace BuildBuy {

    public class Lot : MonoBehaviour {
        [SerializeField] public int width;
        [SerializeField] public int depth;
        [SerializeField] public LotMap buildMap;
        [SerializeField] GameObject gridSquare;
        [SerializeField] GameObject lotPlane;
        [SerializeField] LayerMask groundMask;

        private GameObject[,] grid;
        private GameObject buildPlane;
        private GameObject gridContainer;

        public LayerMask GroudMask => groundMask;
        public bool shortWallView = false;

        void Awake() {
            Vector3 location = transform.position;
            gridContainer = new GameObject();
            gridContainer.transform.parent = transform;
            gridContainer.transform.localPosition = Vector3.zero;
            gridContainer.name = "Grid";
            location.x -= (width / 2);
            location.z -= (depth / 2);
            buildMap = new LotMap(location, width, depth, this);
        }


        void Start() {
            buildPlane = GameObject.Instantiate(lotPlane, transform);
            buildPlane.transform.localScale = new Vector3(0.1f * width, 1.0f, 0.1f * depth);
            buildPlane.transform.parent = gridContainer.transform;
            MakeGrid();
        }


        private void MakeGrid() {
            grid = new GameObject[width - 1, depth - 1];
            for(int i = 0; i < width - 1; i++)
                for(int j = 0; j < depth - 1; j++) {
                    grid[i,j] = GameObject.Instantiate(gridSquare, gridContainer.transform);
                    grid[i,j].transform.localPosition = new Vector3(i - (width / 2) + 0.5f, 0.0015f, j - (depth / 2) + 0.5f);
                }
        }



    }

}
