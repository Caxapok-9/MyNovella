using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu : MonoBehaviour
{
    public GameObject panelSave, panelSetting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(panelSetting.activeSelf)
                panelSetting.SetActive(false);

            if(panelSave.activeSelf)
                panelSave.SetActive(false);
        }
    }

    public void ClickExit()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void ClickGame()
    {
        panelSave.SetActive(true);
    }

    public void ClickSetting()
    {
        panelSetting.SetActive(true);
    }
}
