using UnityEngine;
using System.Collections;

/// <summary>
/// Mediator for IPointChain and Walker.
/// </summary>
public class GrowController : MonoBehaviour, IInitializable {

    public Transform walker;
    public MonoBehaviour pointChain_; // Must be an instance of IPointChain
    public float interval = 0.25f;

    private IPointChain pointChain;
    private Vector3 lastPoint;

    public void Init() {
        Debug.Assert(walker != null);
        // Scale must be (1; 1; 1) because mesh normals depend on walker
        // transformation matrix and they need to be uniform
        Debug.Assert(walker.localScale == Vector3.one);

        Debug.Assert(pointChain_ != null);
        pointChain = pointChain_ as IPointChain;
        Debug.Assert(pointChain != null);

        lastPoint = walker.position;
    }

    void Update() {
        Vector3 delta = walker.position - lastPoint;
        if (delta.magnitude >= interval) {
            lastPoint += delta.normalized * interval;
            pointChain.Grow(lastPoint, walker.localToWorldMatrix);
        }
    }
}
