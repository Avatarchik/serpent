using UnityEngine;
using System.Collections;

public class GrowController : MonoBehaviour, IInitializable {

    public Transform walker;
    public MonoBehaviour growable_;
    public float targetLength;

    private IGrowable growable;
    private float distanceTraveled = 0;
    private Vector3 previousPosition;

    public void Init() {
        Debug.Assert(walker != null);
        // Scale must be (1; 1; 1) because mesh normals depend on walker
        // transformation matrix and they need to be uniform
        Debug.Assert(walker.localScale == Vector3.one);

        Debug.Assert(growable_ != null);
        growable = growable_ as IGrowable;
        Debug.Assert(growable != null);

        previousPosition = walker.position;
    }
	
	void Update () {
        // Distance went since last frame
        float step = (walker.position - previousPosition).magnitude;
        previousPosition = walker.position;
        distanceTraveled += step;

        MaintainLength();
    }

    private void MaintainLength() {
        float currentLength = growable.ComputeLength();
        float shrinkLength = currentLength - targetLength;
        if (shrinkLength > 0)
            growable.Shrink(shrinkLength);

        growable.Grow(walker.localToWorldMatrix, distanceTraveled);
    }
}
