using UnityEngine;
using System.Collections;

public interface IGrowable {
    void Grow(Ring ring, float distanceTraveled);
    void Shrink(float length);
    float ComputeLength();
}

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

        DragSnake();
    }

    private void DragSnake() {
        float currentLength = growable.ComputeLength();
        float shrinkLength = currentLength - targetLength;
        if (shrinkLength > 0)
            growable.Shrink(shrinkLength);

        var ring = new Ring(walker.position, walker.rotation);
        growable.Grow(ring, distanceTraveled);
    }
}
