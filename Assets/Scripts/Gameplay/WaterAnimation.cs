using UnityEngine;
using System.Collections;

namespace Snake3D {

    public class WaterAnimation : MonoBehaviour {

        public float speed1 = 0.05f;
        public float speed2 = 0.001f;
        public float angle1 = -15;
        public float angle2 = 45;

        private Material _material;
        private Vector2 _direction1;
        private Vector2 _direction2;

        void Start() {
            _material = GetComponent<Renderer>().material;

            _direction1 = new Vector2(Mathf.Cos(angle1) * speed1, Mathf.Sin(angle1) * speed1);
            _direction2 = new Vector2(Mathf.Cos(angle2) * speed2, Mathf.Sin(angle2) * speed2);
        }

        void Update() {
#if UNITY_EDITOR
            _direction1 = new Vector2(Mathf.Cos(angle1) * speed1, Mathf.Sin(angle1) * speed1);
            _direction2 = new Vector2(Mathf.Cos(angle2) * speed2, Mathf.Sin(angle2) * speed2);
#endif

            Vector2 offsetVector = _material.GetTextureOffset("_MainTex");
            offsetVector = AnimateVector(offsetVector, _direction1);
            _material.SetTextureOffset("_MainTex", offsetVector);

            offsetVector = _material.GetTextureOffset("_DetailAlbedoMap");
            offsetVector = AnimateVector(offsetVector, _direction2);
            _material.SetTextureOffset("_DetailAlbedoMap", offsetVector);
        }

        // Increments offset vector over time and keeps x and y in [0; 1).
        // Argument "direction" should be multiplied by speed.
        private Vector2 AnimateVector(Vector2 vec, Vector2 direction) {
            vec += direction * Time.deltaTime;
            vec.x = Mathf.Repeat(vec.x, 1);
            vec.y = Mathf.Repeat(vec.y, 1);
            return vec;
        }
    }

} // namespace Snake3D
