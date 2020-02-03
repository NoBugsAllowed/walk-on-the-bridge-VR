using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private int menuNum;
    private Button btnStart;
    private Button btnExit;
    private Button btnRunMain;
    private Button btnRunVolcano;
    private Button btnBack;

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

    void Start()
    {
        switch(menuNum)
        {
            case 1:
                btnStart = GameObject.Find("BtnStart").GetComponent<Button>();
                btnExit = GameObject.Find("BtnExit").GetComponent<Button>();
                break;
            case 2:
                btnRunMain = GameObject.Find("BtnRunMain").GetComponent<Button>();
                btnRunVolcano = GameObject.Find("BtnRunVolcano").GetComponent<Button>();
                btnBack = GameObject.Find("BtnBack").GetComponent<Button>();
                break;
        }
    }

    void Update()
    {
        // Using menu with keyboard
        if (menuNum == 1) // Main screen
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                btnStart.onClick.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                btnExit.onClick.Invoke();
            }
        }
        if (menuNum == 2) // Second screen with scenes
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                btnRunMain.onClick.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                btnRunVolcano.onClick.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                btnBack.onClick.Invoke();
            }
        }
    }
}
