using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

namespace Snake3D {

    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

        // Normalized vector
        public Vector2 Value { get; private set; }
        public float Angle { get; private set; }
        public float maxStickDistance = 58;
        
        UnityEngine.UI.Image joystickBackground;
        UnityEngine.UI.Image joystickCenter;
        Camera myCamera;


        void Start() {
            Value = Vector2.zero;
            joystickBackground = GetComponent<UnityEngine.UI.Image>();
            joystickCenter = transform.Find("Joystick Center").GetComponent<UnityEngine.UI.Image>();

            // What camera to use?
            Canvas canvas = Utils.FindNearestParentWithComponent<Canvas>(transform);
            if (canvas.renderMode == RenderMode.WorldSpace || canvas.renderMode == RenderMode.ScreenSpaceCamera)
                myCamera = canvas.worldCamera;
            else
                myCamera = null;
        }

        public void OnPointerDown(PointerEventData eventData) {
            // Checks if click is out of joystick area
            Vector2 stickPosition = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBackground.rectTransform,
                eventData.position,
                myCamera,
                out stickPosition
            );
            if (stickPosition.magnitude > joystickBackground.rectTransform.rect.width / 2) {
                // Ignore event
                return;
            }

            UpdateValue(eventData.position);
        }

        public void OnDrag(PointerEventData eventData) {
            UpdateValue(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData) {
            Value = Vector2.zero;
            joystickCenter.rectTransform.anchoredPosition = Value;
        }


        void UpdateValue(Vector2 pointerCoords) {
            Vector2 stickPosition = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickBackground.rectTransform,
                pointerCoords,
                myCamera,
                out stickPosition
            );

            Vector2 direction = stickPosition.normalized;
            float magnitude = stickPosition.magnitude;

            if (magnitude > maxStickDistance) {
                magnitude = maxStickDistance;
            }
            joystickCenter.rectTransform.anchoredPosition = direction * magnitude;
            Value = direction * magnitude / maxStickDistance;

            UpdateAngle();
        }

        private void UpdateAngle() {
            float angle = Vector2.Angle(Vector2.right, Value);
            if (Value.y < 0)
                angle = 360 - angle;

            Angle = angle;
        }
    }

} // namespace Snake3D


