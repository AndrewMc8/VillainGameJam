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

    float timer;

    public void Start()
    {
        Button sBtn = startBtn.GetComponent<Button>();
        Button qBtn = exitBtn.GetComponent<Button>();
        Button titleBtn = tileScreen.GetComponent<Button>();
        sBtn.onClick.AddListener(StartOnClick);
        qBtn.onClick.AddListener(ExitOnClick);

        timer = delay;
    }

    public void Update()
    { 
        
    }

    public void SetNumLeft(float numLeft)
    {
        numLeftTxt.text = numLeft + ": Remain";
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
