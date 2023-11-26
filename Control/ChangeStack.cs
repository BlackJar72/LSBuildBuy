using System.Collections.Generic;
using SimCam;
using UnityEngine;

namespace BuildBuy {

    public class ChangeStack {
        private static Stack<Change> changes;

        static ChangeStack() {
            changes = new Stack<Change>();
        }

        public static Stack<Change> Changes => changes;

    }
}