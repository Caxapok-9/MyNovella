using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reader : MonoBehaviour
{
    List<Scene> Scenes = new List<Scene>();

    Scene MainScene;

    int indexReplic;

    bool flagWrite = false;

    bool openMenu = false;

    List<Tuple<int, int>> History = new List<Tuple<int, int>>();

    public float speed = 0.05f;

    public TextMeshProUGUI textMesh, nameMesh;

    public GameObject panelChoise2, panelChoise3, panelChoise4;

    public GameObject panelPerson1, panelPerson2, panelPerson3;

    public GameObject panelMenu, panelSetting;

    public GameObject ButtonNext, ButtonBack;

    public Image backGround;

    public AudioSource Music, Sound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Scenes = JsonConvert.DeserializeObject<List<Scene>>(Resources.Load<TextAsset>("MainConfig").text);

        DrawScene();
        StartCoroutine(WriteText());
    }

    // Update is called once per frame
    void Update()
    {
        if (!openMenu)
        {
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.RightArrow))
                NextReplic();

            if (Input.GetKeyUp(KeyCode.LeftArrow))
                BackReplic();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (panelMenu.activeSelf)
            {
                panelMenu.SetActive(false);
                ButtonBack.SetActive(true);
                ButtonNext.SetActive(true);
                openMenu = false;
            }
            else if(panelSetting.activeSelf)
            {
                panelSetting.SetActive(false);
                panelMenu.SetActive(true);
            }
            else
            {
                panelMenu.SetActive(true);
                ButtonBack.SetActive(false);
                ButtonNext.SetActive(false);
                openMenu = true;
                StopWrite();
            }
        }
    }

    public void OpenSetting() // Открытие окна настроек
    {
        panelMenu.SetActive(false);
        panelSetting.SetActive(true);
    }

    public void ClickContinue() // Нажатие на кнопку продолжить
    {
        panelMenu.SetActive(false);
        ButtonBack.SetActive(true);
        ButtonNext.SetActive(true);
        openMenu = false;
    }

    public void ClickExit() // Нажатие на кнопку выход
    {
        PlayerPrefs.Save();
        SaveProgress();
        SceneManager.LoadScene("Start");
    }

    public void SaveProgress()
    {

    }

    public void NextReplic() // Переход к следующей реплике
    {
        if (indexReplic < MainScene.Replics.Count() - 1)
        {
            if (!flagWrite)
            {
                History.Add(Tuple.Create(MainScene.Id, indexReplic));
                indexReplic++;
                DrawScene();

                if (!History.Contains(Tuple.Create(MainScene.Id, indexReplic)))
                    StartCoroutine(WriteText());
                else
                    StopWrite();
            }
            else
            {
                StopWrite();
            }
        }
        else
        {
            if (!flagWrite)
            {
                if (MainScene.Choises.Count() == 0)
                {
                    MainScene = Scenes.Where(p => p.Id == MainScene.Target).First();
                    indexReplic = 0;                    
                    DrawScene();

                    if (!History.Contains(Tuple.Create(MainScene.Id, indexReplic)))
                        StartCoroutine(WriteText());
                    else
                        StopWrite();
                }
                else
                {
                    DrawChoise();
                }
            }
            else
            {
                History.Add(Tuple.Create(MainScene.Id, indexReplic));
                StopWrite();
            }
        }
    }

    public void ClickChoise(UnityEngine.UI.Button button) // Нажатие в окне выбора
    {
        panelChoise2.SetActive(false);
        panelChoise3.SetActive(false);
        panelChoise4.SetActive(false);        

        ButtonNext.SetActive(true);
        ButtonBack.SetActive(true);

        MainScene = Scenes.Where(p => p.Id == MainScene.Choises.Where(p => p.Key == Convert.ToInt32(button.name)).First().TargetID).First();
        indexReplic = 0;
        DrawScene();

        if (!History.Contains(Tuple.Create(MainScene.Id, indexReplic)))
            StartCoroutine(WriteText());
        else
            StopWrite();
    }

    public void BackReplic() // Предыдущая реплика
    {
        if (indexReplic > 0)
        {
            indexReplic--;
            StopWrite();
            DrawScene();
        }
    }

    void DrawScene() // Перерисовка сцены (при переходе к другой сцене, а не реплике !!!)
    {
        nameMesh.text = MainScene.Replics[indexReplic].Name;        

        DrawPerson();

        if (Music.clip != SearchMusic())
        {
            Music.clip = SearchMusic();
            Music.Play();
        }

        Sound.clip = SearchSound();
        Sound.Play();

        if (MainScene.Replics[indexReplic].Color == 1)
            nameMesh.color = Color.red;
        if (MainScene.Replics[indexReplic].Color == 2)
            nameMesh.color = Color.yellow;
        if (MainScene.Replics[indexReplic].Color == 3)
            nameMesh.color = Color.green;

        backGround.sprite = SearchBackground(MainScene.Background);
    }

    AudioClip SearchMusic() // Поиск музыки
    {
        var m = Resources.Load<AudioClip>("Music\\" + MainScene.Music);
        return m;
    }

    AudioClip SearchSound() // Поиск звука
    {
        var m = Resources.Load<AudioClip>("Sound\\" + MainScene.Music);
        return m;
    }

    void DrawPerson() // Прорисковка персонажей на экране их места
    {
        if(MainScene.Replics[indexReplic].Sprite.Count() == 0)
        {
            panelPerson1.SetActive(false);
            panelPerson2.SetActive(false);
            panelPerson3.SetActive(false);
        }
        
        if (MainScene.Replics[indexReplic].Sprite.Count() == 1)
        {
            panelPerson1.SetActive(true);
            panelPerson2.SetActive(false);
            panelPerson3.SetActive(false);

            panelPerson1.GetComponentsInChildren<Image>()[1].sprite = SearchSprite(MainScene.Replics[indexReplic].Sprite[0]);
        }

        if (MainScene.Replics[indexReplic].Sprite.Count() == 2)
        {
            panelPerson1.SetActive(false);
            panelPerson2.SetActive(true);
            panelPerson3.SetActive(false);

            var persons = panelPerson2.GetComponentsInChildren<Image>();
            
            persons[1].sprite = SearchSprite(MainScene.Replics[indexReplic].Sprite[0]);
            persons[2].sprite = SearchSprite(MainScene.Replics[indexReplic].Sprite[1]);
        }

        if (MainScene.Replics[indexReplic].Sprite.Count() == 3)
        {
            panelPerson1.SetActive(false);
            panelPerson2.SetActive(false);
            panelPerson3.SetActive(true);

            var persons = panelPerson3.GetComponentsInChildren<Image>();
            
            persons[1].sprite = SearchSprite(MainScene.Replics[indexReplic].Sprite[0]);
            persons[2].sprite = SearchSprite(MainScene.Replics[indexReplic].Sprite[1]);
            persons[3].sprite = SearchSprite(MainScene.Replics[indexReplic].Sprite[2]);
        }
    }

    UnityEngine.Sprite SearchSprite(Sprite s) // Поиск спрайтов персонажей
    {
        var sprite = Resources.Load<UnityEngine.Sprite>("Images\\Sprites\\" + s.Name + "\\" + s.Number);
        return sprite;
    }

    UnityEngine.Sprite SearchBackground(int number) // Поиск фона
    {
        var sprite = Resources.Load<UnityEngine.Sprite>("Images\\Background\\" + number);
        return sprite;
    }

    void DrawChoise() // Прорисовка окна выбора
    {
        ButtonNext.SetActive(false);

        ButtonBack.SetActive(false);

        if(MainScene.Choises.Count() == 2)
        {
            UnityEngine.UI.Button[] buttons = panelChoise2.GetComponentsInChildren<UnityEngine.UI.Button>();

            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = MainScene.Choises[0].Text;

            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = MainScene.Choises[1].Text;

            panelChoise2.SetActive(true);
        }

        if (MainScene.Choises.Count() == 3)
        {
            UnityEngine.UI.Button[] buttons = panelChoise3.GetComponentsInChildren<UnityEngine.UI.Button>();

            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = MainScene.Choises[0].Text;

            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = MainScene.Choises[1].Text;

            buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = MainScene.Choises[2].Text;

            panelChoise3.SetActive(true);
        }

        if (MainScene.Choises.Count() == 4)
        {
            UnityEngine.UI.Button[] buttons = panelChoise4.GetComponentsInChildren<UnityEngine.UI.Button>();

            buttons[0].GetComponentInChildren<TextMeshProUGUI>().text = MainScene.Choises[0].Text;

            buttons[1].GetComponentInChildren<TextMeshProUGUI>().text = MainScene.Choises[1].Text;

            buttons[2].GetComponentInChildren<TextMeshProUGUI>().text = MainScene.Choises[2].Text;

            buttons[3].GetComponentInChildren<TextMeshProUGUI>().text = MainScene.Choises[3].Text;

            panelChoise4.SetActive(true);
        }
    }

    System.Collections.IEnumerator WriteText() // Вывод текста плавно 
    {
        textMesh.text = "";

        flagWrite = true;

        for(int i = 0; i < MainScene.Replics[indexReplic].Text.Length; i++)
        {
            textMesh.text += MainScene.Replics[indexReplic].Text[i];
            yield return new WaitForSeconds(speed);
        }

        History.Add(Tuple.Create(MainScene.Id, indexReplic));

        flagWrite = false;
    }

    void StopWrite() // В случае повторного нажатия при идущем выводе текста
    {
        StopAllCoroutines();
        textMesh.text = MainScene.Replics[indexReplic].Text;
        flagWrite = false;
    }
}

