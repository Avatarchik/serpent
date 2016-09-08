using UnityEngine;
using System.Collections.Generic;

namespace Snake3D {

    public class MeshIndex : MonoBehaviour, IInitializable {

        public static MeshIndex instance;

        [NotNull]
        public MeshFilter meshFilter;

        private Dictionary<Edge, int> edgeToTriangleMap;

        public void Init() {
            Debug.Assert(instance == null);
            instance = this;

            MeshUtils.ApplyTransformToMesh(meshFilter);

            GenerateIndex();
        }

        void OnDestroy() {
            edgeToTriangleMap = null;
        }

        public int FindTriangleByEdge(Edge edge) {
            return edgeToTriangleMap[edge];
        }


        private void GenerateIndex() {
            edgeToTriangleMap = new Dictionary<Edge, int>();
            TriangleArray triangles = meshFilter.mesh.GetSaneTriangles(0);

            for (int i = 0; i < triangles.Length; ++i) {
                Triangle triangle = triangles[i];

                for (int j = 0; j < 3; ++j) {
                    Edge edge = new Edge(
                        triangle[j],
                        triangle[(j + 1) % 3]
                        );
                    edgeToTriangleMap.Add(edge, i);
                }
            }
        }
    }

} // namespace Snake3D
