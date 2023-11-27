using System.Collections.Generic;
using UnityEngine;


namespace BuildBuy {

    public class Lot : MonoBehaviour {
        [SerializeField] public int width;
        [SerializeField] public int depth;
        [SerializeField] public LotMap buildMap;
        [SerializeField] GameObject gridSquare;

        private GameObject[,] grid;

        void Awake() {
            Vector3 location = transform.position;
            location.x -= (width / 2);
            location.z -= (depth / 2);
            buildMap = new LotMap(location, width, depth);
        }


        void Start() {
            MakeGrid();
        }


        private void MakeGrid() {
            grid = new GameObject[width - 1, depth - 1];
            for(int i = 0; i < width - 1; i++)
                for(int j = 0; j < depth - 1; j++) {
                    grid[i,j] = GameObject.Instantiate(gridSquare, transform);
                    grid[i,j].transform.localPosition = new Vector3(i - (width / 2) + 0.5f, 0.001f, j - (depth / 2) + 0.5f);
                }
        }



    }

}