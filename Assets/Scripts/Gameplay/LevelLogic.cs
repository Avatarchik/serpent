using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Serpent {

    public class LevelLogic : MonoBehaviour {

        [NotNull] public MeshFilter levelMeshFilter;

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

        [Inject]
        private void Init() {
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

} // namespace Serpent
