using UnityEngine;
using System.Collections;

namespace Snake3D {

    /*
        Main scene class. Singleton.
     */
    public class SceneManager : MonoBehaviour {

        /* Public */

        public static SceneManager inst {
            get { return _instance; }
        }

        /* Private */

        static SceneManager _instance;

        void Start() {
            _instance = this;
        }

        void Update() {

        }
    }

} // namespace Snake3D