using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BuildBuy {

    /// <summary>
    /// This represents a rectangular section of a room; the room may have one
    /// or more sectors, and this most likely will represent the entire room.
    /// </summary>
    public class RoomSector {

        [SerializeField] Room parent;
        [SerializeField] List<VertexPoint> vertices;
        [SerializeField] List<Wall> walls;



    }
}