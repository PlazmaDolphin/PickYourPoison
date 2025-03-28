using UnityEngine;
using UnityEngine.Rendering;

public class LightningBolt: MonoBehaviour
{
    public float direction = 0f;
    public CircleCollider2D circle;
    private Vector2 initPos;
    public const float RANGE = 10f;
    public const float SPEED = 15f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initPos = transform.position;
        //change rotation
        transform.rotation = Quaternion.Euler(0, 0, direction * Mathf.Rad2Deg);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(SPEED * Mathf.Cos(direction), SPEED * Mathf.Sin(direction), 0) * Time.deltaTime;
        //Kill self if:
        //1. Out of bounds
        if (transform.position.x < -10 || transform.position.x > 10 || transform.position.y < -6 || transform.position.y > 6)
        {
            Destroy(gameObject);
            Debug.Log("Lightningbolt out of bounds");
        }
        //2. Collides with enemy
        if (circle.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            Destroy(gameObject);
            //TODO: Damage enemy here
            Debug.Log("Lightningbolt hit enemy");
        }
        //3. Range limit
        if (Vector2.Distance(initPos, transform.position) > RANGE)
        {
            Destroy(gameObject);
            Debug.Log("Fireball reached range limit");
        }
    }
}