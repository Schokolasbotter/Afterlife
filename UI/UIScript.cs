using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public Slider energySlider;
    public Text interactText;
    public GameObject player;
    public GameObject endgameUI;
    public Text sensitivityText;
    public MouseLookingScript cameraScript;
    public AudioManagerScript audioManager;
    private PlayerInputActions playerInputActions;
    private bool isPaused = false;
    public GameObject pauseMenu;
    public GameObject miniMap;
    public GameObject energybar;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerScript>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        //Pause game
        if (playerInputActions.Player.pause.triggered)
        {
            if (!isPaused)
            {
                
                pauseGame();
            }
            else
            {
                unPauseGame();
            }
        }

        float energyValue = player.GetComponent<PlayerMovementScript>().energy;
        //Map Value
        float normal = Mathf.InverseLerp(0, 100, energyValue);
        float mappedEnergy = Mathf.Lerp(0, 1, normal);
        energySlider.value = mappedEnergy;

        //Interactable
        if (player.GetComponent<PlayerMovementScript>().nearInteractable && !player.GetComponent<PlayerMovementScript>().isInteracting)
        {
            interactText.gameObject.SetActive(true);
        }
        else
        {
            interactText.gameObject.SetActive(false);
        }

        if (player.GetComponent<PlayerMovementScript>().endgameStarted)
        {
            endgameUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void changeSensibilityText(float sliderValue)
    {
        string inputText = sliderValue.ToString();
        sensitivityText.text = "Mouse Sensitivity : " + inputText;
    }

    private void pauseGame()
    {
        isPaused = true;
        audioManager.Play("WorkbenchSound");
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        miniMap.SetActive(false);
        energybar.SetActive(false);
    }
    private void unPauseGame()
    {
        isPaused = false;
        audioManager.Play("WorkbenchSound");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        miniMap.SetActive(true);
        energybar.SetActive(true);
    }

    public void backToMenu()
    {
        audioManager.Play("MainMenuButton");
        unPauseGame();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void manualReset()
    {
        audioManager.Play("MainMenuButton");
        unPauseGame();
        player.GetComponent<PlayerMovementScript>().manualReset();
    }
}
