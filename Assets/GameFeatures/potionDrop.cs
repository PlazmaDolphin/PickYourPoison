using UnityEngine;

public class potionDrop : MonoBehaviour
{
    public thePotion pot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("DropPotion", 2.0f, 8.0f);
    }
    public void DropPotion(){
        pot.changeType(thePotion.FIRE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
