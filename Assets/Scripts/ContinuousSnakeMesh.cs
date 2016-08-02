using UnityEngine;
using System.Collections;
using System;

public class ContinuousSnakeMesh : MonoBehaviour, IInitializable, IGrowable {

    public MonoBehaviour snakeMesh_;
    public float interval = 0.25f;

    public ISnakeMesh snakeMesh;

    private bool initialized = false;
    private Vector3 lastGrowPoint;

    private ISnakeMesh headPatch;

    public void Init() {
        Debug.Assert(snakeMesh_ != null);
        snakeMesh = snakeMesh_ as ISnakeMesh;
        Debug.Assert(snakeMesh != null);

        Transform headPatchTransform = transform.FindChild("Head Patch");
        Debug.Assert(headPatchTransform != null);
        headPatch = headPatchTransform.GetComponent<SnakeMesh>();
        Debug.Assert(headPatch != null);
    }


    public void Grow(Ring ring, float distanceTraveled) {
        if (!initialized) {
            initialized = true;
            lastGrowPoint = ring.position;
            GrowBodyMesh(ring, distanceTraveled, true);
            
            headPatch.PushToEnd(ring, distanceTraveled);
            headPatch.PushToEnd(ring, distanceTraveled);
        }

        GrowBodyMesh(ring, distanceTraveled);
        UpdateHeadPatch(ring, distanceTraveled);

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
    private void GrowBodyMesh(Ring ring, float distanceTraveled, bool force=false) {
        Vector3 delta = ring.position - lastGrowPoint;
        if (delta.magnitude >= interval || force) {
            // Grow
            lastGrowPoint += delta.normalized * interval;
            snakeMesh.PushToEnd(ring, distanceTraveled);
        }
    }

    private void UpdateHeadPatch(Ring ring, float distanceTraveled) {
        headPatch.PopFromStart();
        headPatch.PopFromStart();

        Ring lastRing = snakeMesh.Kernel.Path.Last;
        Vector3 lastPosition = lastRing.position;
        float lastDistance = distanceTraveled - (ring.position - lastPosition).magnitude;

        headPatch.PushToEnd(lastRing, lastDistance);
        headPatch.PushToEnd(ring, distanceTraveled);
    }

    private void AnimateUV(float distanceTraveled) {
        Vector2 offset = snakeMesh.TextureOffset;
        offset.y = -distanceTraveled / snakeMesh.RingLength;
        snakeMesh.TextureOffset = offset;
        headPatch.TextureOffset = offset;
    }
}
