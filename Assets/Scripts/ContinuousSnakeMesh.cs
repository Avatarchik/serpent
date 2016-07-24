using UnityEngine;
using System.Collections;
using System;

public interface IGrowable {
    void Grow(Matrix4x4 localToWorld, float distanceTraveled);
    void Shrink(float length);
    float ComputeLength();
}

public class ContinuousSnakeMesh : MonoBehaviour, IInitializable, IGrowable {

    public MonoBehaviour snakeMesh_;
    public float interval = 0.25f;

    public ISnakeMesh snakeMesh;

    private bool initialized = false;
    private Vector3 lastGrowPoint;

    private ISnakeMesh headPatch;
    private Matrix4x4 lastTransformMat; // TODO: remove when SnakeKernel will contain all transform matrices

    public void Init() {
        Debug.Assert(snakeMesh_ != null);
        snakeMesh = snakeMesh_ as ISnakeMesh;
        Debug.Assert(snakeMesh != null);

        Transform headPatchTransform = transform.FindChild("Head Patch");
        Debug.Assert(headPatchTransform != null);
        headPatch = headPatchTransform.GetComponent<SnakeMesh>();
        Debug.Assert(headPatch != null);
    }


    public void Grow(Matrix4x4 localToWorld, float distanceTraveled) {
        Vector3 dest = localToWorld.MultiplyPoint3x4(Vector3.zero);

        if (!initialized) {
            initialized = true;
            lastGrowPoint = dest;

            // TODO: remove
            lastTransformMat = localToWorld;
            headPatch.PushToEnd(localToWorld, distanceTraveled);
            headPatch.PushToEnd(localToWorld, distanceTraveled);
        }

        GrowBodyMesh(localToWorld, distanceTraveled);
        UpdateHeadPatch(localToWorld, distanceTraveled);

        AnimateUV(distanceTraveled);
    }

    public void Shrink(float length) {
        // How many rings to remove
        int rings = (int)(length / interval);

        // Leave at least one ring
        if (rings > snakeMesh.Count - 1)
            rings = snakeMesh.Count - 1;

        for (int i = 0; i < rings; ++i)
            snakeMesh.PopFromStart();
    }

    public float ComputeLength() {
        // TODO
        return interval * snakeMesh.Count;
    }


    // Grows body mesh if needed in current frame.
    private void GrowBodyMesh(Matrix4x4 localToWorld, float distanceTraveled) {
        Vector3 dest = localToWorld.MultiplyPoint3x4(Vector3.zero);

        Vector3 delta = dest - lastGrowPoint;
        if (delta.magnitude >= interval) {
            // Grow
            lastGrowPoint += delta.normalized * interval;
            snakeMesh.PushToEnd(localToWorld, distanceTraveled);

            // TODO: remove in future
            lastTransformMat = localToWorld;
        }
    }

    private void UpdateHeadPatch(Matrix4x4 localToWorld, float distanceTraveled) {
        headPatch.PopFromStart();
        headPatch.PopFromStart();
        
        Vector3 currentPosition = localToWorld.MultiplyPoint3x4(Vector3.zero);
        Vector3 lastPosition = lastTransformMat.MultiplyPoint3x4(Vector3.zero);
        float lastDistance = distanceTraveled - (currentPosition - lastPosition).magnitude;

        headPatch.PushToEnd(lastTransformMat, lastDistance);
        headPatch.PushToEnd(localToWorld, distanceTraveled);
    }

    private void AnimateUV(float distanceTraveled) {
        Vector2 offset = snakeMesh.TextureOffset;
        offset.y = -distanceTraveled / snakeMesh.RingLength;
        snakeMesh.TextureOffset = offset;
        headPatch.TextureOffset = offset;
    }
}
