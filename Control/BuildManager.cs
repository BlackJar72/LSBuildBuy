using System.Collections.Generic;

namespace BuildBuy {

    public class BuildManager {
        /// <summary>
        /// Used for undo function; could there / should there aslo be a re-do function?
        /// Undo alone would treat this like a stack, pushing changes on an pushing them off
        /// when undone.  Allowing redo would involve an integer pointing at an address that
        /// can be moved up and down, with more recent changes only being removed if one is
        /// overwritten by a new change.  However, it might be best o just setup an undo first.
        /// </summary>
        List<Change> changeHistory;
        Change latestChange;


        // TODO: Method to create and add a change.

        public void ApplyLastChange() {
            latestChange.Apply();;
        }
    }
}