using UnityEngine;
using System.Collections;

namespace Snake3D {

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
            /* TODO

            var gameObject = Instantiate(foodPrefab, position, Quaternion.identity) as GameObject;
            foodPointer.food = gameObject.transform;*/
        }
    }

} // namespace Snake3D
