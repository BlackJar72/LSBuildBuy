using UnityEngine;
using UnityEngine.UI;

namespace BuildBuy {

    public class ButtonToggler : MonoBehaviour {
        [SerializeField] Texture2D unselectedImage;
        [SerializeField] Texture2D selectedImage;
        [SerializeField] bool isSelected;

        private Sprite unselected;
        private Sprite selected;


        void Awake() {
            selected = Sprite.Create(selectedImage, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));
            unselected = Sprite.Create(unselectedImage, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));
        }


        void Start() {
            if(isSelected) SetSelected();
            else SetUnselected();
        }


        public void SetSelected() {
            isSelected = true;
            GetComponent<Image>().sprite = selected;
        }


        public void SetUnselected() {
            isSelected = false;
            GetComponent<Image>().sprite = unselected;
        }


        public void Toggle() {
            if(isSelected) SetUnselected();
            else SetSelected();
        }
    }


}
