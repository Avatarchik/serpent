using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
        }


    }

} // namespace Snake3D
