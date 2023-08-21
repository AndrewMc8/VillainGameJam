using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("UI")]
    [SerializeField] TMP_Text numLeftTxt;
    [SerializeField] GameObject tileScreen;
    [SerializeField] GameObject title;
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject exitBtn;
    [SerializeField] TMP_Text[] evilTxt;
    [SerializeField] float delay = 3;
    [Header("Game")]
    [SerializeField] int enemiesLeft;
    [SerializeField] GameObject normalBackground;
    [SerializeField] GameObject doneBackground;
    [SerializeField] GameObject spawner;

    float timer;

    public void Start()
    {
        Button sBtn = startBtn.GetComponent<Button>();
        Button qBtn = exitBtn.GetComponent<Button>();
        Button titleBtn = tileScreen.GetComponent<Button>();
        sBtn.onClick.AddListener(StartOnClick);
        qBtn.onClick.AddListener(ExitOnClick);

        numLeftTxt.text = enemiesLeft + ": Remain";

        timer = delay;
    }

    public void Update()
    { 
        if(enemiesLeft == 0)
        {
            normalBackground.SetActive(false);
            doneBackground.SetActive(true);
            spawner.SetActive(false);
        }
    }

    public void SetNumLeft()
    {
        enemiesLeft--;
        numLeftTxt.text = enemiesLeft + ": Remain";
    }

    public void SetTileScreen(bool show)
    {
        tileScreen.SetActive(show);
    }

    public void StartOnClick()
    {
        SetTileScreen(false);
    }

    public void ExitOnClick()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
