using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Transform cursorLocation;
    public Animator animator;
    public GameObject fireballPrefab;
    //public powerBar theBarofPower;

    // Bounding box for movement (optional)
    private float minX = -8f, maxX = 8f;
    private float minY = -5f, maxY = 2.3f;
    private bool flipped = false;

    // Punching
    public float punchRange = 1f; // How far the punch reaches
    public float punchDuration = 0.5f; // Duration of the punch
    private bool isPunching = false; // Check if the player is currently punching
    public int potionType = 0; // (0 = no potion)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Handle WASD movement
        movement.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        movement.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        if (movement.magnitude > 1)
        {
            movement = movement.normalized; // Normalize diagonal movement
        }

        // Flip sprite based on cursor location
        if (cursorLocation.position.x < transform.position.x && !flipped ||
            cursorLocation.position.x > transform.position.x && flipped)
        {
            flipped = !flipped;
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        // Punch logic
        if (Input.GetKeyDown(KeyCode.Space) && !isPunching)
        {
            StartCoroutine(Punch());
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && potionType != 0){
            //theBarofPower.AddPotion(potionType);
            potionType = 0;
            animator.SetTrigger("PotionLose");
            //spawn fireball
            /*
            transform.position += new Vector3(0, 1f, 0);
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            fireball.GetComponent<Fireball>().direction = Mathf.Atan2(cursorLocation.position.y - transform.position.y, cursorLocation.position.x - transform.position.x);
            Physics2D.IgnoreCollision(fireball.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
            // Revert position after spawning fireball
            transform.position -= new Vector3(0, 1f, 0);
            potionType = 0;
            animator.SetTrigger("PotionLose");
            */
        }
        if (Input.GetKeyDown(KeyCode.Q) && potionType != 0)
        {
            // Use power
           // theBarofPower.usePower(transform, cursorLocation, GetComponent<Collider2D>());
        }

        // Update animation
        animator.SetFloat("speed", movement.magnitude);
        animator.SetBool("punching", isPunching);
    }

    void FixedUpdate()
    {
        // Apply movement
        rb.linearVelocity = movement * moveSpeed;

        // Optional: Clamp player's position within bounding box
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }

    IEnumerator Punch()
    {
        isPunching = true;

        // Example punch logic with Raycast
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            flipped ? Vector2.left : Vector2.right,
            punchRange
        );

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("Punch hit: " + hit.collider.name);
            hit.collider.GetComponent<Enemy>().TakeDamage(); // Call TakeDamage on the enemy
        }

        yield return new WaitForSeconds(punchDuration);
        isPunching = false;
    }

    private void SpawnFireball()
    {
        // Create fireball and set its direction
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        fireball.GetComponent<Fireball>().direction = Mathf.Atan2(
            cursorLocation.position.y - transform.position.y,
            cursorLocation.position.x - transform.position.x
        );

        // Ignore collision with the player
        Physics2D.IgnoreCollision(
            fireball.GetComponent<CircleCollider2D>(),
            GetComponent<BoxCollider2D>()
        );
    }

    public void AddPotion(int potionType)
    {
        this.potionType = potionType;
        animator.SetTrigger("PotionGet");
    }
}
