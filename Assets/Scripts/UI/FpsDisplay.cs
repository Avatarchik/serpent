using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Snake3D {

public class FpsDisplay : MonoBehaviour {

    private const float kFPSUpdateInterval = 0.5f;
    private Text debugText;

    void Start() {
        // Display FPS
        debugText = GameObject.FindWithTag("Debug text").GetComponent<Text>();
        StartCoroutine(UpdateFPS());
    }

    IEnumerator UpdateFPS() {
        for (; ; ) {
            int fps = Time.unscaledDeltaTime == 0 ? 0 : (int)(1f / Time.unscaledDeltaTime);
            debugText.text = string.Format(
                "FPS: {0}", fps
            );
            yield return new WaitForSeconds(kFPSUpdateInterval);
        }
    }

}

} // namespace Snake3D
