using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform), typeof(Image))]
public class FoodPointer : MonoBehaviour {

    public Transform food;
    public float margin = 48;

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;
    private Behaviour image;

    void Start () {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
        Debug.Assert(parentRectTransform != null);
        image = GetComponent<Image>();
    }
	
	void Update () {
        if (food == null)
            return;

        // Get food position in screen space
        Camera camera = Camera.main;
        Vector2 position = camera.WorldToScreenPoint(food.position);

        image.enabled = !camera.pixelRect.Contains(position);


        // Align to screen border
        position -= 0.5f * camera.pixelRect.size;
        Rect parentRect = parentRectTransform.rect;
        position.x = Mathf.Clamp(position.x, parentRect.xMin + margin, parentRect.xMax - margin);
        position.y = Mathf.Clamp(position.y, parentRect.yMin + margin, parentRect.yMax - margin);
        rectTransform.anchoredPosition = position;

        // Rotate
        float angle = Vector2.Angle(Vector2.right, position);
        if (position.y < 0)
            angle = -angle;
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
