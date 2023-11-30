using System.Collections.Generic;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    /// <summary>
    /// A storehouse for things like materials that are used in many places
    /// </summary>
    public class BuildRegistries : MonoBehaviour {
        public static SurfaceMatRegistry mats;
        [SerializeField] SurfaceMatRegistry materials;



        void Awake() {
            // Make the current globally available material palette that of the current painter
            // (which should be unique anyway).
            mats = materials;
        }

    }
}