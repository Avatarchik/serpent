using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour {

    public Mesh sourceMesh;
    public Cubemap cubeMap;

    public GameObject testObject;

	// Use this for initialization
	void Start () {
        GenerateHeightmap(123);
	}

    void GenerateHeightmap(int seed) {
        int savedSeed = Random.seed;

        int size = 128;
        cubeMap = new Cubemap(size, TextureFormat.RGB24, false);

        // Pixels are laid out right to left, top to bottom
        Color[] pixels = new Color[size * size];
        Color pixel = new Color();
        for (int y = 0; y < size; ++y) {
            for (int x = 0; x < size; ++x) {
                pixel.r = (float)x / size;
                pixel.g = (float)y / size;
                pixel.b = 0;
                pixels[x + y * size] = pixel;
            }
        }

        cubeMap.SetPixels(pixels, CubemapFace.NegativeX);
        cubeMap.SetPixels(pixels, CubemapFace.NegativeY);
        cubeMap.SetPixels(pixels, CubemapFace.NegativeZ);
        cubeMap.SetPixels(pixels, CubemapFace.PositiveX);
        cubeMap.SetPixels(pixels, CubemapFace.PositiveY);
        cubeMap.SetPixels(pixels, CubemapFace.PositiveZ);

        cubeMap.Apply();

        Renderer renderer = testObject.GetComponent<Renderer>();
        Material testMaterial = renderer.material;
        testMaterial.SetTexture("_HeightCubemap", cubeMap);

        Random.seed = savedSeed;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
