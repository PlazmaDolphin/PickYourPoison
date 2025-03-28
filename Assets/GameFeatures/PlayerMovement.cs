using UnityEngine;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEditor.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Transform cursorLocation;
    public Animator animator;

    // FIXME bounding box
    public float minX = -7.91f, maxX = 8.08f;
    public float minY = -4.96f, maxY = 2.51f;
    private bool flipped = false;

    // Punching
    public float punchRange = 1f; // How far the punch reaches
    public float punchDuration = 0.5f; // Duration of the punch
    private bool isPunching = false; // Check if the player is currently punching
    public int potionType = 0; // (0 = no potion)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
    }

    void Update()
    {
        // Get input for movement
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");
        //WASD movement
        movement.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        movement.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        // Normalize movement to ensure consistent speed in all directions
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }
        //Flip if mouse is to left of player
        if (cursorLocation.position.x < transform.position.x && !flipped)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            flipped = true;
        }
        else if (cursorLocation.position.x > transform.position.x && flipped)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            flipped = false;
        }


        // Trigger punch when player presses the "Fire1" (usually left mouse or Ctrl)
        if (Input.GetKeyDown(KeyCode.Space) && !isPunching)
        {
            StartCoroutine(Punch());
            
        }
        //Update animation
        animator.SetFloat("speed", movement.magnitude);
        animator.SetBool("punching", isPunching);
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
    public void AddPotion(int potionType)
    {
        this.potionType = potionType;
        //UPDATE ANIMATION HERE
    }
}
