using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
    public GameObject panelNewGame;

    public UnityEngine.UI.Button buttonContinue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.GetInt("NewGame") == 1)
            buttonContinue.enabled = true;
        else
            buttonContinue.enabled = false;        
    }

    // Update is called once per frame
    void Update()
    {
        
            
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void ClickContinue()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ClickNewGame()
    {
        if(PlayerPrefs.GetInt("NewGame") == 1)
        {
            panelNewGame.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt("NewGame", 1);
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void ClickYes()
    {
        PlayerPrefs.SetInt("NewGame", 1);
        PlayerPrefs.DeleteKey("Scene");
        PlayerPrefs.DeleteKey("Replic");
        SceneManager.LoadScene("SampleScene");
    }

    public void ClickNo()
    {
        panelNewGame.SetActive(false);
    }
}
