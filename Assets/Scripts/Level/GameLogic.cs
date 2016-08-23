using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Snake3D {

    public class GameLogic : MonoBehaviour, IInitializable {

        public static GameLogic instance { get; private set; }

        public int Score {
            get { return score; }
            set {
                score = value;
                scoreText.text = value.ToString();
            }
        }

        private Text scoreText;
        private int score;

        public void Init() {
            Debug.Assert(instance == null);
            instance = this;

            scoreText = GameObject.Find("Score text").GetComponent<Text>();
            Debug.Assert(scoreText != null);


            AdjustUiScale();
        }

        public void ExitGame() {
            Application.Quit();
        }


        private void AdjustUiScale() {
#if !UNITY_EDITOR
            CanvasScaler scaler = GameObject.Find("Play Canvas").GetComponent<CanvasScaler>();
            scaler.scaleFactor = 1;
#endif
        }
    }

} // namespace Snake3D
