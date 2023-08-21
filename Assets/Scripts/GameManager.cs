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
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject title;
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject exitBtn;
    [SerializeField] GameObject[] evilTxt;
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
        sBtn.onClick.AddListener(StartOnClick);
        qBtn.onClick.AddListener(ExitOnClick);

        numLeftTxt.text = enemiesLeft + ": Remain";

        timer = delay;
    }

    public void Update()
    {
        if(titleScreen.activeInHierarchy)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && !title.activeInHierarchy)
            {
                if (!evilTxt[0].activeInHierarchy) evilTxt[0].SetActive(true);
                else if(!evilTxt[1].activeInHierarchy) evilTxt[1].SetActive(true);
                else if (evilTxt[1].activeInHierarchy)
                {
                    foreach (var txt in evilTxt) txt.SetActive(false);
                    startBtn.SetActive(true);
                    exitBtn.SetActive(true);
                    title.SetActive(true);
                }

                timer = delay;
            }
        }

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

    public void SetTitleScreen(bool show)
    {
        titleScreen.SetActive(show);
    }

    public void StartOnClick()
    {
        SetTitleScreen(false);
    }

    public void ExitOnClick()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
