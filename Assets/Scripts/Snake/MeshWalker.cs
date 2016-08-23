using UnityEngine;

namespace Snake3D {

    /** Analogue of Transform for mesh tangent-space.
     * 
     * Doesn't store scale and reference to corresponding Mesh. */
    public struct TangentTransform {
        public int triangleIndex;
        public Vector2 localPosition;
        public float angle;
    }


    // TODO: (?) remove dependency on Transform (at the other hand, it forces not to allocate MeshWalker)
    [RequireComponent(typeof(Transform))]
    public class MeshWalker : MonoBehaviour, IInitializable {
        [NotNull]
        public MeshFilter meshFilter;

        private TangentTransform tangentTransform;
        private Mesh mesh;
        private TriangleArray triangles;
        private Vector3[] vertices;
        private Vector3[] normals;

        public void Init() {
            mesh = meshFilter.mesh;

            triangles = new TriangleArray(mesh.triangles);
            vertices = mesh.vertices;
            normals = mesh.normals;
        }

        /// Respawns at vertex nearest to Transform
        public void RespawnNearestToTransform() {
            // Find nearest triangle to transform
            int nearestTriangle = 0;
            float nearestDistance = float.PositiveInfinity;
            Vector3 position = transform.position;

            for (int i = 0; i < triangles.Length; ++i) {
                Vector3 vertex = vertices[triangles[i].v1];
                float distance = (position - vertex).sqrMagnitude;
                if (distance < nearestDistance) {
                    // Found new nearest triangle
                    nearestDistance = distance;
                    nearestTriangle = i;
                }
            }

            RespawnAtTriangle(nearestTriangle);
        }

        /*public void RespawnAtDefaultPlace() {
            // TODO: throw vertical ray from (0, 0, 0) to (0, +Infinity, 0)
            throw new System.NotImplementedException();
        }*/

        public void RespawnRandomly() {
            // Not true randomness, but enough for our purposes
            
            int triangleIndex = Random.Range(0, triangles.Length);
            RespawnAtTriangle(triangleIndex);
        }

        public void RespawnAtTriangle(int triangleIndex) {
            if (triangleIndex < 0 || triangleIndex > triangles.Length)
                throw new System.IndexOutOfRangeException();

            tangentTransform.triangleIndex = triangleIndex;
            // tangentTransform.position is already Vector2.zero
            tangentTransform.angle = Random.Range(0f, 360f);

            WriteToTransform();
        }
        

        public void Rotate(float angle) {
            throw new System.NotImplementedException();
        }

        /**
         * Moves IWalker forward by \param distance, stopping if an edge has been reached
         * (in which case \param distanceLeft > 0)
         */
        public void MoveForward(float distance, out float distanceLeft) {
            // TODO

            var triangles = new TriangleArray(mesh.triangles);
            Vector3[] vertices = mesh.vertices;
            Vector2 direction = LocalDirection;
            Vector2 position = tangentTransform.localPosition;
            Triangle triangle = CurrentTriangle;

            // Triangle vertices in world space
            Vector3 v1w = vertices[triangle.v1];
            Vector3 v2w = vertices[triangle.v2];
            Vector3 v3w = vertices[triangle.v3];

            Matrix4x4 worldToTangent = new Matrix4x4();
            Vector2 v1 = worldToTangent * v1w;
            Vector2 v2 = worldToTangent * v2w;
            Vector2 v3 = worldToTangent * v3w;

            // Check for collision with edges

            // TODO: remove
            distanceLeft = Mathf.Max(0, distance - 1);

        }

        /// Walker doesn't necessarily updates Transform during MoveForward and Rotate,
        /// so user needs to do it manually
        public void WriteToTransform() {
            // TODO
            int vertexIndex = CurrentTriangle.v1;
            transform.position = vertices[vertexIndex];
            transform.up = normals[vertexIndex];
        }


        private Vector2 LocalDirection {
            get {
                float a = tangentTransform.angle;
                return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            }
        }

        private Triangle CurrentTriangle {
            get {
                return triangles[tangentTransform.triangleIndex];
            }
        }
    }

} // namespace Snake3D