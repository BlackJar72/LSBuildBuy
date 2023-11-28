using System.Collections;
using System.Collections.Generic;
using kfutils;
using SimCam;
using UnityEngine;


namespace BuildBuy {


    public class Pencil : MonoBehaviour {
        [SerializeField] Quaternion initialRotation;
        [SerializeField] Quaternion flipRotation;
        private bool flipped = false;
        private bool moving = false;
        private float startT, t;
        [SerializeField] float timeToTurn;


        public bool Flip() {
            if(moving) return false;
            else {
                //moving = true;
                startT = Time.fixedTime;
                if(flipped) {
                    //StartCoroutine(FlipUp());
                    transform.localRotation = initialRotation;
                } else {
                    //StartCoroutine(FlipDown());
                    transform.localRotation = flipRotation;
                }
                flipped = !flipped;
                return true;
            }
        }


        // FIXME: Why TF is this not working?!  Why do these jump to the final position? Why is Slerp broken?!?!?
        private IEnumerator FlipUp() {
            while (moving) {
                yield return new WaitForFixedUpdate();
                t = Mathf.Clamp((Time.fixedTime - startT) / timeToTurn, 0f, 1f);
                transform.localRotation = Quaternion.Slerp(flipRotation, initialRotation, t);
                moving = (t < 1f);
            }
        }


        private IEnumerator FlipDown() {
            while (moving) {
                yield return new WaitForFixedUpdate();
                t = Mathf.Clamp((Time.fixedTime - startT) / timeToTurn, 0f, 1f);
                transform.localRotation = Quaternion.Slerp(initialRotation, flipRotation, t);
                moving = (t < 1f);
            }
        }

    }

}
