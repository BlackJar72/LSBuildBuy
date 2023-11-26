using UnityEngine;

namespace BuildBuy {

    [System.Serializable]
    public enum BuildPiece {
        WALL,
        FLOOR,
        CEILING
        // More later if needed....
    }


    [System.Serializable]
    public enum BuildOp {
        ADD,
        REMOVE
    }


    // FIXME? Should this be a class or a struct...?
    /// <summary>
    /// A change applied to the building, representing a player instruction to do something.  These are to be applied
    /// as soon as the player does something like placing a wall and also stored in a stack for undo functionality.
    /// The class in immutable as these should never be changed; an undo must undo what was done, not an altered version
    /// of it.  "Instruction," of course, refers to things like drawing or eracing walls through the build mode.
    /// </summary>
    public sealed class Change {
        public readonly BuildPiece type;
        public readonly BuildOp operation;
        public readonly int variant;
        public readonly Vector2Int start, end;
        public readonly int level;


        public Change(BuildPiece Component, BuildOp instruction, int specifier, Vector2Int startPosition, Vector2Int endPosition, int story) {
            type      = Component;
            operation = instruction;
            variant   = specifier;
            start     = startPosition;
            end       = endPosition;
            level     = story;
        }


        public void Apply() {
            // TODO: Apply changes here
        }


        public override string ToString() {
            return "[" + type.ToString() + ", " + operation.ToString() + ", " + variant + ", " + start.ToString() + ", "
                   + end.ToString() + ", " + level + "]";
        }


    }
}