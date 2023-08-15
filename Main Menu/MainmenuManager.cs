using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;



public class MainmenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject CreditsMenu;
    public GameObject ControlsMenu;
    public GameObject MainCamera;
    public VideoClip video;
    private VideoPlayer videoPlayer;
    private AudioManagerScript audioManager;

    public void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerScript>();
    }
    public void openCreditsMenu()
    {
        audioManager.Play("MainMenuButton");
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
        ControlsMenu.SetActive(false);
    }
    
    public void openControlsMenu()
    {
        audioManager.Play("MainMenuButton");
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        ControlsMenu.SetActive(true);
    }
    
    public void openMainMenu()
    {
        audioManager.Play("MainMenuButton");
        MainMenu.SetActive(true);
        CreditsMenu.SetActive(false);
        ControlsMenu.SetActive(false);
    }

    public void playGame()
    {
        audioManager.Play("MainMenuButton");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void QuitGame()
    {
        audioManager.Play("MainMenuButton");
        Application.Quit();
    }

    public void playVideo()
    {
        audioManager.Play("MainMenuButton");
        MainMenu.SetActive(false);
        videoPlayer = MainCamera.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Afterlife Cinematic.mp4");
        videoPlayer.loopPointReached += EndReached;
        videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        MainMenu.SetActive(true);
        vp.Stop();
    }
}
