using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Snake3D {

    public class TerrainWalker : MonoBehaviour {

        public Joystick joystick;
        public float offsetFromSurface = 1;
        public float speed = 8;
        public float rotationSpeed = 180; // degrees per second

        private Terrain terrain;
        private float currentAngle = 0;

        void Start() {
            terrain = FindObjectOfType<Terrain>();
            if (terrain == null)
                throw new UnityException("Terrain object not found in current scene");

            CastToSurface();
        }

        void Update() {
            // Rotate
            float rotationStep = rotationSpeed * Time.deltaTime;
            RotateUpToAngle(-joystick.Angle + 90, rotationStep);

            // Translate
            float step = speed * Time.deltaTime;
            transform.Translate(0, 0, step);

            CastToSurface();
        }

        /**
         * <param name="target">Target angle</param>
         * <param name="absoluteStep">Step without sign</param>
         */
        private void RotateUpToAngle(float target, float absoluteStep) {
            float delta = Mathf.DeltaAngle(currentAngle, target);
            float sign = Mathf.Sign(delta);
            
            float step = sign * Mathf.Min(Mathf.Abs(delta), absoluteStep);

            currentAngle += step;
            transform.eulerAngles = new Vector3(0, currentAngle, 0);
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








