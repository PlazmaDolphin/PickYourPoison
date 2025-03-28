using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 movement;

    // Define boundary limits (adjust these based on your box size)
    public float minX = -5f, maxX = 5f;
    public float minY = -5f, maxY = 5f;

    private Quaternion originalRotation; // Store the original rotation
    private bool isRotating = false;  // To check if already rotating

    // Punching
    public float punchRange = 1f; // How far the punch reaches
    public float punchDuration = 0.2f; // Duration of the punch
    private bool isPunching = false; // Check if the player is currently punching

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
        originalRotation = transform.rotation; // Store the initial rotation of the player
    }

    void Update()
    {
        // Get input for movement
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize movement to ensure consistent speed in all directions
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        // Flip the character based on the horizontal movement (left or right)
        if (movement.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = movement.x > 0 ? 0.4329175f : -0.4329175f; // Flip the sprite when moving left or right
            scale.y = 0.3069777f; // possible FIXME
            transform.localScale = scale;
        }

        // Trigger attack rotation on left-click (no mouse influence on rotation)
        if (Input.GetMouseButtonDown(0) && !isRotating)  // 0 is for left-click
        {
            StartCoroutine(RotateAndSnapBack()); // Start the attack rotation coroutine
        }

        // Trigger punch when player presses the "Fire1" (usually left mouse or Ctrl)
        if (Input.GetButtonDown("Fire1") && !isPunching)
        {
            StartCoroutine(Punch());
        }
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = movement * moveSpeed;

        // Clamp position inside the box
        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        rb.position = clampedPosition;
    }

    IEnumerator RotateAndSnapBack()
    {
        // Mark the player as currently rotating
        isRotating = true;

        // Calculate a 45-degree rotation
        float targetAngle = transform.localScale.x > 0 ? -45f : 45f; // -45 for right, 45 for left (flipped)

        // Smoothly rotate to the target angle (45 degrees increment)
        float elapsedTime = 0f;
        float rotationDuration = 0.2f; // Duration to rotate to the target angle
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation; // Ensure the final rotation is exactly the target angle (-45 or 45 degrees)

        // Short wait before snapping back to original rotation
        yield return new WaitForSeconds(0.1f);  // You can adjust this duration as needed

        // Smoothly snap back to the original rotation
        elapsedTime = 0f;
        float snapDuration = 0.2f; // Duration for snapping back to original rotation

        while (elapsedTime < snapDuration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, elapsedTime / snapDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = originalRotation; // Ensure the rotation is exactly the original after the transition

        // Mark the player as not rotating anymore
        isRotating = false;
    }

    // Punch Mechanic
    IEnumerator Punch()
    {
        isPunching = true;

        // Determine punch direction based on the current facing direction (leftmost side)
        Vector2 punchDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;  // Punch in the direction of the leftmost side
        Vector2 punchPosition = (Vector2)transform.position + punchDirection * punchRange;

        // Use a collider (e.g., box or circle) to check for enemies within the punch range
        RaycastHit2D hit = Physics2D.Raycast(transform.position, punchDirection, punchRange);

        // If the ray hits something, process the hit (e.g., deal damage)
        if (hit.collider != null)
        {
            Debug.Log("Punch hit: " + hit.collider.name);
            // You can add logic here for dealing damage, playing animations, etc.
        }

        // Optionally, you can trigger a punch animation here if you have one.
        // Example: animator.SetTrigger("Punch");

        // Wait for the punch to complete
        yield return new WaitForSeconds(punchDuration);

        isPunching = false;
    }

    public void AddPotion(int potiontype)
    {

    }
}
