using System.Collections.Generic;
using System;
using UnityEngine;


namespace BuildBuy {

    [Serializable]
    /// This class represents a section of wall, holding a pair of game objects, one for the mesh and
    /// material on each side of the wall.  These are separate to fascilitate giving each side its own
    /// material.  These meshes are, of course, proceduarl, and should be generated to connect excatly
    /// with neighbors, including factoring in the angles in connectiing segments and their orientation.
    /// They must also factor in wall inserts so as to create any required hole and place any additioal
    /// game objects (e.g., door frames, doors, windows).
    public class WallSection {
        [SerializeField] WallInsert type;
        [SerializeField] GameObject pside; // Side facing the positive direction on the relevant axis
        [SerializeField] GameObject nside; // Side facing the negative direction on the relevant axis
        /// Other game objects attached as part of this segment; this is the specific, instantiate object
        /// existing in game, not its prefab.  For basic wall segments this should be null.
        [SerializeField] GameObject addition;
    }


}