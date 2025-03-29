using UnityEngine;

public class heartScript : MonoBehaviour
{
    public SpriteRenderer theSprite;
    public Sprite hp0, hp1, hp2, hp3;
    public int hp = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateHeartSprite();
    }
    public void updateHeartSprite(){
        switch (hp)
        {
            case 0:
                theSprite.sprite = hp0;
                break;
            case 1:
                theSprite.sprite = hp1;
                break;
            case 2:
                theSprite.sprite = hp2;
                break;
            case 3:
                theSprite.sprite = hp3;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
