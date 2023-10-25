using System.Collections.Generic;

namespace BuildBuy {

    public class Change {
        // FIXME / TODO: A type for this to hold...?  Maybe some better base than ICubeMeshable?
            List<AHousePiece> toAdd;
            List<AHousePiece> toRemove;



        public void Apply() {
            foreach(ICubeMeshable part in toRemove) {
                // TODO: Remove piece
            }
            // FIXME / TODO: Need a modify list, for inserting reference to neighbors (etc.) without having to be
            //               rebuild everything.  Alternately, fix (remove / add / change) reference when
            //               adding or removing pieces, by looking as added/removed pieces reference to modify
            //               referenced pieces (would I need a prebuild of changed parts, though?)....
            foreach (ICubeMeshable part in toAdd) {
                // TODO: add piece
            }
        }




    }
}