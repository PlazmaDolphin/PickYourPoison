using UnityEngine;

public class thePotion : MonoBehaviour
{
    //Potion Types
    public const int NONE = 0;
    public const int FIRE = 1;
    public const int ICE = 2;
    public const int LIGHTNING = 3;
    public int potionType = NONE;
    public SpriteRenderer theSprite;
    public Sprite fireImg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        changeType(potionType);
    }
    public void changeType(int type){
        potionType = type;
        switch (potionType)
        {
            case FIRE:
                theSprite.enabled = true;
                theSprite.sprite = fireImg;
                break;
            case NONE:
                //set to None
                theSprite.enabled = false;
                break;
        }
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
            changeType(NONE);
        }
    }
}
