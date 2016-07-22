using UnityEngine;

public class SnakeHead : MonoBehaviour {

    public GameObject foodPrefab;
    public GrowController growController;
    public float growDelta = 2;

    private Terrain terrain;

    void Start () {
        Debug.Assert(foodPrefab != null);
        Debug.Assert(growController != null);

        terrain = FindObjectOfType<Terrain>();
        Debug.Assert(terrain != null);

        SpawnNewFood();
	}
	
	void Update () {
	
	}

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Food"))
            // TODO: game over
            return;
        
        Destroy(other.transform.parent.gameObject);

        SpawnNewFood();
        GrowSnake();
    }

    private void GrowSnake() {
        growController.targetLength += growDelta;
    }

    private void SpawnNewFood() {
        var position = new Vector3();

        Vector3 size = terrain.terrainData.size;
        float padding = 1;
        position.x = Random.Range(padding, size.x - padding);
        position.z = Random.Range(padding, size.z - padding);

        // Get normalized terrain coordinates
        float x = position.x / size.x;
        float y = position.z / size.z;
        position.y = terrain.terrainData.GetInterpolatedHeight(x, y);

        Instantiate(foodPrefab, position, Quaternion.identity);
    }
}
