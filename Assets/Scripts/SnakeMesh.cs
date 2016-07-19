﻿using UnityEngine;
using JetBlack.Core.Collections.Generic;

public class SnakeMesh : MonoBehaviour, IInitializable {

    public SnakeKernel kernel;
    public float radius = 0.5f;
    public int visiblePointsPerRing = 16; // Visible points per ring (as opposed to real number)
    public GameObject vertexLabelPrefab;
    public bool showDebugInfo = false;

    private ICircularBuffer<Vector3> vertices;
    private ICircularBuffer<Vector3> normals;
    private ICircularBuffer<Vector2> uvs;
    private ICircularBuffer<int> triangles;
    private Transform[] vertexLabels;

    // There is additional row of vertices because we need correct UVs
    private int RealPointsPerRing { get { return visiblePointsPerRing + 1; } }

    void Update() {
        // Empty Update() just to activate component checkbox in the editor
    }

    public void Init() {
        Debug.Assert(kernel != null);

        AllocateBuffers();

#if UNITY_EDITOR
        SetupVertexLabels();
#endif
    }

    public void Grow(Vector3 dest, Matrix4x4 localToWorld, float distanceTraveled) {
        kernel.Grow(dest);

        // Vertices and normals
        float ringLength = 2 * Mathf.PI * radius;
        for (int i = 0; i < RealPointsPerRing; ++i) {
            float angle = (2 * Mathf.PI) * i / visiblePointsPerRing;
            var normal = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
            normal = localToWorld * normal;
            Vector3 vertex = normal * radius + dest;

            Vector2 uv = new Vector2();
            uv.x = 0.75f - (float)i / visiblePointsPerRing;
            uv.y = distanceTraveled / ringLength;

            vertices.Enqueue(vertex);
            normals.Enqueue(normal);
            uvs.Enqueue(uv);

#if UNITY_EDITOR
            UpdateVertexDebugInfo(vertices.Count - 1);
#endif
        }

        AddSegmentTriangles();

        UpdateMesh();
    }

    public void Shrink() {
        vertices.Dequeue(RealPointsPerRing);
        normals.Dequeue(RealPointsPerRing);
        uvs.Dequeue(RealPointsPerRing);

        // Make deleted triangles degenerate
        for (int i = 0; i < trianglesPerSegment; ++i) {
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = 0;
            triangles[i * 3 + 2] = 0;
        }
        triangles.Dequeue(trianglesPerSegment * 3);

        kernel.Shrink();
    }


    #region Private part

    private int trianglesPerSegment { get { return visiblePointsPerRing * 2; } }


    private void AllocateBuffers() {
        int verticesNum = kernel.pointsNum * RealPointsPerRing;
        vertices = new CircularBuffer<Vector3>(verticesNum);
        normals = new CircularBuffer<Vector3>(verticesNum);
        uvs = new CircularBuffer<Vector2>(verticesNum);
        int trianglesNum = (kernel.pointsNum - 1) * trianglesPerSegment;
        triangles = new CircularBuffer<int>(trianglesNum * 3);
        
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.MarkDynamic();
    }

    private void AddSegmentTriangles() {
        ICircularBuffer<Vector3> path = kernel.Path;
        if (path.Count >= 2) {
            int ring1Offset = path.RawPosition(path.Count - 2) * RealPointsPerRing;
            int ring2Offset = path.RawPosition(path.Count - 1) * RealPointsPerRing;

            int[] quadIndices = new int[4];
            int[] triangleIndices = new int[6];
            for (int i = 0; i < visiblePointsPerRing; ++i) {
                quadIndices[0] = ring1Offset + i;
                quadIndices[1] = ring1Offset + i + 1;
                quadIndices[2] = ring2Offset + i + 1;
                quadIndices[3] = ring2Offset + i;

                MeshUtils.ConvertQuadToTriangles(quadIndices, triangleIndices);
                triangles.Enqueue(triangleIndices);
            }
        }
    }

    private void UpdateMesh() {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();
        mesh.vertices = vertices.RawBuffer;
        mesh.normals = normals.RawBuffer;
        mesh.uv = uvs.RawBuffer;
        mesh.triangles = triangles.RawBuffer;
    }


    #region Debug methods

#if UNITY_EDITOR

    /// <summary>
    /// Setup labels indicating vertex index.
    /// </summary>
    private void SetupVertexLabels() {
        if (!showDebugInfo)
            return;

        int verticesNum = kernel.pointsNum * RealPointsPerRing;

        vertexLabels = new Transform[verticesNum];
        for (int i = 0; i < verticesNum; ++i) {
            GameObject label = Instantiate<GameObject>(vertexLabelPrefab);
            label.GetComponent<TextMesh>().text = i.ToString();
            vertexLabels[i] = label.transform;
        }
    }

    /// <summary>
    /// Update debug info for given vertex.
    /// </summary>
    /// <param name="index">Vertex index in circular buffer (not raw)</param>
    private void UpdateVertexDebugInfo(int index) {
        if (!showDebugInfo)
            return;

        Vector3 vertex = vertices[index];
        Vector3 normal = normals[index];

        int labelIndex = vertices.RawPosition(index);
        vertexLabels[labelIndex].position = vertex;

        Debug.DrawLine(vertex, vertex + Vector3.one * 0.01f, Color.red, 5f);
        Debug.DrawLine(vertex, vertex + normal * 0.25f, Color.yellow, 5f);
    }

#endif // UNITY_EDITOR

    #endregion Debug methods

    #endregion Private part
}