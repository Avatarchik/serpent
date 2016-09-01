using UnityEngine;
using System.Collections;

namespace Snake3D {
    
    public class MovementController : MonoBehaviour, IInitializable {

        [NotNull] public Joystick joystick;
        [NotNull] public MeshFilter meshFilter;

        public float rotationSpeed = 240; // Degrees per second
        public float moveSpeed = 10; // Meters per second
        public float offsetFromSurface = 1;

        private MeshWalker walker;

        public void Init() {
            walker = new MeshWalker(meshFilter.mesh);
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
            transform.position += transform.up * offsetFromSurface;
        }

        private void Move(float distance) {
            Vector3 previousPos = transform.position;
#if UNITY_EDITOR
            Color[] colors = new Color[] { Color.red, Color.green, Color.blue };
            int currentColor = 0;
#endif

            while (distance > 0) {
                walker.StepUntilEdge(distance, out distance);
            }
        }
    }

} // namespace Snake3D
