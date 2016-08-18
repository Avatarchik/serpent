using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SnakeHead : MonoBehaviour {

    public GrowController growController;
    public float growDelta = 4;

    private AudioSource audioSource;

    void Start () {
        Debug.Assert(growController != null);

        audioSource = GetComponent<AudioSource>();
	}

    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Food")) {
            // TODO: game over
            return;
        }

        OnFoodPickedUp(other.transform.parent.gameObject);
    }

    private void OnFoodPickedUp(GameObject food) {
        Destroy(food);
        audioSource.Play();

        FoodSpawner.instance.SpawnNewFood();
        // Grow snake
        growController.targetLength += growDelta;
    }
}
