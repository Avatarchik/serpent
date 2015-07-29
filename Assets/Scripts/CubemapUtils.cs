using UnityEngine;
using System.Collections;

public static class CubemapUtils {

    public static Vector3 CalculateRadiusFromFaceCoords(CubemapFace face, Vector2 uv) {
        Vector3 radius = new Vector3();
        uv = uv * 2 - Vector2.one;
        
        switch (face) {
            case CubemapFace.PositiveX:
                radius.Set(1, -uv.y, -uv.x);
                break;
            case CubemapFace.PositiveY:
                radius.Set(uv.x, 1, uv.y);
                break;
            case CubemapFace.PositiveZ:
                radius.Set(uv.x, -uv.y, 1);
                break;

            case CubemapFace.NegativeX:
                radius.Set(-1, -uv.y, uv.x);
                break;
            case CubemapFace.NegativeY:
                radius.Set(uv.x, -1, -uv.y);
                break;
            case CubemapFace.NegativeZ:
                radius.Set(-uv.x, -uv.y, -1);
                break;
        }

        return radius.normalized;
    }
}
