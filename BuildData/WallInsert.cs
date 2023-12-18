using System.Collections.Generic;
using System;
using UnityEngine;


namespace BuildBuy {

    [Serializable]
    public struct HolePart {
        [SerializeField] float x;  // Distance from start in the positive direction along its length; could be on X or Z axis
        [SerializeField] float y1; // Y coordinate for the bottom of this insertion, measured from the bottom of the wall segment
        [SerializeField] float y2; // Y coordinate for the top of this insertion hole, measured from the top of the wall segment
        public float X => x;
        public float Y1 => y1;
        public float Y2 => y2;
        public HolePart(float x, float y1, float y2) { this.x = x; this.y1 = y1; this.y2 = y2; }
    }


    [Serializable]
    public struct InsertPrefab {
        [SerializeField] GameObject prefab;
        [SerializeField] Vector2 position;
        public GameObject Prefab => prefab;
        public Vector2 Position => position;
    }


    [CreateAssetMenu(menuName = "Build Buy/Wall Insert", fileName = "WallInsert", order = 202)]
    public class WallInsert : BuildItem {
        /// <summary>
        /// Measures for the location of the hole; there mmust be at least 2, for the start and end (or none at all).
        /// </summary>
        [SerializeField] HolePart[] holeMeasures;
        [SerializeField] Vector2 position;

        public HolePart[] measures => holeMeasures;
        public Vector2 Position => position;

        public HolePart Measure(int index) => measures[index];
    }


}