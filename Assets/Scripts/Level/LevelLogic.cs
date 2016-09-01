using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Snake3D {

    public class LevelLogic : MonoBehaviour, IInitializable {

        [NotNull] public MeshFilter levelMeshFilter;


        public static LevelLogic instance { get; private set; }

        public int Score {
            get { return score; }
            set {
                score = value;
                scoreText.text = value.ToString();
            }
        }

        public Mesh LevelMesh { get; private set; }

        private Text scoreText;
        private int score;

        public void Init() {
            Debug.Assert(instance == null);
            instance = this;

            scoreText = GameObject.Find("Score text").GetComponent<Text>();
            Debug.Assert(scoreText != null);

            AdjustUiScale();

            LevelMesh = levelMeshFilter.mesh;
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
