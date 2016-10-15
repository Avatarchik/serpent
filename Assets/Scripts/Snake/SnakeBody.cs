using UnityEngine;
using Zenject;

namespace Serpent {

    public class SnakeBody : MonoBehaviour {

        public float interval = 0.75f;

        private Transform movementController;
        private ContinuousPath path;

        [Inject]
        public void Init() {
            movementController = transform.FindChild("Movement Controller");
            Debug.Assert(movementController != null);

            path = new ContinuousPath(interval);
        }

        void Update() {
            path.Grow(new ValueTransform(movementController));

            path.DebugDraw();
        }
    }

} // namespace Serpent
