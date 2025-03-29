using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
    
    public AudioSource src;//the source audio that take the audio from any clip being played to play it
    public AudioClip drinksfx, atksfx, majsfx, dmgsfx, goblinsfx, goblinMadsfx; //audio clip names go here

    public void Drinking() //drinking any potion sound effect
    {
        src.clip = drinksfx;
        src.Play();
    }

    public void regattack() //regular attack sound effect
    {
        src.clip = atksfx;
        src.Play();
    }

     public void majattack() //magic attack sound effect
    {
        src.clip = majsfx;
        src.Play();
    }

    //all sound bites under this comment are done by members of the team and are original

    public void damage() //sound effect for when the mc takes damage - done
    {
        src.clip = dmgsfx; 
        src.Play();
    }

    public void goblin() //sound effect for the goblin taking damage - done
    {
        src.clip = goblinsfx;
        src.Play();
    }

    public void goblinmad()
    {
        src.clip = goblinMadsfx;
    }
/*

    public void bartender() //voice line for bartender 
    {
        src.clip = bartsfx; //pick your poison
        src.Play();
    }

    public void mc() //
    {
        src.clip = mcsfx;
        src.Play();
    }

    public void mc2()
    {
        src.clip = mcsfx2;
        src.Play();
    }*/
    //mwj
    //start and update not needed
}
