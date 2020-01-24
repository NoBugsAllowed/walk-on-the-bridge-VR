using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void RunMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void RunVolcano()
    {
        SceneManager.LoadScene("Volcano");
    }
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
