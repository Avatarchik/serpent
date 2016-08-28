using UnityEngine;
using System.Collections;

namespace Snake3D {
    
    public class MeshWalkerTest : MonoBehaviour {

        [NotNull] public Transform intersectionMarker;
        [NotNull] public Transform startMarker;
        [NotNull] public MeshFilter surfaceMeshFilter;

        private MeshWalker walker;
        private float angle, distance = 1;
        private Vector2 coords;

        void Start() {
            MeshUtils.ApplyTransformToMesh(surfaceMeshFilter);

            walker = new MeshWalker(surfaceMeshFilter.mesh);
            walker.debugDrawEnabled = true;
            walker.RespawnNearPoint(startMarker.position + transform.position);
        }
        
        void OnGUI() {
            GUI.Box(new Rect(10, 10, 220, 220), "MeshWalker test");
            GUI.Label(new Rect(20, 40, 100, 20), "Angle");
            angle = GUI.HorizontalSlider(new Rect(120, 40, 100, 20), angle, 0, Mathf.PI * 2);
            GUI.Label(new Rect(20, 60, 100, 20), "X");
            coords.x = GUI.HorizontalSlider(new Rect(120, 60, 100, 20), coords.x, 0, 10);
            GUI.Label(new Rect(20, 80, 100, 20), "Y");
            coords.y = GUI.HorizontalSlider(new Rect(120, 80, 100, 20), coords.y, 0, 10);
            GUI.Label(new Rect(20, 100, 100, 20), "Distance");
            distance = GUI.HorizontalSlider(new Rect(120, 100, 100, 20), distance, 0, 10);
        }

        void Update() {
            walker.RespawnAtTriangle(733, angle, coords);
            walker.WriteToTransform(startMarker);

            float distanceLeft;
            walker.StepUntilEdge(distance, out distanceLeft);
            walker.WriteToTransform(intersectionMarker);


            walker.DebugDrawAxes();
        }
    }

} // namespace Snake3D
