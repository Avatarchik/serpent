using UnityEngine;
using System.Collections;

namespace Snake3D {

    public class PlanetWalker : MonoBehaviour, IInitializable {

        public Joystick joystick;
        public PlanetGeometry planetGeometry;

        public float rotationSpeed = 60; // Degrees per second
        public float moveSpeed = 2; // Meters per second
        public float offsetFromSurface = 1;

        public void Init() {
            CastToSurface();
        }

        void Update() {
            float angle = joystick.Value.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, angle, 0, Space.Self);

            float step = moveSpeed * Time.deltaTime;
            transform.Translate(0, 0, step, Space.Self);

            CastToSurface();
        }

        void CastToSurface() {
            Vector3 radiusVec = transform.position.normalized;
            float height = planetGeometry.GetHeightAt(radiusVec) + offsetFromSurface;
            transform.position = radiusVec * height;

            // TODO: Respect surface normal
        }
    }

} // namespace Snake3D
