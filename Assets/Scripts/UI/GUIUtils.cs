using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIUtils : MonoBehaviour {

    private const float kFPSUpdateInterval = 0.5f;
    private Text debugText;

    public void ExitGame() {
        Application.Quit();
    }

    void Start() {
        // Display FPS
        debugText = GameObject.FindWithTag("Debug text").GetComponent<Text>();
        StartCoroutine(UpdateFPS());

        // Adjust UI scale
#if !UNITY_EDITOR
        CanvasScaler scaler = GameObject.Find("Play Canvas").GetComponent<CanvasScaler>();
        scaler.scaleFactor = 1;
#endif
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
