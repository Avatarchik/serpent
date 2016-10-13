using UnityEngine;
using System.Collections;
using System;

namespace Serpent {

    public class Food : MonoBehaviour {

        [NotNull] public Transform movingPart;
        public float verticalAmplitude = 1;
        public float verticalPeriod = 4;
        public float rotationPeriod = 50;

        [NotNull] public Transform highlightField;
        [NotNull] public Projector projector;
        public float highlightScalePeriod = 1;
        public float maxHighlightScale = 1.3f;

        private Vector3 initialPosition;
        private Vector3 initialHighlightScale;
        private float initialProjectorSize;

        void Start() {
            initialPosition = movingPart.localPosition;
            initialHighlightScale = highlightField.localScale;
            initialProjectorSize = projector.orthographicSize;
        }

        void Update() {
            AnimateMovingPart();
            AnimateHighlight();
        }

        private void AnimateHighlight() {
            float t = Mathf.PingPong(2 * Time.time / highlightScalePeriod, 1);
            float factor = Mathf.SmoothStep(1, maxHighlightScale, t);
            highlightField.localScale = initialHighlightScale * factor;
            projector.orthographicSize = initialProjectorSize * factor;
        }

        private void AnimateMovingPart() {
            // Translate
            Vector3 delta = new Vector3();
            delta.y = Mathf.Sin(2 * Mathf.PI * Time.time / verticalPeriod) * verticalAmplitude;
            movingPart.localPosition = initialPosition + delta;

            // Rotate
            float phase = Time.deltaTime / rotationPeriod;
            movingPart.Rotate(360 * phase * 5, 360 * phase * 11, 360 * phase * 7);
        }
    }

} // namespace Serpent
