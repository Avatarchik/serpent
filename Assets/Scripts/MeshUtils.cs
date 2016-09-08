﻿using UnityEngine;
using System.Collections;
using System;

namespace Snake3D {

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

        public static void FlipNormals(this Mesh mesh, int submeshIndex=0) {
            // Triangle orientation
            {
                TriangleArray triangles = mesh.GetSaneTriangles(submeshIndex);
                for (int i = 0; i < triangles.Length; ++i)
                    triangles[i] = triangles[i].Reversed;

                mesh.triangles = (int[])triangles;
            }

            // Normals
            {
                Vector3[] normals = mesh.normals;
                for (int i = 0; i < normals.Length; ++i)
                    normals[i] = -normals[i];

                mesh.normals = normals;
            }
        }

        public static BoundsOctree<int> GenerateOctree(this Mesh mesh) {
            var octree = new BoundsOctree<int>(64, Vector3.zero, 1, 1.25f);

            Vector3[] vertices = mesh.vertices;
            TriangleArray triangles = mesh.GetSaneTriangles(0);

            Func<int, Bounds> getTriangleBounds = (int index) => {
                IndexedTriangle t = triangles[index];
                Vector3 v1 = vertices[t.i1];
                Vector3 v2 = vertices[t.i2];
                Vector3 v3 = vertices[t.i3];
                Bounds aabb = new Bounds(v1, Vector3.zero);
                aabb.Encapsulate(v2);
                aabb.Encapsulate(v3);

                return aabb;
            };

            for (int i = 0; i < triangles.Length; ++i) {
                octree.Add(i, getTriangleBounds(i));
            }

            return octree;
        }
    }

} // namespace Snake3D
