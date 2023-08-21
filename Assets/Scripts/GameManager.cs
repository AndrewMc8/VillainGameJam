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
    [SerializeField] GameObject winTxt;
    [SerializeField] float delay = 3;
    [Header("Game")]
    [SerializeField] int enemiesLeft;
    [SerializeField] GameObject normalBackground;
    [SerializeField] GameObject doneBackground;
    [SerializeField] GameObject spawner;
    [SerializeField] GameObject player;

    float timer;
    Animator playerAnimator;

    public void Start()
    {
        Button sBtn = startBtn.GetComponent<Button>();
        Button qBtn = exitBtn.GetComponent<Button>();
        sBtn.onClick.AddListener(StartOnClick);
        qBtn.onClick.AddListener(ExitOnClick);

        numLeftTxt.text = enemiesLeft + ": Remain";
        playerAnimator = player.GetComponent<Animator>();

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
                    foreach (var txt in evilTxt)
                    {
                        txt.SetActive(false);
                        Destroy(txt);
                    }
                    startBtn.SetActive(true);
                    exitBtn.SetActive(true);
                    title.SetActive(true);
                }

                timer = delay;
            }
        }

        if (enemiesLeft == 0)
        {
            normalBackground.SetActive(false);
            doneBackground.SetActive(true);
            spawner.SetActive(false);

            timer -= Time.deltaTime;

            if (timer <= 0 && player.GetComponent<SpriteRenderer>())
            {
                playerAnimator.SetTrigger("Teleport");
                Destroy(player.GetComponent<SpriteRenderer>(), 2.0f);
                timer = delay;
            }
            else if (timer <= 0 && !player.GetComponent<SpriteRenderer>())
            {
                timer = delay;
                titleScreen.SetActive(true);
                winTxt.SetActive(true);
            }
            else if (timer <= 0 && winTxt.activeInHierarchy) Application.Quit();
        }
    }

    public void SetNumLeft()
    {
        enemiesLeft--;
        numLeftTxt.text = enemiesLeft + ": Remain";
        timer = delay;
    }

    public void SetTitleScreen(bool show)
    {
        titleScreen.SetActive(show);
    }

    public void StartOnClick()
    {
        SetTitleScreen(false);
        title.SetActive(false);
        exitBtn.SetActive(false);
        startBtn.SetActive(false);
        Destroy(title);
        Destroy(startBtn);
        Destroy(exitBtn);
    }

    public void ExitOnClick()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
