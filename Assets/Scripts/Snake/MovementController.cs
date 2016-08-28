using UnityEngine;
using System.Collections;

namespace Snake3D {

    // TODO: (?) remove hard dependency on Mesh (replace by factory etc.)
    public class MovementController : MonoBehaviour, IInitializable {

        [NotNull] public Joystick joystick;
        [NotNull] public MeshWalker walker;

        public float rotationSpeed = 60; // Degrees per second
        public float moveSpeed = 2; // Meters per second
        public float offsetFromSurface = 1;
        
        public void Init() {
            //walker.RespawnAtDefaultPlace();
            walker.RespawnRandomly();
        }

        void OnGUI() {
            if (GUI.Button(new Rect(10, 100, 100, 20), "Respawn"))
                walker.RespawnRandomly();
        }
        
        void Update() {
            float angle = joystick.Value.x * rotationSpeed * Time.deltaTime;
            walker.Rotate(angle);
            
            Move(moveSpeed * Time.deltaTime);

            walker.WriteToTransform(transform);
        }

        private void Move(float distance) {
            Vector3 previousPos = transform.position;
#if UNITY_EDITOR
            Color[] colors = new Color[] { Color.red, Color.green, Color.blue };
            int currentColor = 0;
#endif

            while (distance > 0) {
                walker.StepUntilEdge(distance, out distance);

#if UNITY_EDITOR
                walker.WriteToTransform(transform);
                Vector3 currentPos = transform.position;
                Debug.DrawLine(previousPos, currentPos, colors[currentColor], 5f, false);
                currentColor = ++currentColor % colors.Length;
                previousPos = currentPos;
#endif
            }
        }
    }

} // namespace Snake3D
