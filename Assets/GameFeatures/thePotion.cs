using UnityEngine;

public class thePotion : MonoBehaviour
{
    //Potion Types
    public const int NONE = 0;
    public const int FIRE = 1;
    public const int ICE = 2;
    public const int LIGHTNING = 3;
    public int potionType = FIRE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerHitbox"))
        {
            //Add potion to player inventory
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
            player.AddPotion(potionType);
            //Destroy potion object
            Destroy(gameObject);
        }
    }
}
