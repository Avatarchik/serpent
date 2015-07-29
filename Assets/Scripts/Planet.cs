using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class Planet : MonoBehaviour {

    public const int kMaxSubdivisionLevel = 5;

    public int subdivisionLevel = 4;
    public float radius = 25;

    //[HideInInspector]
    public Cubemap heightMap;

    void Reset() {
        Resources.UnloadUnusedAssets();
        if (heightMap != null)
            Object.DestroyImmediate(heightMap);

        Start();
    }

	// Use this for initialization
    void Start() {
        CheckAndCorrectInputs();

        Icosphere.Create(gameObject, subdivisionLevel, radius);

        heightMap = GenerateHeightmap(123);

        // Test
        Renderer renderer = gameObject.GetComponent<Renderer>();
        Material testMaterial = renderer.sharedMaterial;
        testMaterial.SetTexture("_HeightCubemap", heightMap);

        System.GC.Collect();
	}

    void CheckAndCorrectInputs() {
        bool warn = false;
        if (subdivisionLevel < 0) {
            subdivisionLevel = 0;
            warn = true;
        } else if (subdivisionLevel > kMaxSubdivisionLevel) {
            subdivisionLevel = kMaxSubdivisionLevel;
            warn = true;
        }
        if (warn)
            Debug.LogWarning(string.Format("Subdivision Level must be in [0; {0}]", kMaxSubdivisionLevel));
    }

    Cubemap GenerateHeightmap(int seed) {
        /*
         * Шаги:
         *   Для каждой из шести сторон:
         *     Сгенерировать шум
         *     cubemap.SetPixels();
         */

        int savedSeed = Random.seed;

        int size = 128;
        Cubemap heightMap = new Cubemap(size, TextureFormat.RGB24, false);

        // Pixels are laid out right to left, top to bottom
        Color[] pixels = new Color[size * size];
        Color pixel = new Color();

        LibNoise.ModuleBase generator = new LibNoise.Generator.Perlin();

        // Iterate through all enum values (cube faces)
        foreach (CubemapFace face in System.Enum.GetValues(typeof(CubemapFace))) {
            for (int y = 0; y < size; ++y) {
                for (int x = 0; x < size; ++x) {
                    Vector2 uv = new Vector2((float) x / (size-1), (float) y / (size-1));
                    Vector3 radius = CubemapUtils.CalculateRadiusFromFaceCoords(face, uv);

                    /*pixel.r = (radius.x + 1) * 0.5f;
                    pixel.g = (radius.y + 1) * 0.5f;
                    pixel.b = (radius.z + 1) * 0.5f;*/

                    float height = (float) generator.GetValue(radius);
                    height = (height + 1) * 0.5f;
                    pixel.r = pixel.g = pixel.b = height;

                    pixels[x + y * size] = pixel;
                }
            }

            heightMap.SetPixels(pixels, face);
            heightMap.Apply();
        }


        Random.seed = savedSeed;

        return heightMap;
    }
	
#if UNITY_EDITOR
    // Watch for properties change in editor

    private int _lastSubdivisionLevel;
    private bool _lastValuesInitialized = false;

    void Update () {
        if (!_lastValuesInitialized) {
            _lastValuesInitialized = true;
            _lastSubdivisionLevel = subdivisionLevel;
            return;
        }

        if (_lastSubdivisionLevel != subdivisionLevel) {
            Reset();

            _lastSubdivisionLevel = subdivisionLevel;
        }
    }
#endif // UNITY_EDITOR
}
