using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Snake3D {

    public class TerrainWalker : MonoBehaviour {

        public Joystick joystick;
        public float offsetFromSurface = 1;
        public float speed = 2;
        public float rotationSpeed = 100; // degrees per second

        private Terrain terrain;

        void Start() {
            terrain = FindObjectOfType<Terrain>();
            if (terrain == null)
                throw new UnityException("Terrain object not found in current scene");

            CastToSurface();
        }

        void Update() {
            float angle = joystick.Value.x * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, angle, 0);

            float step = speed * Time.deltaTime;
            transform.Translate(0, 0, step);

            CastToSurface();
        }

        private void CastToSurface() {
            Vector3 pos = transform.position;

            // Get normalized terrain coordinates
            Vector3 size = terrain.terrainData.size;
            float x = pos.x / size.x;
            float y = pos.z / size.z;

            // Update height
            pos.y = terrain.terrainData.GetInterpolatedHeight(x, y) + offsetFromSurface;
            transform.position = pos;

            // Update normal
            Vector3 normal = terrain.terrainData.GetInterpolatedNormal(x, y);
            Vector3 forward = Vector3.Cross(transform.forward, normal).normalized;
            forward = Vector3.Cross(normal, forward);
            transform.LookAt(transform.position + forward, normal);
            //Debug.DrawLine(transform.position, transform.position + forward, Color.red);
            //Debug.DrawLine(transform.position, transform.position + normal, Color.yellow);
        }
    }
}








