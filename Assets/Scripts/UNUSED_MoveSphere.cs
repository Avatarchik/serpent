using UnityEngine;
using System.Collections;

namespace Snake3D {

    public class MoveSphere : MonoBehaviour {

        public TerrainCollider terrainCollider;
        public Joystick joystick;
        public float velocity = 10;

        void Update() {
            // Cast to terrain
            Ray ray = new Ray(transform.position + transform.up * 500, -transform.up);
            RaycastHit hitInfo = new RaycastHit();
            terrainCollider.Raycast(ray, out hitInfo, 1000);

            transform.position = hitInfo.point;
            transform.up = hitInfo.normal;
        }
    }

}