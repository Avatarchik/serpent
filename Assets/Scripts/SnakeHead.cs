using UnityEngine;

namespace Snake3D {

    [RequireComponent(typeof(AudioSource))]
    public class SnakeHead : MonoBehaviour {

        [NotNull] public GrowController growController;
        public float growDelta = 4;

        private AudioSource audioSource;

        void Start() {
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

            FoodSpawner.instance.SpawnNewFood();
            LevelLogic.instance.Score++;
            // Grow snake
            growController.targetLength += growDelta;
        }
    }

} // namespace Snake3D
