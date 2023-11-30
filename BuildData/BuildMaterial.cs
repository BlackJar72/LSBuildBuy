using System;
using UnityEngine;

namespace BuildBuy {

    [CreateAssetMenu(menuName = "Build Buy/Build Material", fileName = "BuildMaterial", order = 101)]
    public class BuildMaterial : ScriptableObject {
        [SerializeField] Material material;
        [SerializeField] string name;
        [SerializeField] SurfaceType surfaceType;

        public Material Mat => material;
        public string Name => name;
        public SurfaceType Surface => surfaceType;
    }


    [Flags][Serializable]
    public enum SurfaceType {
        Wall = 1,
        Floor = 2,
        Ceiling = 4
    }

}