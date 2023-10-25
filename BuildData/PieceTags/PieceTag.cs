using kfutils;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public enum PieceType {
        vertex = 0,
        wall = 1,
        segment = 2,
        sector = 3,
        room = 4,
        story = 5,
        structure = 6
    }


    public class PieceTag : MonoBehaviour {
        [SerializeField] private AHousePiece data;
        [SerializeField] private PieceType type;

        public AHousePiece Data => data;
        public PieceType Type => type;

        public void init(AHousePiece creator, PieceType pieceType) {
            data = creator;
            type = pieceType;
        }

    }
}