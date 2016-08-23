using UnityEngine;
using System.Collections;
using System;

namespace Snake3D {
    
    public struct Triangle {
        // Indices
        public int v1, v2, v3;

        public Triangle(int v1, int v2, int v3) {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }

        public int this[int index] {
            get {
                switch (index) {
                    case 0: return v1;
                    case 1: return v2;
                    case 2: return v3;
                    default: throw new System.IndexOutOfRangeException();
                }
            }

            set {
                switch (index) {
                    case 0: v1 = value; return;
                    case 1: v2 = value; return;
                    case 2: v3 = value; return;
                    default: throw new System.IndexOutOfRangeException();
                }
            }
        }

        public int Length { get { return 3; } }

        public override string ToString() {
            return string.Format("{{{0}, {1}, {2}}}", v1, v2, v3);
        }

        public Triangle Reversed { get {
                return new Triangle(v3, v2, v1);
            }
        }
    }

    /// A proxy for handy reading/writing of triangles from/to the triangle array.
    public struct TriangleArray {
        private int[] rawTriangles;

        public TriangleArray(int[] rawArray) {
            rawTriangles = rawArray;
        }

        public static explicit operator int[] (TriangleArray triangleArray) {
            return triangleArray.rawTriangles;
        }

        public void ReverseTriangles() {
            for (int i = 0; i < Length; ++i) {
                this[i] = this[i].Reversed;
            }
        }

        public Triangle this[int index] {
            get {
                int offset = index * 3;
                return new Triangle(
                    rawTriangles[offset + 0],
                    rawTriangles[offset + 1],
                    rawTriangles[offset + 2]
                    );
            }
            set {
                int offset = index * 3;
                rawTriangles[offset + 0] = value.v1;
                rawTriangles[offset + 1] = value.v2;
                rawTriangles[offset + 2] = value.v3;
            }
        }

        public int Length { get { return rawTriangles.Length / 3; } }
    }

    public static class MeshUtils {

        // TODO: refactor this method and code depending on it
        // to use wrapped Triangle-s instead of in-place machinery
        public static void ConvertQuadToTriangles(int[] quadIndices, int[] triangleIndices) {
            int[] q = quadIndices;
            int[] t = triangleIndices;
            // 1st triangle
            t[0] = q[0];
            t[1] = q[1];
            t[2] = q[3];
            // 2nd triangle
            t[3] = q[3];
            t[4] = q[1];
            t[5] = q[2];
        }

        public static TriangleArray GetSaneTriangles(this Mesh mesh, int submesh) {
            return new TriangleArray(mesh.GetTriangles(submesh));
        }

        public static void ResetTransform(this Transform transform) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void ApplyTransformToMesh(MeshFilter meshFilter) {
            Transform transform = meshFilter.transform;
            //Matrix4x4 matrix = transform.localToWorldMatrix;
            Mesh mesh = meshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            Vector2[] uvs = mesh.uv;
            var triangles = new TriangleArray(mesh.triangles);

            for (int i = 0; i < vertices.Length; ++i) {
                vertices[i] = transform.TransformPoint(vertices[i]);
                normals[i] = transform.TransformVector(normals[i]);
            }

            // Flip triangle order if needed
            {
                Vector3 s = transform.localScale;
                if (s.x * s.y * s.z < 0)
                    triangles.ReverseTriangles();
            }
            
            transform.ResetTransform();
            mesh.Clear(false);
            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uvs;
            mesh.triangles = (int[]) triangles;
            mesh.UploadMeshData(false);
        }
    }

} // namespace Snake3D
