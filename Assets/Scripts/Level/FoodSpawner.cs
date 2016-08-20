using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour, IInitializable {

    public GameObject foodPrefab;
    public FoodPointer foodPointer;

    public static FoodSpawner instance { get; private set; }

    private Terrain terrain;

    // Enable editor deactivation
    void Update() { }

    public void Init() {
        Debug.Assert(instance == null);
        instance = this;

        Debug.Assert(foodPrefab != null);
        Debug.Assert(foodPointer != null);

        terrain = FindObjectOfType<Terrain>();
        Debug.Assert(terrain != null);


        SpawnNewFood();
    }

    public void SpawnNewFood() {
        var position = new Vector3();

        Vector3 size = terrain.terrainData.size;
        float padding = 1;
        position.x = Random.Range(padding, size.x - padding);
        position.z = Random.Range(padding, size.z - padding);

        // Get normalized terrain coordinates
        float x = position.x / size.x;
        float y = position.z / size.z;
        position.y = terrain.terrainData.GetInterpolatedHeight(x, y);

        var gameObject = Instantiate(foodPrefab, position, Quaternion.identity) as GameObject;
        foodPointer.food = gameObject.transform;
    }
}
