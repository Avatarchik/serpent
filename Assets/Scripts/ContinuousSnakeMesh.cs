using UnityEngine;
using System.Collections;
using System;

public class ContinuousSnakeMesh : MonoBehaviour, IInitializable, IGrowable {

    public MonoBehaviour snakeMesh_;
    public float interval = 0.25f;

    public ISnakeMesh snakeMesh;
    public Transform tail;
    
    private Ring lastPoppedRing;

    private ISnakeMesh headPatch;
    private ISnakeMesh tailPatch;

    public void Init() {
        Debug.Assert(snakeMesh_ != null);
        snakeMesh = snakeMesh_ as ISnakeMesh;
        Debug.Assert(snakeMesh != null);

        headPatch = InitPatch("Head Patch");
        tailPatch = InitPatch("Tail Patch");

        Debug.Assert(tail != null);

        // Add first ring
        // TODO: replace by more sane mechanism (incorrect ring here)
        {
            Ring ring = new Ring(tail);

            GrowBodyMesh(ring, true);
            UpdateLastPoppedRing(ring);

            float distanceTraveled = 0;
            headPatch.PushToEnd(ring, distanceTraveled);
            headPatch.PushToEnd(ring, distanceTraveled);
            tailPatch.PushToEnd(ring, distanceTraveled);
            tailPatch.PushToEnd(ring, distanceTraveled);
        }


        snakeMesh.Kernel.OnPopFromStart += UpdateLastPoppedRing;
    }

    private ISnakeMesh InitPatch(string gameObjectName) {
        Transform patchTransform = transform.FindChild(gameObjectName);
        Debug.Assert(patchTransform != null);
        ISnakeMesh patch = patchTransform.GetComponent<SnakeMesh>();
        Debug.Assert(patch != null);
        return patch;
    }

    public void Grow(Ring ring) {
        GrowBodyMesh(ring);
        UpdateHeadPatch(ring);

        AnimateUV();
    }

    public void ShrinkToLength(float targetLength) {
        // Body mesh
        {
            // How many rings to remove
            float headPatchLength = GetPatchLength(headPatch);
            int targetRings = (int)((targetLength - headPatchLength) / interval) + 1;

            // Leave at least one ring
            targetRings = Mathf.Max(targetRings, 1);

            while (snakeMesh.Count > targetRings)
                snakeMesh.PopFromStart();
        }

        // Tail position
        {
            float tailPatchLength = targetLength - GetPatchLength(headPatch) - bodyLength;
            float factor = tailPatchLength / interval;

            Ring tailRing = Ring.lerp(snakeMesh.Kernel.Path[0], lastPoppedRing, factor);
            tailRing.SetTransform(tail);
            UpdateTailPatch(tailRing);
        }
    }

    public float ComputeLength() {
        return bodyLength + GetPatchLength(headPatch) + GetPatchLength(tailPatch);
    }

    #region Private

    private float bodyLength { get { return interval * (snakeMesh.Count - 1); } }
    private float bodyDistanceTraveled {
        get {
            return Mathf.Max(0, (snakeMesh.Kernel.RingsAdded - 1) * interval);
        }
    }


    private float GetPatchLength(ISnakeMesh patch) {
        var path = patch.Kernel.Path;
        float result = (path[0].position - path[1].position).magnitude;
        //Debug.Assert(result <= interval);
        return result;
    }

    // Grows body mesh if needed in current frame.
    //
    // TODO: handle adding of several rings per call
    private void GrowBodyMesh(Ring ring, bool force = false) {
        Vector3 lastGrowPoint;
        if (force)
            lastGrowPoint = ring.position;
        else
            lastGrowPoint = snakeMesh.Kernel.Path.Last.position;

        Vector3 delta = ring.position - lastGrowPoint;
        if (delta.magnitude >= interval || force) {
            // Grow
            //lastGrowPoint += delta.normalized * interval;
            float distanceTraveled = bodyDistanceTraveled + interval;
            snakeMesh.PushToEnd(ring, distanceTraveled);
        }
    }

    // TODO: remove duplicate code in UpdateHeadPatch() and UpdateTailPatch()
    private void UpdateHeadPatch(Ring headRing) {
        headPatch.PopFromStart();
        headPatch.PopFromStart();

        Ring bodyRing = snakeMesh.Kernel.Path.Last;
        float patchLength = (headRing.position - bodyRing.position).magnitude;

        headPatch.PushToEnd(bodyRing, bodyDistanceTraveled);
        headPatch.PushToEnd(headRing, bodyDistanceTraveled + patchLength);
    }

    private void UpdateTailPatch(Ring tailRing) {
        tailPatch.PopFromStart();
        tailPatch.PopFromStart();

        Ring bodyRing = snakeMesh.Kernel.Path[0];
        float bodyDistance = bodyDistanceTraveled - bodyLength;
        float tailPatchLength = (tailRing.position - bodyRing.position).magnitude;

        tailPatch.PushToEnd(tailRing, bodyDistance - tailPatchLength);
        tailPatch.PushToEnd(bodyRing, bodyDistance);
    }

    private void UpdateLastPoppedRing(Ring ring) {
        lastPoppedRing = ring;

        lastPoppedRing.SetTransform(tail);
    }

    private void AnimateUV() {
        Vector2 offset = snakeMesh.TextureOffset;
        float distanceTraveled = bodyDistanceTraveled + GetPatchLength(headPatch);
        offset.y = -distanceTraveled / snakeMesh.RingLength;
        snakeMesh.TextureOffset = offset;
        headPatch.TextureOffset = offset;
        tailPatch.TextureOffset = offset;

        Material tailMaterial = tail.GetChild(0).GetComponent<MeshRenderer>().material;
        /*get {
            Vector2 result = material.mainTextureOffset;
            Vector2 scale = material.mainTextureScale;
            result.x /= scale.x;
            result.y /= scale.y;
            return result;
        }
        set {
            Vector2 result = value;
            result.Scale(material.mainTextureScale);
            material.mainTextureOffset = result;
        }*/
    }

    #endregion Private
}
