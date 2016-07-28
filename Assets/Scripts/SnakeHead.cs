using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SnakeHead : MonoBehaviour {

    public GameObject foodPrefab;
    public GrowController growController;
    public float growDelta = 4;
    public FoodPointer foodPointer;

    private Terrain terrain;
    private AudioSource audioSource;

    void Start () {
        Debug.Assert(foodPrefab != null);
        Debug.Assert(growController != null);
        Debug.Assert(foodPointer != null);

        terrain = FindObjectOfType<Terrain>();
        Debug.Assert(terrain != null);
        audioSource = GetComponent<AudioSource>();

        SpawnNewFood();
	}

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Food"))
            // TODO: game over
            return;
        
        Destroy(other.transform.parent.gameObject);
        audioSource.Play();

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

        var gameObject = Instantiate(foodPrefab, position, Quaternion.identity) as GameObject;
        foodPointer.food = gameObject.transform;
    }
}
