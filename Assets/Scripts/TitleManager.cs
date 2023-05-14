using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void ContinueGame()
    {
        
    }
    public void ConfigGame()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
