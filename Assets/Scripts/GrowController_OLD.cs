using UnityEngine;
using System.Collections;

/// <summary>
/// Mediator for SnakeMesh and Walker.
/// </summary>
public class GrowController_OLD : MonoBehaviour, IInitializable {

    public Transform walker;
    public SnakeMesh snakeMesh;
    public float interval = 0.25f;

    private Vector3 lastGrowPoint;
    private float distanceTraveled = 0;
    private Vector3 previousPosition;

    // UV animation
    private MeshRenderer meshRenderer;
    private float textureShift = 1;

    public void Init() {
        Debug.Assert(walker != null);
        // Scale must be (1; 1; 1) because mesh normals depend on walker
        // transformation matrix and they need to be uniform
        Debug.Assert(walker.localScale == Vector3.one);
        Debug.Assert(snakeMesh != null);

        lastGrowPoint = walker.position;
        previousPosition = walker.position;


        meshRenderer = snakeMesh.GetComponent<MeshRenderer>();
    }

    void Update() {
        // Distance went since last frame
        float step = (walker.position - previousPosition).magnitude;
        previousPosition = walker.position;

        distanceTraveled += step;

        // Grow if needed
        Vector3 delta = walker.position - lastGrowPoint;
        if (delta.magnitude >= interval) {
            lastGrowPoint += delta.normalized * interval;
            snakeMesh.Grow(lastGrowPoint, walker.localToWorldMatrix, distanceTraveled);
        }

        // Animate UV
        // TODO: move to more suitable class?
        float ringLength = 2 * Mathf.PI * snakeMesh.radius;
        textureShift -= step / ringLength * meshRenderer.material.mainTextureScale.y;
        if (textureShift <= 0)
            textureShift += 1;
        Vector2 offset = meshRenderer.material.mainTextureOffset;
        offset.y = textureShift;
        meshRenderer.material.mainTextureOffset = offset;
    }
}
