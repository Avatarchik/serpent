using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Snake3D {

    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

        // Normalized vector
        public Vector2 value {
            get { return _value; }
        }
        public float maxStickDistance = 58;


        Vector2 _value = new Vector2();
        UnityEngine.UI.Image _joystickBackground;
        UnityEngine.UI.Image _joystickCenter;
        Camera _myCamera;


        void Start() {
            _joystickBackground = GetComponent<UnityEngine.UI.Image>();
            _joystickCenter = transform.Find("Joystick Center").GetComponent<UnityEngine.UI.Image>();

            // What camera to use?
            Canvas canvas = Utilities.FindNearestParentWithComponent<Canvas>(transform);
            if (canvas.renderMode == RenderMode.WorldSpace || canvas.renderMode == RenderMode.ScreenSpaceCamera)
                _myCamera = canvas.worldCamera;
            else
                _myCamera = null;
        }

        public void OnPointerDown(PointerEventData eventData) {
            // Checks if click is out of joystick area
            Vector2 stickPosition = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _joystickBackground.rectTransform,
                eventData.position,
                _myCamera,
                out stickPosition
            );
            if (stickPosition.magnitude > _joystickBackground.rectTransform.rect.width / 2) {
                // Ignore event
                return;
            }

            UpdateValue(eventData.position);
        }

        public void OnDrag(PointerEventData eventData) {
            UpdateValue(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData) {
            _value.Set(0, 0);
            _joystickCenter.rectTransform.anchoredPosition = _value;
        }


        void UpdateValue(Vector2 pointerCoords) {
            Vector2 stickPosition = new Vector2();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _joystickBackground.rectTransform,
                pointerCoords,
                _myCamera,
                out stickPosition
            );

            Vector2 direction = stickPosition.normalized;
            float magnitude = stickPosition.magnitude;

            if (magnitude > maxStickDistance) {
                magnitude = maxStickDistance;
            }
            _joystickCenter.rectTransform.anchoredPosition = direction * magnitude;
            _value = direction * magnitude / maxStickDistance;
        }
    }

} // namespace Snake3D


