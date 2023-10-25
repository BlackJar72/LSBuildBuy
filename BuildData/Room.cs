using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BuildBuy {

    /// <summary>
    /// This represents a conceptual room, and is made up of one or more RoomSectors; rectangular rooms
    /// would be made of one sector while more complex shapes would have several.  The number of sectors should
    /// usually be the least needed to represent the rooms shape, though some unoptomized sector arrangements
    /// resulting from how the room is build by the player is acceptable; however, no rectangle should be
    /// gratuitously broken up in normal use.
    /// </summary>
    public class Room {
        private List<RoomSector> sectors;

    }
}