using UnityEngine;
using UnityEngine.Video;

public class introVid : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public GameObject videoScreen; // Reference to the GameObject that displays the video
    public GameObject theUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.isPlaying)
        {
            videoScreen.SetActive(true); // Show the video screen
            theUI.SetActive(false); // Hide the UI
        }
        else
        {
            videoScreen.SetActive(false); // Hide the video screen
            theUI.SetActive(true); // Show the UI
        }   
    }
}
