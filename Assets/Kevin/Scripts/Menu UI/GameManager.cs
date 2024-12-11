using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject canvas; // Canvas for the game
    [SerializeField] private GameObject menuCanvas; // Canvas for the main menu / settings menu

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    [SerializeField] private GameObject resumeButton;

    private static bool hasStartedGame = false;
    private static bool gameIsPaused = false;

    private void Start()
    {
        Time.timeScale = 0f;
        ScreenInteractable.instance.Disable();

        string receivedData = SceneDataHolder.Data;
        if (receivedData == "restart")
        {
            StartNewGame();
            SceneDataHolder.Data = "mainMenu";
        }
    }

    private void Update()
    {   // You can only pause the game if you clicked on "Start New Game" button
        if (Input.GetKeyDown(KeyCode.Escape) && hasStartedGame)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void HomeScreen()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Pause() 
    {
        Time.timeScale = 0f;
        gameIsPaused = true;
        menuCanvas.SetActive(true);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        ScreenInteractable.instance.Disable();

        AudioManager.Instance.PauseAmbiance();
        CameraMove.Instance.CanHover = false;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        menuCanvas.SetActive(false);
        ScreenInteractable.instance.TempDisable();

        AudioManager.Instance.ResumeAmbiance();
        CameraMove.Instance.CanHover = true;
    }

    // When pressed it will restart the entire game and close the main menu
    public void StartNewGame()
    {
        Time.timeScale = 1f;
        gameIsPaused = false;
        hasStartedGame = true;
        canvas.SetActive(true);
        menuCanvas.SetActive(false);
        resumeButton.SetActive(true);

        ScreenInteractable.instance.TempDisable();
        // Put code here to reset ghosts, anomalies, score, etc

        AudioManager.Instance.SetAmbiance(true, false);
        CameraMove.Instance.CanHover = true;
    }
}