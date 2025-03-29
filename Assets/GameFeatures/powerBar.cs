using UnityEngine;
using UnityEngine.UI;

public class powerBar : MonoBehaviour
{
    //FILL IN ALL POWER PREFABS HERE
    public GameObject fireballPrefab, fireWallPrefab, iceSpikePrefab;
    public Image icon1, icon2;
    public Sprite emptyImg, fireImg, iceImg, lightningImg;
    public int potion1 = thePotion.NONE;
    public int potion2 = thePotion.NONE;
    void Start()
    {
        // Initialize the power bar with empty slots
        icon1.sprite = emptyImg;
        icon2.sprite = emptyImg;
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
            Debug.Log("Power bar is full!");
        }
        updatePowerBar();
    }
    public void updatePowerBar(){
        switch (potion1)
        {
            case thePotion.FIRE:
                icon1.sprite = fireImg;
                break;
            case thePotion.ICE:
                icon1.sprite = iceImg;
                break;
            case thePotion.LIGHTNING:
                icon1.sprite = lightningImg;
                break;
            default:
                icon1.sprite = emptyImg;
                break;
        }
        switch (potion2)
        {
            case thePotion.FIRE:
                icon2.sprite = fireImg;
                break;
            case thePotion.ICE:
                icon2.sprite = iceImg;
                break;
            case thePotion.LIGHTNING:
                icon2.sprite = lightningImg;
                break;
            default:
                icon2.sprite = emptyImg;
                break;
        }
    }
    public void usePower(Transform playerPos, Transform cursorPos, Collider2D playerCol){
        //FIRE + NONE = FIREBALL
        Debug.Log("Using Power: " + potion1 + " " + potion2);
        if (potion1 == thePotion.FIRE && potion2 == thePotion.NONE)
        {
            //spawn fireball
            GameObject theFireball = Instantiate(fireballPrefab, playerPos.position, Quaternion.identity);
            theFireball.GetComponent<Fireball>().direction = Mathf.Atan2(cursorPos.position.y - playerPos.position.y, cursorPos.position.x - playerPos.position.x);
            Physics2D.IgnoreCollision(theFireball.GetComponent<CircleCollider2D>(), playerCol);
        }
        //FIRE + FIRE = FIREWALL
        else if (potion1 == thePotion.FIRE && potion2 == thePotion.FIRE)
        {
            //spawn fireWall
            GameObject theFireWall = Instantiate(fireWallPrefab, playerPos.position, Quaternion.identity);
            theFireWall.GetComponent<fireWall>().direction = Mathf.Atan2(cursorPos.position.y - playerPos.position.y, cursorPos.position.x - playerPos.position.x);
            Physics2D.IgnoreCollision(theFireWall.GetComponent<BoxCollider2D>(), playerCol);
            //Move forward by 1 unit
            theFireWall.transform.position += 2* new Vector3(Mathf.Cos(theFireWall.GetComponent<fireWall>().direction), Mathf.Sin(theFireWall.GetComponent<fireWall>().direction), 0);
        }
        //ICE + NONE = ICE SPIKES
        else if (potion1 == thePotion.ICE && potion2 == thePotion.NONE)
        {
            //spawn ice spikes
            GameObject theIceSpikes = Instantiate(fireballPrefab, playerPos.position, Quaternion.identity);
            //set iceSpike targetPos to cursorPos
            theIceSpikes.GetComponent<iceSpike>().targetPos = cursorPos.position;
            Physics2D.IgnoreCollision(theIceSpikes.GetComponent<CircleCollider2D>(), playerCol);
        }
        potion1 = thePotion.NONE;
        potion2 = thePotion.NONE;
        updatePowerBar();
    }
    // Update is called once per frame
    void Update()
    {
        
    }   
}   
