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

        public List<BuildMaterial> walls => wallMaterials;
        public List<BuildMaterial> floors => floorMaterials;
        public List<BuildMaterial> ceilings => ceilingMaterials;

        public BuildMaterial DefaultWall => defaultWall;
        public BuildMaterial DefaultFloor1 => defaultLowerFloor;
        public BuildMaterial DefaultFloor2 => defaultUpperFloor;
        public BuildMaterial DefaultCeiling => defaultCeiling;
    }

}