using UnityEngine;

namespace Snake3D {

    public static class Utils {

        public static T FindNearestParentWithComponent<T>(Transform trans) where T : Component {
            for (; trans != null; trans = trans.parent) {
                T component = trans.GetComponent<T>();
                if (component != null)
                    return component;
            }

            return null;
        }

        public static Vector2 GetUnscaledTextureOffset(this Material material) {
            Vector2 result = material.mainTextureOffset;
            Vector2 scale = material.mainTextureScale;
            result.x /= scale.x;
            result.y /= scale.y;
            return result;
        }

        public static void SetUnscaledTextureOffset(this Material material, Vector2 offset) {
            Vector2 result = offset;
            result.Scale(material.mainTextureScale);
            material.mainTextureOffset = result;
        }

        public static void DrawGradientLine(Vector3 start, Vector3 end, Color startColor, Color endColor,
            bool depthTest = false, float duration = 0) {

            const int kSteps = 8;
            Vector3 direction = end - start;

            for (int i = 0; i < kSteps; i++) {
                float factor = (float)i / (kSteps - 1);
                Color color = Color.Lerp(startColor, endColor, factor);

                Vector3 a = start + direction * ((float)i / kSteps);
                Vector3 b = start + direction * ((float)(i + 1) / kSteps);
                Debug.DrawLine(a, b, color);
            }
        }
    }

} // namespace Snake3D
