using UnityEngine;
using System.Collections.Generic;

namespace Snake3D {

    public class MeshIndex : MonoBehaviour, IInitializable {

        public static MeshIndex instance;

        [NotNull]
        public MeshFilter meshFilter;

        private Dictionary<IndexedEdge, int> edgeToTriangleMap;

        public void Init() {
            Debug.Assert(instance == null);
            instance = this;

            MeshUtils.ApplyTransformToMesh(meshFilter);

            GenerateIndex();
        }

        void OnDestroy() {
            edgeToTriangleMap = null;
        }

        public int FindTriangleByEdge(IndexedEdge edge) {
            return edgeToTriangleMap[edge];
        }


        private void GenerateIndex() {
            edgeToTriangleMap = new Dictionary<IndexedEdge, int>();
            TriangleArray triangles = meshFilter.mesh.GetSaneTriangles(0);

            for (int i = 0; i < triangles.Length; ++i) {
                IndexedTriangle triangle = triangles[i];

                for (int j = 0; j < 3; ++j) {
                    IndexedEdge edge = new IndexedEdge(
                        triangle[j],
                        triangle[(j + 1) % 3]
                        );
                    edgeToTriangleMap.Add(edge, i);
                }
            }
        }
    }

} // namespace Snake3D
