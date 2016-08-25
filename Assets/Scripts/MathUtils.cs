using UnityEngine;
using System.Collections;

namespace Snake3D {

    public static class MathUtils {
        
        /// Calculates angle of the vector in degrees. Angle belongs to [0; 360)
        public static float GetAngle(this Vector2 vector) {
            float angle = Vector2.Angle(Vector2.right, vector);
            if (vector.y < 0)
                angle = 360 - angle;

            if (angle >= 360)
                angle -= 360;

            return angle;
        }
    }

} // namespace Snake3D
