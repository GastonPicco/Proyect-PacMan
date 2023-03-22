using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class canvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PanelJugar,PanelNiveles,PanelPausa,ButtonVolver;
    public bool needScore;
    public bool lifes;
    public bool InGame;
    public GameObject life1Image, life2Image, life3Image;
    public TextMeshProUGUI ScoreText;
    public GameObject Score2;
    public TextMeshProUGUI ScoreText2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!InGame)
        {
            if (PanelJugar.activeInHierarchy)
            {
                PanelNiveles.SetActive(false);
            }
            else if (!PanelJugar.activeInHierarchy)
            {
                PanelNiveles.SetActive(true);
            }
        }
        
        if (InGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !PanelPausa.activeInHierarchy)
            {
                PanelPausa.SetActive(true);
                Time.timeScale = 0;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && PanelPausa.activeInHierarchy && GameManager.data.lifes != 0|| Input.GetKeyDown(KeyCode.Escape) && PanelPausa.activeInHierarchy && !GameManager.data.win)
            {
                PanelPausa.SetActive(false);
            }
            if (PanelPausa.activeInHierarchy)
            {
                GameManager.data.pause = true;
                Time.timeScale = 0;
            }
            else
            {
                GameManager.data.pause = false;
                Time.timeScale = 1;
            }
        }
        if (needScore)
        {
            ScoreText.text = "Score: "+ GameManager.data.Score;
            if (GameManager.data.win)
            {
                PanelPausa.SetActive(true);
                ButtonVolver.SetActive(false);
                Score2.SetActive(true);
                ScoreText2.text = "Score: " + GameManager.data.Score;
            }
        }
        if (lifes)
        {
            if(GameManager.data.lifes == 3)
            {
                life1Image.SetActive(true);
                life2Image.SetActive(true);
                life3Image.SetActive(true);
            }
            else if(GameManager.data.lifes == 2)
            {
                life1Image.SetActive(true);
                life2Image.SetActive(true);
                life3Image.SetActive(false);
            }
            else if (GameManager.data.lifes == 1)
            {
                life1Image.SetActive(true);
                life2Image.SetActive(false);
                life3Image.SetActive(false);
            }
            else if (GameManager.data.lifes == 0)
            {
                life1Image.SetActive(false);
                life2Image.SetActive(false);
                life3Image.SetActive(false);
                PanelPausa.SetActive(true);
                ButtonVolver.SetActive(false);
            }
        }
    }
    public void Jugar()
    {
        PanelNiveles.SetActive(true);
        PanelJugar.SetActive(false);
    }
    public void Volever()
    {
        PanelJugar.SetActive(true);
    }
    public void Salir()
    {
        Application.Quit();
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void UnPause()
    {
        PanelPausa.SetActive(false);
    }
}
