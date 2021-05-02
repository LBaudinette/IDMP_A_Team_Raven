using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool isPaused;
    private bool inputPause;
    public GameObject pausePanel;
    public GameObject inventoryPanel;
    public GameObject journalPanel;
    public bool usingPausePanel;
    public string mainMenuString;
    public GameObject resumeButton;

    private PlayerControls playerControls;

    // TODO: Quit To main menu
    //public string mainMenu;
    //public GameSaveManager gameSaveManager;

    //public void OnEnable()
    //{
    //    resumeButton.Select();
    //}

    // Start is called before the first frame update
    void Start()
    {
       // EventSystem.current.SetSelectedGameObject(null);
        //resumeButton.Select();
        isPaused = false;
        inputPause = false;
        pausePanel.SetActive(false);
        inventoryPanel.SetActive(false);
        journalPanel.SetActive(false);
        usingPausePanel = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerControls.Player.Pause.started += _ => inputPause = true;
        if (inputPause)
        {
            ChangePause();
            inputPause = false;
        }
    }

    public void ChangePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            usingPausePanel = true;
            //EventSystem EVRef = EventSystem.current;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(resumeButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            inventoryPanel.SetActive(false);
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void SwitchPanels(GameObject panelToSwitchTo)
    {
        // for first item selected on controller. Not sure where goes.
        //eventSystem.SetSelectedGameObject(firstButtonOfPanel);
        usingPausePanel = !usingPausePanel;
        if (usingPausePanel)
        {
            pausePanel.SetActive(true);
            panelToSwitchTo.SetActive(false);
        }
        else
        {
            panelToSwitchTo.SetActive(true);
            //EventSystem.current.SetSelectedGameObject(firstSelectedButtonOfPanel);

            pausePanel.SetActive(false);
        }
    }

    public void FirstSelectedButtonOfPanel(GameObject firstSelectedButtonOfPanel)
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonOfPanel);
    }

    public void QuitToMain()
    {
        SceneManager.LoadScene(mainMenuString);
        Time.timeScale = 1f;
    }

    public void setControls(PlayerControls controls)
    {
        this.playerControls = controls;
    }
}
