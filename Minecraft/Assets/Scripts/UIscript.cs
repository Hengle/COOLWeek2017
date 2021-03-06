﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIscript : MonoBehaviour
{
    public Player Player = null;
    public Gameplay Gameplay = null;

    //variables for timer
    public float timer = 0;
    public int minutes = 0;
    public int seconds = 0;

    //variables for towers (will change)
    public int enemytowers;
    public int playertowers;
    public int neturaltowers;

    //variables for minion count
    public int enemyminions;
    public int playerminions;
    public int totalminions;

    //power variable
    public int power;

    //did you win
    public bool wongame;
    public bool ongame;

    public RectTransform EnergyMeterMax = null;
    public RectTransform EnergyMeterCurrent = null;
    public float EnergyMaxScaleValue = 1.0f;

    public Text TimerText = null;

    public Transform TowerBoxContainer = null;
    public Text EnergyText = null;

    // Use this for initialization
    void Start()
    {
        EnergyMaxScaleValue = EnergyMeterMax.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        //timer variables
        timer += Time.deltaTime;
        seconds += (int)Time.deltaTime;
        minutes = (int)timer / 60;
        seconds = (int)timer % 60;

        //tower variables
        enemytowers = 0;
        playertowers = 0;
        neturaltowers = 0;

        //minion numbers
        enemyminions = 0;
        playerminions = 0;
        totalminions = 0;

        //pwr variable
        power = 0;

        //check # of towers and minions, also win status

        //tower check
        enemytowers = Gameplay.enemytowercount;
        playertowers = Gameplay.playertowercount;
        neturaltowers = Gameplay.neturaltowercount;

        //minion check
        enemyminions = Gameplay.redminion;
        playerminions = Gameplay.blueminion;
        totalminions = Gameplay.totalminion;

        // wincheck
        wongame = Gameplay.win;
        ongame = Gameplay.gameon;


        //check powerlevel
        power = Player.Energy;

        // Update the energy meter UI
        float maxEnergyEver = Player.EnergyMax;
        float curMaxEnergyRatio = Player.EnergyCap / maxEnergyEver;
        float curEnergyRatio = Player.Energy / maxEnergyEver;

        EnergyMeterMax.localScale = new Vector3(EnergyMeterMax.localScale.x, EnergyMaxScaleValue * curMaxEnergyRatio);
        EnergyMeterCurrent.localScale = new Vector3(EnergyMeterCurrent.localScale.x, EnergyMaxScaleValue * curEnergyRatio);

        // Show the amount of energy
        EnergyText.text = Player.Energy.ToString();

        // Update the game timer UI
        if (seconds >= 10 && minutes >= 10)
        {
            TimerText.text = minutes + ":" + seconds;
        }
        if (seconds < 10 && minutes >= 10)
        {
            TimerText.text = minutes + ":0" + seconds;
        }
        if (seconds < 10 && minutes < 10)
        {
            TimerText.text = "0" + minutes + ":0" + seconds;
        }
        if (seconds > 10 && minutes < 10)
        {
            TimerText.text = "0" + minutes + ":" + seconds;
        }

        // Update tower boxes
        List<towerScript> towers = towerScript.AllTowers.ToList();
        towers.Sort((left, right) =>
        {
            if (left.m_teamAllegiance == right.m_teamAllegiance)
            {
                if (left.m_teamAllegiance == Minion.Allegiance.BLUE)
                {
                    // Blue towers are sorted strongest to weakest in the UI
                    if (left.m_towerArmor < right.m_towerArmor)
                        return 1;
                    else if (left.m_towerArmor > right.m_towerArmor)
                        return -1;
                }
                else if (left.m_teamAllegiance == Minion.Allegiance.RED)
                {
                    // Red towers are sorted weakest to strongest in the UI
                    if (left.m_towerArmor < right.m_towerArmor)
                        return -1;
                    else if (left.m_towerArmor > right.m_towerArmor)
                        return 1;
                }

                // All neutral towers or equivalent blue/red towers are equal
                return 0;
            }

            // If the towers aren't the same allegiance, blue towers are always first
            if (left.m_teamAllegiance == Minion.Allegiance.BLUE)
                return -1;
            if (right.m_teamAllegiance == Minion.Allegiance.BLUE)
                return 1;

            // Next, neutral towers are first
            if (left.m_teamAllegiance == Minion.Allegiance.NEUTRAL)
                return -1;
            if (right.m_teamAllegiance == Minion.Allegiance.NEUTRAL)
                return 1;

            // Should never occur
            return 0;
        });

        int towerIndex = 0;
        foreach (towerScript tower in towers)
        {
            Transform towerBoxTransform = TowerBoxContainer.FindChild("TowerBox (" + towerIndex++ + ")");
            if (towerBoxTransform)
            {
                RawImage towerBoxImage = towerBoxTransform.GetComponent<RawImage>();
                if (towerBoxImage)
                {
                    Color towerColor = Color.grey;
                    if (tower.m_teamAllegiance == Minion.Allegiance.BLUE)
                    {
                        towerColor = Color.Lerp(Color.grey, Color.blue, tower.m_towerArmor / 25.0f);
                    }
                    if (tower.m_teamAllegiance == Minion.Allegiance.RED)
                    {
                        towerColor = Color.Lerp(Color.grey, Color.red, tower.m_towerArmor / 25.0f);
                    }
                    towerBoxImage.color = towerColor;
                }
            }
        }
    }

    //GUI here
    private void OnGUI()
    {
        /*
        //onscreen towercount (temporary)
        GUI.Label(new Rect(Screen.width / 2 - 150, 30, 500, 100), "Enemy: " + enemytowers + " Netural: " + neturaltowers + " Yours: " + playertowers);

        //onscreen minioncount
        GUI.Label(new Rect(Screen.width / 2 - 200, 60, 600, 100), "MINION COUNT -- Enemy: " + enemyminions + " Total: " + totalminions + " Yours: " + playerminions);

        //onscreen powerlevel
        GUI.Label(new Rect(Screen.width / 2 - 50, 80, 100, 100), "BLOCK POWER:" + power);
        */

        //win/loss signal
        if (ongame == false)
        {
            Time.timeScale = (float)0.0;

            if (wongame == true)
            {
                //GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height/2, 100, 100), "WINNER");
                UnityEngine.SceneManagement.SceneManager.LoadScene("WinScreen");
            }
            else
            {
                //GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2, 100, 100), "LOSER");
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScreen");
            }
        }
        else
        {
            Time.timeScale = (float)1;
        }
    }
}