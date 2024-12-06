using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingButtonsScript : MonoBehaviour
{
    public void RestartGame()
    {
        SceneDataHolder.Data = "restart";
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneDataHolder.Data = "mainMenu";
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
public static class SceneDataHolder
{
    public static string Data;
}