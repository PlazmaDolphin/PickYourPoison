using UnityEngine;
using System.Collections;
using UnityEditor.Tilemaps;
using UnityEditor.Rendering;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public Transform cursorLocation;
    public Animator animator;
    public static Transform PlayerLocation;
    public GameObject fireballPrefab;

<<<<<<< HEAD
    // You can remove these if you want the player to move freely,
    // or adjust them dynamically based on the camera.
    // public float minX = -7.91f, maxX = 8.08f;
    // public float minY = -4.96f, maxY = 2.2f;

=======
    // FIXME bounding box
    private float minX = -8f, maxX = 8f;
    private float minY = -5f, maxY = 2.3f;
>>>>>>> b6efd02a994833c85c5c23111ce93386fc5eef9b
    private bool flipped = false;

    // Punching
    public float punchRange = 1f; // How far the punch reaches
    public float punchDuration = 0.5f; // Duration of the punch
    private bool isPunching = false; // Check if the player is currently punching
    public int potionType = 0; // (0 = no potion)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerLocation = transform; // Set static reference
    }

    void Update()
    {
        // WASD movement
        movement.x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        movement.y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }

        // Flip sprite based on cursor location
        if (cursorLocation.position.x < transform.position.x && !flipped)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            flipped = true;
        }
        else if (cursorLocation.position.x >= transform.position.x && flipped)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            flipped = false;
        }

        PlayerLocation = transform; // Update static reference

        if (Input.GetKeyDown(KeyCode.Space) && !isPunching)
        {
            StartCoroutine(Punch());
        }
<<<<<<< HEAD

=======
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            //spawn fireball
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            fireball.GetComponent<fireball>().direction = Mathf.Atan2(cursorLocation.position.y - transform.position.y, cursorLocation.position.x - transform.position.x);
            Physics2D.IgnoreCollision(fireball.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
            //move up a little
            fireball.transform.position += new Vector3(0, 1f, 0);
        }
        //Update animation
>>>>>>> b6efd02a994833c85c5c23111ce93386fc5eef9b
        animator.SetFloat("speed", movement.magnitude);
        animator.SetBool("punching", isPunching);
    }

    void FixedUpdate()
    {
        // Apply movement freely; remove clamping if you want the player to use the full screen
        rb.linearVelocity = movement * moveSpeed;
    }

    // Simple punch mechanic (expand as needed)
    IEnumerator Punch()
    {
        isPunching = true;

        // Example punch logic; you can expand this as needed
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (flipped ? Vector2.left : Vector2.right), punchRange);
        if (hit.collider != null)
        {
            Debug.Log("Punch hit: " + hit.collider.name);
        }

        yield return new WaitForSeconds(punchDuration);
        isPunching = false;
    }

    public void AddPotion(int potionType)
    {
        this.potionType = potionType;
        // Update animation or other logic here
    }
}
