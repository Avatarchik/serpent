using UnityEngine;
using System.Collections;

public static class Utils {
    public static Color VectorToColor(Vector3 vec) {
        return new Color(vec.x, vec.y, vec.z);
    }
}
