using UnityEditor;
using UnityEngine;


namespace BuildBuy {


    public enum BuildItemType {
        wallSegment = 0,  // Walls
        gridVertex = 1,   // Wall Corners & decorations (including pillars placed there)
        tileItem = 2,     // Build items (not buy items) placed in tiles (notable pillars)
        floorTile = 3,
        ceilingTile = 4
    }


    [CreateAssetMenu(menuName = "Build Buy/Build Item", fileName = "BuildItem", order = 201)]
    public class BuildItem : ScriptableObject {
        [SerializeField] private int id;
        [SerializeField] private BuildItemType type;
        [SerializeField] private GameObject inWorld;

        public int ID => id;
        public BuildItemType Type => type;
        public GameObject InWorld => inWorld;

        public void SetID(int newID) {
            id = newID;
        }
    }

}
