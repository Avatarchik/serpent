using UnityEngine;
using System.Collections;
using RawImage = UnityEngine.UI.RawImage;

namespace Snake3D {

    [ExecuteInEditMode]
    public class TiledBackground : MonoBehaviour {

        public float speed = 0.1f;

        RawImage rawImage;
        RectTransform rectTransform;
        int textureSize;    // Assuming square texture
        float offset = 0;

	    // Use this for initialization
	    void Start () {
            rawImage = GetComponent<RawImage>();
            rectTransform = GetComponent<RectTransform>();

            textureSize = rawImage.texture.width;
	    }
	
	    // Update is called once per frame
	    void Update() {
            offset += speed * Time.deltaTime;
            if (offset >= 1)
                offset -= 1;

            Rect rtRect = rectTransform.rect;
            rawImage.uvRect = new Rect(
                0,
                offset,
                rtRect.width / textureSize,
                rtRect.height / textureSize
                );
	    }
    }

} // namespace Snake3D
