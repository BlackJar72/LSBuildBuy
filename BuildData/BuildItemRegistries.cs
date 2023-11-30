using System.Collections.Generic;
using UnityEngine;


namespace BuildBuy {

    public class BuildItemRegistries {
        private bool initialized = false;
        public static List<BuildItem> walls = new List<BuildItem>();
        public static List<BuildItem> corners = new List<BuildItem>();
        public static List<BuildItem> tiles = new List<BuildItem>();
        public static List<BuildItem> floorTiles = new List<BuildItem>();
        public static List<BuildItem> ceilingTiles = new List<BuildItem>();


        public void init(BuildItemRegistry source) {
            if(!initialized) {
                source.CopyWalls(walls);
                source.CopyWCorners(corners);
                source.CopyTiles(tiles);
                source.CopyFloorTiles(floorTiles);
                source.CopyCeilingTiles(ceilingTiles);
                initialized = true;
            }
        }
    }


    ///
    /// This to act as an in-editor initialization file, that is, as a place to put BuildItems which are also
    /// created in the editor.  There should really normally be only one of these, attatched to a and called by
    /// a loading manager in the start scene, from which is should be run once.
    ///
    [CreateAssetMenu(menuName = "Build Buy/Build Item Registry", fileName = "BuildItemRegistry", order = 210)]
    public class BuildItemRegistry : ScriptableObject {
        [SerializeField] List<BuildItem> walls;
        [SerializeField] List<BuildItem> corners;
        [SerializeField] List<BuildItem> tiles;
        [SerializeField] List<BuildItem> floorTiles;
        [SerializeField] List<BuildItem> ceilingTiles;

        public void CopyWalls(List<BuildItem> global) {
            for(int i = 0; i < walls.Count; i++) global[i] = walls[i];
            for(int i = 0; i < global.Count; i++) {
                if(global[i] != null) global[i].SetID(i);
            }
        }

        public void CopyWCorners(List<BuildItem> global) {
            for(int i = 0; i < corners.Count; i++) global[i] = corners[i];
            for(int i = 0; i < global.Count; i++) {
                if(global[i] != null) global[i].SetID(i);
            }
        }

        public void CopyTiles(List<BuildItem> global) {
            for(int i = 0; i < tiles.Count; i++) global[i] = tiles[i];
            for(int i = 0; i < global.Count; i++) {
                if(global[i] != null) global[i].SetID(i);
            }
        }

        public void CopyFloorTiles(List<BuildItem> global) {
            for(int i = 0; i < floorTiles.Count; i++) global[i] = floorTiles[i];
            for(int i = 0; i < global.Count; i++) {
                if(global[i] != null) global[i].SetID(i);
            }
        }

        public void CopyCeilingTiles(List<BuildItem> global) {
            for(int i = 0; i < ceilingTiles.Count; i++) global[i] = ceilingTiles[i];
            for(int i = 0; i < global.Count; i++) {
                if(global[i] != null) global[i].SetID(i);
            }
        }
    }
}