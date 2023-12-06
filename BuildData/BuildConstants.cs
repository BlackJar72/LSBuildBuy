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


    public static class BuildConstants {

        public static readonly float[,] WallMods =
        new float[,]{
            // +Z
            //0       1     2      3      4      5      6     7       8      9      10     11     12     13     14     15
            {0.0f,  0.0f,  0.0f,  0.0f, +0.0f,  0.0f, +0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, -0.5f, -0.5f, -0.5f, -0.5f},  // 0   N-   Z+X-
            {0.0f,  0.0f,  0.0f,  0.0f, +0.0f,  0.0f, -0.5f, -0.5f,  0.0f,  0.0f,  0.0f,  0.0f, +0.5f,  0.0f, -0.5f, -0.5f},  // 1   N+   Z+X+
            // +X
            {0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, +0.0f, +0.5f,  0.0f,  0.0f, -0.5f, -0.5f, -0.5f, -0.5f},  // 2   E-   X+Z-
            {0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, +0.0f, -0.5f,  0.0f, -0.5f, +0.5f, -0.5f,  0.0f, -0.5f},  // 3   E+   X+Z+
            // -Z
            {0.0f, -0.0f,  0.0f, -0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, +0.5f,  0.0f, +0.5f,  0.0f, +0.5f,  0.0f, +0.5f},  // 4   S-   Z-X-
            {0.0f, -0.0f,  0.0f, +0.5f,  0.0f,  0.0f,  0.0f, +0.5f,  0.0f, -0.5f,  0.0f, +0.5f,  0.0f,  0.0f,  0.0f, +0.5f},  // 5   S+   Z-X+
            // -X
            {0.0f,  0.0f, -0.0f, -0.5f,  0.0f,  0.0f, +0.5f, +0.5f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f,  0.0f, +0.5f},  // 6   W-   X-Z-
            {0.0f,  0.0f, -0.0f, +0.5f,  0.0f,  0.0f, -0.5f, +0.5f,  0.0f,  0.0f,  0.0f, +0.5f,  0.0f,  0.0f, +0.5f, +0.5f}   // 7   W+   X-Z+
            //0       1     2      3      4      5      6     7       8      9      10     11     12     13     14     15
        };

    }
}