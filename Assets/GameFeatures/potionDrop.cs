using UnityEngine;

public class potionDrop : MonoBehaviour
{
    public thePotion pot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("DropPotion", 2.0f, 4.0f);      // FIXME after potion is collected, change for it to wait 8 secs before creating new potion
    }
    public void DropPotion(){
        pot.changeType(thePotion.FIRE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
