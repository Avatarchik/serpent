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
    
    public void Init() {
        Debug.Assert(snakeMesh_ != null);
        snakeMesh = snakeMesh_ as ISnakeMesh;
        Debug.Assert(snakeMesh != null);
    }


    public void Grow(Matrix4x4 localToWorld, float distanceTraveled) {
        Vector3 dest = localToWorld.MultiplyPoint3x4(Vector3.zero);

        if (!initialized) {
            initialized = true;
            lastGrowPoint = dest;
        }

        // Grow if needed
        Vector3 delta = dest - lastGrowPoint;
        if (delta.magnitude >= interval) {
            lastGrowPoint += delta.normalized * interval;
            snakeMesh.PushToEnd(localToWorld, distanceTraveled);
        }

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


    private void AnimateUV(float distanceTraveled) {
        Vector2 offset = snakeMesh.TextureOffset;
        offset.y = -distanceTraveled / snakeMesh.RingLength;
        snakeMesh.TextureOffset = offset;
    }
}
