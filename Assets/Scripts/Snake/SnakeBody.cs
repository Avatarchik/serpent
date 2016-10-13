using UnityEngine;
using System.Collections;

namespace Serpent {

    public class SnakeBody : MonoBehaviour, IInitializable {

        public float interval = 0.75f;

        private Transform movementController;
        private ContinuousPath path;

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
