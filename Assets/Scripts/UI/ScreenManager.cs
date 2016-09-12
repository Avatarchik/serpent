using UnityEngine;
using System.Collections;

namespace Snake3D {

    public class ScreenManager : MonoBehaviour {

        public GameObject initialOpen;

        private GameObject current;

        public void OpenScreen(GameObject screen) {
            current?.SetActive(false);

            screen.SetActive(true);
            current = screen;
        }

        void Start() {
            current = initialOpen;

            current?.SetActive(true);
        }

        void Update() {

        }
    }

} // namespace Snake3D
