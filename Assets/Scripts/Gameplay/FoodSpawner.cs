using UnityEngine;
using Zenject;

namespace Serpent {

    public class FoodSpawner : MonoBehaviour {

        [NotNull] public GameObject foodPrefab;
        [NotNull] public FoodPointer foodPointer;

        private MeshWalker walker;

        // Enable editor deactivation
        void Update() { }

        [Inject]
        private void Init(LevelLogic levelLogic, MeshIndex meshIndex) {

            walker = new MeshWalker(levelLogic.LevelMesh, meshIndex);

            SpawnNewFood();
        }

        public void SpawnNewFood() {
            var gameObject = Instantiate(foodPrefab) as GameObject;
            walker.RespawnRandomly();
            walker.WriteToTransform(gameObject.transform);

            foodPointer.food = gameObject.transform;
        }
    }

} // namespace Serpent
