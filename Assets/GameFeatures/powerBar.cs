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
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
