using UnityEngine;
using System.Collections;

namespace Snake3D {

    [RequireComponent(typeof(MeshWalker))]
    public class MeshWalkerTest : MonoBehaviour {

        private MeshWalker walker;
        private float angle;
        private Vector2 coords;

        void Start() {
            walker = GetComponent<MeshWalker>();

            MeshUtils.ApplyTransformToMesh(walker.meshFilter);

            walker.Init();
            walker.RespawnNearestToTransform();
        }
        
        void OnGUI() {
            GUI.Box(new Rect(10, 10, 220, 220), "MeshWalker test");
            GUI.Label(new Rect(20, 40, 100, 20), "Angle");
            angle = GUI.HorizontalSlider(new Rect(120, 40, 100, 20), angle, 0, Mathf.PI * 2);
            GUI.Label(new Rect(20, 60, 100, 20), "X");
            coords.x = GUI.HorizontalSlider(new Rect(120, 60, 100, 20), coords.x, 0, 10);
            GUI.Label(new Rect(20, 80, 100, 20), "Y");
            coords.y = GUI.HorizontalSlider(new Rect(120, 80, 100, 20), coords.y, 0, 10);
        }

        void Update() {
            walker.RespawnAtTriangle(733, angle, coords);

            float distanceLeft;
            walker.MoveForward(3, out distanceLeft);
        }
    }

} // namespace Snake3D
