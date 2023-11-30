using System.Collections.Generic;
using UnityEngine;

namespace BuildBuy {


    [CreateAssetMenu(menuName = "Build Buy/Build Material", fileName = "BuildMaterial", order = 102)]
    public class SurfaceMatRegistry : ScriptableObject {
        [SerializeField] List<BuildMaterial> wallMaterials;
        [SerializeField] List<BuildMaterial> floorMaterials;
        [SerializeField] List<BuildMaterial> ceilingMaterials;

        public List<BuildMaterial> walls => wallMaterials;
        public List<BuildMaterial> floors => floorMaterials;
        public List<BuildMaterial> ceilings => ceilingMaterials;
    }

}