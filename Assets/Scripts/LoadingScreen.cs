using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Snake3D {

    public class LoadingScreen : MonoBehaviour {

        public float blinksPerSecond = 0.5f;
        public Text textComponent;
        public CanvasGroup textContainer;

        const float kPercentageUpdateInterval = 0.5f; // seconds

        string loadingText;
        bool waitForClick = false;
        

        void Start() {
            loadingText = textComponent.text;

            StartCoroutine(LoadLevel());
        }

        void Update() {
            if (waitForClick) {
                float factor = Mathf.Sin(blinksPerSecond * Time.unscaledTime * 2 * Mathf.PI);
                factor = 0.5f * (factor + 1f);

                //factor = 0.5f * (factor + 1);
                textContainer.alpha = factor;

                if (Input.GetMouseButtonDown(0))
                    SwitchToLevel();
            }
        }

        void SwitchToLevel() {
            Destroy(this.gameObject);
            GameObject.Find("{ Level initializer }")
                .GetComponent<LevelInitializer>()
                .enabled = true;
        }

        IEnumerator LoadLevel() {
            yield return new WaitForSeconds(1);

            AsyncOperation asyncOperation = Application.LoadLevelAdditiveAsync("Level");

            for(;;) {
                // Getting the value once at start of loop iteration
                // to avoid desynchronization
                float progress = asyncOperation.progress;
                int percentage = Mathf.RoundToInt(asyncOperation.progress * 100);
                textComponent.text = string.Format("{0} {1}%", loadingText, percentage);

                if (progress != 1)
                    yield return new WaitForSeconds(kPercentageUpdateInterval);
                    break;
            }

            waitForClick = true;
            textComponent.text = "Нажмите для продолжения";

            yield return null;
        }
    }

} // namespace Snake3D
