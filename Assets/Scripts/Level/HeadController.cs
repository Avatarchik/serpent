using UnityEngine;
using System.Collections;

namespace Snake3D {

    public class HeadController : MonoBehaviour {

        public Joystick joystick;
        public PlanetGeometry planetGeometry;

        void Start() {
            CastToSurface();
        }

        void Update() {

        }

        void CastToSurface() {
            Vector3 radiusVec = transform.position.normalized;
            float height = planetGeometry.GetHeightAt(radiusVec);
            transform.position = radiusVec * height;

            // TODO: Respect surface normal
        }
    }

} // namespace Snake3D
