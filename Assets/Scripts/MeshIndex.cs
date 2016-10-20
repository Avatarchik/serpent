using UnityEngine;
using System.Collections.Generic;
using Zenject;

namespace Serpent {
    
    public class MeshIndex {
        
        private readonly Dictionary<IndexedEdge, int> edgeToTriangleMap = new Dictionary<IndexedEdge, int>();

        public MeshIndex(Mesh mesh) {
            GenerateIndex(mesh);
        }

        public int FindTriangleByEdge(IndexedEdge edge) => edgeToTriangleMap[edge];


        private void GenerateIndex(Mesh mesh) {
            TriangleArray triangles = mesh.GetSaneTriangles(0);

            for (int i = 0; i < triangles.Length; ++i) {
                IndexedTriangle triangle = triangles[i];

                for (int j = 0; j < 3; ++j) {
                    var edge = new IndexedEdge(
                        triangle[j],
                        triangle[(j + 1) % 3]
                        );
                    edgeToTriangleMap.Add(edge, i);
                }
            }
        }
    }

} // namespace Serpent
