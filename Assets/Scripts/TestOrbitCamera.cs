using UnityEngine;
using System.Collections;

namespace Snake3D {

    public class TestOrbitCamera : MonoBehaviour {

        // Speed in degrees
        public float speed = 60;
        public UI.Joystick joystick;

        Vector3 _eulerRotation = new Vector3();

        void Start() {

        }

        void Update() {
            Vector3 rotationDelta = new Vector3();
            rotationDelta.y = -joystick.value.x;
            rotationDelta.x = joystick.value.y;
            rotationDelta *= speed * Time.deltaTime;

            _eulerRotation += rotationDelta;
            _eulerRotation.x = Mathf.Clamp(_eulerRotation.x, -90, 90);

            transform.rotation = Quaternion.Euler(_eulerRotation);
        }
    }

} // namespace Snake3D
