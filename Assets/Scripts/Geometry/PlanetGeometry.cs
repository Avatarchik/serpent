using UnityEngine;
using System.Collections;

namespace Snake3D.Geometry {

    /*
     * TODO:
     *   Properly dispose resources
     *   Interpolate height value in GetHeightAt()
     */

    //[ExecuteInEditMode]
    public class PlanetGeometry : MonoBehaviour {

        public const int kMaxSubdivisionLevel = 5;

        public UnityEngine.UI.Text textComponent;

        public int subdivisionLevel = 4;
        public float radius = 25;
        public float oceanDepth = 2, mountainHeight = 2;

        public int heightCubemapSize = 64;
        public double noiseScale = 0.5;
        public int seed = 123321;

        //[HideInInspector]
        public Cubemap heightMap;


        public float GetHeightAt(Vector3 radiusVector) {
            Color pixel = CubemapProjections.ReadPixel(heightMap, radiusVector);

            // Altitude above water level
            float heightDelta = pixel.r * (oceanDepth + mountainHeight) - oceanDepth;

            return radius + heightDelta;
        }

        void Reset() {
            Resources.UnloadUnusedAssets();
            if (heightMap != null)
                Object.DestroyImmediate(heightMap);

            GeneratePlanet();
        }

        // Use this for initialization
        void Awake() {
            GeneratePlanet();
        }

        void GeneratePlanet() {
            CheckAndCorrectInputs();

            Icosphere.Create(gameObject, subdivisionLevel, radius);

            // Generate and measure generation time
            float time = Time.realtimeSinceStartup;

            heightMap = GenerateHeightmap(seed);

            ApplyDisplacementMap();

            time = Time.realtimeSinceStartup - time;
            string text = string.Format("Generated in {0:N3} seconds", time);
            text += string.Format("{0:N3} {0:N3}", 0.123, 1234.0);
            textComponent.text += text;

            // Apply as texture
            Renderer renderer = gameObject.GetComponent<Renderer>();
            Material testMaterial = renderer.material;
            testMaterial.SetTexture("_HeightCubemap", heightMap);

            System.GC.Collect();
        }

        void ApplyDisplacementMap() {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            // Don't need to do error checks on MeshFilter and Mesh existence here

            Vector3[] vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; ++i) {
                float height = GetHeightAt(vertices[i]);
                vertices[i] = vertices[i].normalized * height;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
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
             * Steps:
             *   For each one of six sides:
             *     Generate the noise
             *     cubemap.SetPixels();
             */

            int savedSeed = Random.seed;

            int size = heightCubemapSize;
            Cubemap heightMap = new Cubemap(size, TextureFormat.RGB24, false);

            // Pixels are laid out right to left, top to bottom
            Color[] pixels = new Color[size * size];
            Color pixel = new Color();

            LibNoise.Generator.Perlin generator = new LibNoise.Generator.Perlin();
            generator.Frequency = this.noiseScale;

            // Iterate through all enum values (cube faces)
            foreach (CubemapFace face in System.Enum.GetValues(typeof(CubemapFace))) {
                for (int y = 0; y < size; ++y) {
                    for (int x = 0; x < size; ++x) {
                        Vector2 uv = new Vector2((float)x / (size - 1), (float)y / (size - 1));
                        Vector3 radiusVec = CubemapProjections.GetRadiusVectorFromFace(face, uv);

                        /*pixel.r = (radius.x + 1) * 0.5f;
                        pixel.g = (radius.y + 1) * 0.5f;
                        pixel.b = (radius.z + 1) * 0.5f;*/

                        float height = (float)generator.GetValue(radiusVec * this.radius);
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

        void Update() {
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

} // namespace Snake3D.Geometry
