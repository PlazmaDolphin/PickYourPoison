using UnityEngine;

public class powerBar : MonoBehaviour
{
    public int potion1 = thePotion.NONE;
    public int potion2 = thePotion.NONE;
    void Start()
    {

    }
    public void AddPotion(int type)
    {
        if (potion1 == thePotion.NONE)
        {
            potion1 = type;
        }
        else if (potion2 == thePotion.NONE)
        {
            potion2 = type;
        }
        else
        {
            // Both slots are full, do nothing or handle accordingly
            Debug.Log("Power bar is full!");
        }
    }
    public void usePower(Transform playerPos, Transform cursorPos){
        //FIRE + NONE = FIREBALL
        if (potion1 == thePotion.FIRE && potion2 == thePotion.NONE)
        {
            //spawn fireball
            playerPos.position += new Vector3(0, 1f, 0);
            GameObject fireball = Instantiate(playerPos.GetComponent<PlayerMovement>().fireballPrefab, playerPos.position, Quaternion.identity);
            fireball.GetComponent<fireball>().direction = Mathf.Atan2(cursorPos.position.y - playerPos.position.y, cursorPos.position.x - playerPos.position.x);
            Physics2D.IgnoreCollision(fireball.GetComponent<CircleCollider2D>(), playerPos.GetComponent<BoxCollider2D>());
            //move up a little
            playerPos.position -= new Vector3(0, 1f, 0);
            potion1 = thePotion.NONE;
        }
        //FIRE + FIRE = FIREWALL
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
