using UnityEngine;
using System.Collections;

namespace Snake3D {

    /*
     * TODO:
     *   Properly dispose resources
     *   Interpolate height value in GetHeightAt()
     */
    
    //[ExecuteInEditMode]
    [RequireComponent(typeof(MeshFilter), typeof(Renderer))]
    public class PlanetGeometry : MonoBehaviour {

        public const int kMaxSubdivisionLevel = 5;

        public UnityEngine.UI.Text textComponent;

        public int subdivisionLevel = 4;
        public float radius = 25;
        public float oceanDepth = 2, mountainHeight = 2;

        public int heightCubemapSize = 64;
        public double noiseScale = 0.5;
        public int octaves = 3;
        public int seed = 123321;

        //[HideInInspector]
        public Cubemap heightMap;

        // Octree with triangle bounds
        public BoundsOctree<int> Octree;
        public bool showOctreeNodeBounds = false;
        public bool showOctreeObjectBounds = true;


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

        void Reset() {
            Resources.UnloadUnusedAssets();
            if (heightMap != null)
                DestroyImmediate(heightMap);

            GeneratePlanet();
        }

        // Use this for initialization
        void Awake() {
            GeneratePlanet();
        }

        void OnDrawGizmos() {
            if (Octree == null)
                return;

            if (showOctreeNodeBounds)
                Octree.DrawAllBounds();

            if (showOctreeObjectBounds)
                Octree.DrawAllObjects();

            //octree.DrawCollisionChecks(); // Draw the last *numCollisionsToSave* collision check boundaries\
        }

        public float GetHeightAt(Vector3 radiusVector) {
            Color pixel = CubemapProjections.ReadPixel(heightMap, radiusVector);

            // Altitude above water level
            float heightDelta = pixel.r * (oceanDepth + mountainHeight) - oceanDepth;

            return radius + heightDelta;
        }


        #region Private part

        private void GeneratePlanet() {
            CheckAndCorrectInputs();

            Icosphere.Create(gameObject, subdivisionLevel, radius);

            // Generate and measure generation time
            float startTime = Time.realtimeSinceStartup;
            {
                heightMap = GenerateHeightmap(seed);
                ApplyDisplacementMap();
            }

            ApplyHeightmapAsTexture();
            GenerateOctree();

            // Print performance info
            {
                float elapsedTime = Time.realtimeSinceStartup - startTime;
                textComponent.text += string.Format("Generated in {0:N3} seconds\n", elapsedTime);
            }

            System.GC.Collect();
        }

        private void GenerateOctree() {
            Octree = new BoundsOctree<int>(64, Vector3.zero, 1, 1.25f);

            Mesh mesh = GetComponent<MeshFilter>().mesh;
            Vector3[] vertices = mesh.vertices;
            int[] triangles = mesh.triangles;
            for (int triangle = 0; triangle < triangles.Length / 3; ++triangle) {
                int offset = triangle * 3;
                Vector3 v1 = vertices[triangles[offset + 0]];
                Vector3 v2 = vertices[triangles[offset + 1]];
                Vector3 v3 = vertices[triangles[offset + 2]];
                Bounds aabb = new Bounds(v1, Vector3.zero);
                aabb.Encapsulate(v2);
                aabb.Encapsulate(v3);
                Octree.Add(triangle, aabb);
            }
        }

        private void ApplyHeightmapAsTexture() {
            Material material = GetComponent<Renderer>().material;
            material.SetTexture("_HeightCubemap", heightMap);
        }

        private void ApplyDisplacementMap() {
            Mesh mesh = GetComponent<MeshFilter>().mesh;

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

        private void CheckAndCorrectInputs() {
            bool warn = false;
            if (subdivisionLevel < 0) {
                subdivisionLevel = 0;
                warn = true;
            }
            else if (subdivisionLevel > kMaxSubdivisionLevel) {
                subdivisionLevel = kMaxSubdivisionLevel;
                warn = true;
            }
            if (warn)
                Debug.LogWarning(string.Format("Subdivision Level must be in [0; {0}]", kMaxSubdivisionLevel));
        }

        private Cubemap GenerateHeightmap(int seed) {
            /*
             * Steps:
             *   For each one of six sides:
             *     Generate the noise
             *     cubemap.SetPixels();
             */

            Random.State savedState = Random.state;

            int size = heightCubemapSize;
            Cubemap heightMap = new Cubemap(size, TextureFormat.RGB24, false);

            // Pixels are laid out right to left, top to bottom
            Color[] pixels = new Color[size * size];
            Color pixel = new Color();

            var generator = new LibNoise.Generator.Perlin();
            generator.Frequency = noiseScale;
            generator.OctaveCount = octaves;
            generator.Seed = seed;

            // Iterate through all enum values (cube faces)
            foreach (CubemapFace face in System.Enum.GetValues(typeof(CubemapFace))) {
                if (face == CubemapFace.Unknown)
                    continue;

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


            Random.state = savedState;

            return heightMap;
        }

        #endregion Private part
    }

} // namespace Snake3D
