﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    //MINON CONTROL VARIABLES
    public int redminion = 0;
    public int blueminion = 0;
    public int totalminion = 0;
    public int maxred = 10;
    public int maxblue = 7;

    //tower control variables
    public int enemytowercount;
    public int playertowercount;
    public int neturaltowercount;

    public bool win;
    public bool gameon;

    // Use this for initialization
    void Start()
    {
        gameon = true;
    }

    // Update is called once per frame
    void Update()
    {
        //reset the counting variables
        enemytowercount = 0;
        playertowercount = 0;
        neturaltowercount = 0;
        redminion = 0;
        blueminion = 0;
        totalminion = 0;

        //interact with Minions!
        Minion[] minionlist = FindObjectsOfType(typeof(Minion)) as Minion[];
        foreach (Minion minions in minionlist)
        {
            totalminion++;
            if (minions.miniononplayerteam == true)
            {
                blueminion++;
            }
            else
            {
                redminion++;
            }
        }

        totalminion = redminion + blueminion;

        //interact with Towers!
        towerScript[] towerlist = FindObjectsOfType(typeof(towerScript)) as towerScript[];
        foreach (towerScript tower in towerlist)
        {
            //netural tower
            if (tower.m_teamAllegiance == Minion.Allegiance.NEUTRAL)
            {
                neturaltowercount++;
            }

            //player tower - keeps count up and has spawn rules
            if (tower.m_teamAllegiance == Minion.Allegiance.RED)
            {
                playertowercount++;
            }

            //enemy tower code - keeps count up and has spawn rules
            if (tower.m_teamAllegiance == Minion.Allegiance.BLUE)
            {
                enemytowercount++;
            }
        }

        if (enemytowercount == 0 && gameon == true)
        {
            win = true;
            gameon = false;
        }
        
        if (playertowercount == 0 && gameon == true)
        {
            win = false;
            gameon = false;
        }

        //just here for my sanity
    }

    //any code after this is <<outside>> of "Update"

    bool canSpawnMinion (Minion.Allegiance allegiance)
    {
        if (allegiance == Minion.Allegiance.RED)
        {
            if (redminion >= maxred)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        if (allegiance == Minion.Allegiance.BLUE)
        {
            if (blueminion >= maxblue)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        return false;
    }
}
