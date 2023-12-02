using System.Collections.Generic;
using UnityEngine;

namespace BuildBuy {


    [CreateAssetMenu(menuName = "Build Buy/Material Registry List", fileName = "MaterialList", order = 102)]
    public class SurfaceMatRegistry : ScriptableObject {
        [SerializeField] List<BuildMaterial> wallMaterials;
        [SerializeField] List<BuildMaterial> floorMaterials;
        [SerializeField] List<BuildMaterial> ceilingMaterials;

        [SerializeField] BuildMaterial defaultWall;
        [SerializeField] BuildMaterial defaultLowerFloor;
        [SerializeField] BuildMaterial defaultUpperFloor;
        [SerializeField] BuildMaterial defaultCeiling;

        private List<BuildMaterial> allMaterials;

        public List<BuildMaterial> walls => wallMaterials;
        public List<BuildMaterial> floors => floorMaterials;
        public List<BuildMaterial> ceilings => ceilingMaterials;
        public List<BuildMaterial> all => allMaterials;

        public BuildMaterial DefaultWall => defaultWall;
        public BuildMaterial DefaultFloor1 => defaultLowerFloor;
        public BuildMaterial DefaultFloor2 => defaultUpperFloor;
        public BuildMaterial DefaultCeiling => defaultCeiling;


        void Awake() {
            allMaterials = new List<BuildMaterial>();
            foreach(BuildMaterial mat in wallMaterials) {
                if(!allMaterials.Contains(mat)) allMaterials.Add(mat);
            }
            foreach(BuildMaterial mat in floorMaterials) {
                if(!allMaterials.Contains(mat)) allMaterials.Add(mat);
            }
            foreach(BuildMaterial mat in ceilingMaterials) {
                if(!allMaterials.Contains(mat)) allMaterials.Add(mat);
            }
        }


    }


}