using UnityEngine;

public class enemy : MonoBehaviour
{
    public int health = 5; //definitely change this later
    public const int WEAKNESS = 1; //Fire? Water? Earth? Wind? Light? Dark?
    public BoxCollider movementBounds; // Assign a BoxCollider in the Inspector
    public float speed = 2f; // Movement speed
    private Vector3 minBounds, maxBounds;
    private bool movingUp = true;

    void Start()
    {
        if (movementBounds == null)
        {
            Debug.LogError("Movement Bounds not set! Please assign a BoxCollider.");
            return;
        }

        // Calculate the min and max Y bounds based on the BoxCollider
        minBounds = new Vector3(transform.position.x, movementBounds.bounds.min.y, transform.position.z);
        maxBounds = new Vector3(transform.position.x, movementBounds.bounds.max.y, transform.position.z);
    }

    void Update()
    {
        if (movementBounds == null) return;

        // Move the enemy up and down within the bounds
        float step = speed * Time.deltaTime;
        transform.position += (movingUp ? Vector3.up : Vector3.down) * step;

        // Check if the enemy reached the top or bottom bounds
        if (transform.position.y >= maxBounds.y)
            movingUp = false;
        else if (transform.position.y <= minBounds.y)
            movingUp = true;
    }
}