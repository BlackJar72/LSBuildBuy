using System.Collections.Generic;
using UnityEngine;


namespace BuildBuy {

    public class LotMap {
        private List<FloorMap> stories;

        private Vector3 location;
        private int width, depth;


        public List<FloorMap> Stories => stories;


        public LotMap(Vector3 position, int xsize, int zsize) {
            stories = new List<FloorMap>();
            location = position;
            width = xsize;
            depth = zsize;
        }

    }
}