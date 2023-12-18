using System.Collections.Generic;
using UnityEngine;


namespace BuildBuy {


    [CreateAssetMenu(menuName = "Build Buy/Build Item Registry", fileName = "BuildItemRegistry", order = 210)]
    public class BuildItemRegistry : ScriptableObject {
        private bool initialized = false;

        private static List<BuildItem> walls;
        private static List<BuildItem> corners;
        private static List<BuildItem> tiles;
        private static List<BuildItem> floors;
        private static List<BuildItem> ceilings;
        private static List<BuildItem> decor;

        [SerializeField] List<BuildItem> wallsSections;
        [SerializeField] List<BuildItem> cornerObjects;
        [SerializeField] List<BuildItem> tileObjects;
        [SerializeField] List<BuildItem> floorTiles;
        [SerializeField] List<BuildItem> ceilingTiles;
        [SerializeField] List<BuildItem> furniture;

        public List<BuildItem> Walls => walls;
        public List<BuildItem> Corners => corners;
        public List<BuildItem> Tiles => tiles;
        public List<BuildItem> Floors => floors;
        public List<BuildItem> Ceilings => ceilings;
        public List<BuildItem> Decor => decor;

        //Converting ID to index, where ID 0 = nothings, so real objects start at 1
        public BuildItem wall(int i) => walls[i - 1];
        public BuildItem corner(int i) => corners[i - 1];
        public BuildItem tile(int i) => tiles[i - 1];
        public BuildItem floor(int i) => floors[i - 1];
        public BuildItem ceiling(int i) => ceilings[i - 1];
        public BuildItem item(int i) => furniture[i];


        void Awake() {
            CopyWalls();
            CopyWCorners();
            CopyTiles();
            CopyFloorTiles();
            CopyCeilingTiles();
        }

        private void CopyWalls() {
            for(int i = 0; i < wallsSections.Count; i++) {
                if(wallsSections[i] != null) wallsSections[i].SetID(i + 1);
            }
            walls = wallsSections;
        }

        private void CopyWCorners() {
            for(int i = 0; i < cornerObjects.Count; i++) {
                if(cornerObjects[i] != null) cornerObjects[i].SetID(i + 1);
            }
            corners = cornerObjects;
        }

        private void CopyTiles() {
            for(int i = 0; i < tileObjects.Count; i++) {
                if(tileObjects[i] != null) tileObjects[i].SetID(i + 1);
            }
            tiles = tileObjects;
        }

        private void CopyFloorTiles() {
            for(int i = 0; i < floorTiles.Count; i++) {
                if(floorTiles[i] != null) floorTiles[i].SetID(i + 1);
            }
            floors = floorTiles;
        }

        private void CopyCeilingTiles() {;
            for(int i = 0; i <ceilingTiles.Count; i++) {
                if(ceilingTiles[i] != null) ceilingTiles[i].SetID(i + 1);
            }
            ceilings = ceilingTiles;
        }

        private void CopyFurnitureTiles() {;
            for(int i = 0; i <furniture.Count; i++) {
                if(furniture[i] != null) furniture[i].SetID(i + 1);
            }
            decor = furniture;
        }
    }
}