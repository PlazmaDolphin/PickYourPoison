/*using UnityEngine;

public class powerBar : MonoBehaviour
{
    //FILL IN ALL POWER PREFABS HERE
    public GameObject fireballPrefab;
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
<<<<<<< HEAD
    public void usePower(Transform playerPos, Transform cursorPos, Collider2D playerCol){
        //FIRE + NONE = FIREBALL
        if (potion1 == thePotion.FIRE && potion2 == thePotion.NONE)
        {
            //spawn fireball
            GameObject fireball = Instantiate(fireballPrefab, playerPos.position, Quaternion.identity);
            fireball.GetComponent<fireball>().direction = Mathf.Atan2(cursorPos.position.y - playerPos.position.y, cursorPos.position.x - playerPos.position.x);
            Physics2D.IgnoreCollision(fireball.GetComponent<CircleCollider2D>(), playerCol);
            potion1 = thePotion.NONE;
        }
        //FIRE + FIRE = FIREWALL   
=======
    public void usePower(Transform playerPos, Transform cursorPos){
>>>>>>> 7cea9f50cdb951cf32e8a338572a406eceb8d7fd
    }
    // Update is called once per frame
    void Update()
    {
        
    }   
}   */
