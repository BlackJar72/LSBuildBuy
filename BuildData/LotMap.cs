using System;
using System.Collections.Generic;
using UnityEngine;


namespace BuildBuy {


    [System.Serializable]
    public enum Directions {
        N = 0,
        E = 1,
        S = 2,
        W = 3
    }


    [System.Serializable][Flags]
    public enum DirectionFlags {
        N = 1,
        E = 2,
        S = 4,
        W = 8
    }


    [System.Serializable]
    public class LotMap  {
        private List<FloorMap> stories;

        public static readonly float[,] WallMods =
            new float[,]{
                // +Z
                //0       1     2      3      4      5      6     7       8      9      10     11     12     13     14     15
                {0.0f,  0.0f,  0.0f,  0.0f, +0.0f,  0.0f, +0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f, -0.5f, -0.5f, -0.5f},  // 0   N-   Z+X-
                {0.0f,  0.0f,  0.0f,  0.0f, +0.0f,  0.0f, -0.5f, -0.5f,  0.0f,  0.0f,  0.0f,  0.0f, +0.5f,  0.0f, -0.5f, -0.5f},  // 1   N+   Z+X+
                // +X
                {0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, +0.0f, +0.5f,  0.0f,  0.0f, -0.5f, -0.5f, -0.5f, -0.5f},  // 2   E-   X+Z-
                {0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, +0.0f, -0.5f,  0.0f, -0.05, +0.5f, -0.5f,  0.0f, -0.5f},  // 3   E+   X+Z+
                // -Z
                {0.0f, -0.0f,  0.0f, -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, +0.5f,  0.0f, +0.5f,  0.0f, +0.5f,  0.0f, +0.5f},  // 4   S-   Z-X-
                {0.0f, -0.0f,  0.0f, +0.5f,  0.0f,  0.0f,  0.0f, +0.5f,  0.0f, -0.5f,  0.0f, +0.5f,  0.0f,  0.0f,  0.0f, +0.5f},  // 5   S+   Z-X+
                // -X
                {0.0f,  0.0f, -0.0f, -0.5f,  0.0f,  0.0f, +0.5f, +0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, +0.5f},  // 6   W-   X-Z-
                {0.0f,  0.0f, -0.0f, +0.5f,  0.0f,  0.0f, -0.5f, +0.5f,  0.0f,  0.0f,  0.0f, +0.5f,  0.0f,  0.0f, +0.5f, +0.5f}   // 7   W+   X-Z+
                //0       1     2      3      4      5      6     7       8      9      10     11     12     13     14     15
            };

        [SerializeField] Vector3 location;
        [SerializeField] int width;
        [SerializeField] int depth;
        [SerializeField] int numStories;

        public List<FloorMap> Stories => stories;

        private Lot lot;
        private int currentStory = 0;

        public Vector3 Location => location;
        public int Width => width;
        public int Depth => depth;
        public int NumStories => numStories;

        public int CurStory => currentStory;


        public LotMap(Vector3 position, int xsize, int zsize, int ysize, Lot parent) {
            stories = new List<FloorMap>();
            location = position;
            width = xsize;
            depth = zsize;
            numStories = ysize;
            lot = parent;
            for(int i = 0; i < numStories; i++) AddStory();
        }


        public Vector2Int GridPosFromWorldPos(Vector3 worldPos, int adjustment = 0) {
            return new Vector2Int(Mathf.RoundToInt(worldPos.x - location.x + adjustment),
                    Mathf.RoundToInt(worldPos.z - location.z + adjustment));
        }


        public bool IsInMap(Vector3 worldPos) {
            Vector2Int p = GridPosFromWorldPos(worldPos);
            return ((p.x > -1) && (p.x < width) && (p.y > -1) && (p.y < depth));
        }


        public bool IsInMap(Vector2Int p) => ((p.x > -1) && (p.x < width) && (p.y < -1) && (p.y < depth));


        public void AddStory(float height = 3) {
            int level = stories.Count;
            FloorMap map;
            if(level == 0) {
                map = new FloorMap(location, width, depth, location.y, height, lot, 0);
            } else {
                FloorMap last = Stories[Stories.Count - 1];
                float altitude = last.heights.x + last.heights.y;
                map = new FloorMap(location, width, depth, altitude, height, lot, Stories.Count);
            }
            Stories.Add(map);
        }


        public void SetStory(int level) {
            currentStory = Mathf.Clamp(level, 0, numStories - 1);
        }


    }
}