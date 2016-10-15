using UnityEngine;
using Zenject;

namespace Serpent {

    [RequireComponent(typeof(AudioSource))]
    public class SnakeHead : MonoBehaviour {

        [NotNull] public GrowController growController;
        public float growDelta = 4;

        private LevelLogic levelLogic;
        private FoodSpawner foodSpawner;
        private AudioSource audioSource;

        [Inject]
        void Init(LevelLogic levelLogic, FoodSpawner foodSpawner) {
            this.levelLogic = levelLogic;
            this.foodSpawner = foodSpawner;

            audioSource = GetComponent<AudioSource>();
        }

        void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Food")) {
                // TODO: game over
                return;
            }

            OnFoodPickedUp(other.transform.parent.gameObject);
        }

        private void OnFoodPickedUp(GameObject food) {
            Destroy(food);
            audioSource.Play();

            foodSpawner.SpawnNewFood();
            levelLogic.Score++;
            // Grow snake
            growController.targetLength += growDelta;
        }
    }

} // namespace Serpent
