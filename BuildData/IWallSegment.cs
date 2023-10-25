using System;
using UnityEngine;

namespace BuildBuy {


    [Flags]
    public enum WallSegmentPosition {
        middle = 0, // neither start nor end
        start = 1,
        end = 2,
        whole = start & end // aka, start + end, aka 3
    }


    public interface IWallSegment : ICubeMeshable {

        Vector3 StartPoint { get; set; }
        Vector3 EndPoint { get; set; }
        Wall Parent { get; set; }
        WallSegmentPosition Position { get; set; }

    }
}
