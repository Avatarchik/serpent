using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Snake3D {

    public class TerrainWalker : MonoBehaviour {

        public Joystick joystick;
        public float offsetFromSurface = 1;
        public float speed = 2;

        private Terrain terrain;

        void Start() {
            terrain = FindObjectOfType<Terrain>();
            if (terrain == null)
                throw new UnityException("Terrain object not found in current scene");
        }

        void Update() {
            const float kSpeed = 5;

            // Update position
            Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //if (Application.isMobilePlatform) {
                direction.x = joystick.Value.x;
                direction.z = joystick.Value.y;
            //}
            Vector3 newPosition = transform.position + direction * kSpeed * Time.deltaTime;

            // Normalized terrain coordinates
            Vector3 size = terrain.terrainData.size;
            float x = newPosition.x / size.x;
            float y = newPosition.z / size.z;

            // Update height
            newPosition.y = terrain.terrainData.GetInterpolatedHeight(x, y) + offsetFromSurface;
            transform.position = newPosition;

            // Update normal
            transform.up = terrain.terrainData.GetInterpolatedNormal(x, y);
        }
    }

}








