using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused;
    public GameObject pausePanel;
    public GameObject inventoryPanel;
    public GameObject journalPanel;
    public bool usingPausePanel;

    // TODO: Quit To main menu
    //public string mainMenu;
    //public GameSaveManager gameSaveManager;

    //TODO Journal panel

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        inventoryPanel.SetActive(false);
        journalPanel.SetActive(false);
        usingPausePanel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            ChangePause();
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
        }
        else
        {
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
            pausePanel.SetActive(false);
        }
    }

}
