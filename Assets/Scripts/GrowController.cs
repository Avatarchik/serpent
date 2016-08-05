using UnityEngine;
using System.Collections;

public interface IGrowable {
    void Grow(Ring ring);
    void ShrinkToLength(float targetLength);
    float ComputeLength();
}

public class GrowController : MonoBehaviour, IInitializable {

    public Transform walker;
    public MonoBehaviour growable_;
    public float targetLength;

    private IGrowable growable;

    public void Init() {
        Debug.Assert(walker != null);
        // Scale must be (1; 1; 1) because mesh normals depend on walker
        // transformation matrix and they need to be uniform
        Debug.Assert(walker.localScale == Vector3.one);

        Debug.Assert(growable_ != null);
        growable = growable_ as IGrowable;
        Debug.Assert(growable != null);
    }
	
	void Update() {
        MaintainLength();
    }

    private void MaintainLength() {
        float currentLength = growable.ComputeLength();
        float shrinkLength = currentLength - targetLength;
        if (shrinkLength > 0)
            growable.ShrinkToLength(targetLength);
        
        growable.Grow(new Ring(walker));
    }
}
