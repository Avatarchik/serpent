using UnityEngine;
using System.Collections;

namespace Snake3D {

    // Must be initialized after LevelLogic
    public class FoodSpawner : MonoBehaviour, IInitializable {

        [NotNull] public GameObject foodPrefab;
        [NotNull] public FoodPointer foodPointer;

        public static FoodSpawner instance { get; private set; }

        private MeshWalker walker;

        // Enable editor deactivation
        void Update() { }

        public void Init() {
            Debug.Assert(instance == null);
            instance = this;

            walker = new MeshWalker(LevelLogic.instance.LevelMesh);

            SpawnNewFood();
        }

        public void SpawnNewFood() {
            var gameObject = Instantiate(foodPrefab) as GameObject;
            walker.RespawnRandomly();
            walker.WriteToTransform(gameObject.transform);

            foodPointer.food = gameObject.transform;
        }
    }

} // namespace Snake3D
