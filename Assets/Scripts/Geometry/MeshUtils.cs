using UnityEngine;
using System.Collections;

public static class MeshUtils {

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
}
