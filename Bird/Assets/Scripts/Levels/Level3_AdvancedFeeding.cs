﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level3_AdvancedFeeding : Level
{
    public GameObject BedroomDoor;
    private static readonly Vector3 StartPosition = new Vector3(2.0f, 1.484119f, -3.6f);
    private static readonly Quaternion StartRotation = Quaternion.Euler(new Vector3(0, -90, 0));
    private static readonly Vector3 FoodPosition1 = new Vector3(0.0f, -0.04582069f, 0.0f);
    private static readonly Vector3 FoodPosition2 = new Vector3(-0.4f, 0.1356329f, 0.0f);
    private static readonly Quaternion FoodRotation = Quaternion.Euler(new Vector3(90, 0, 0));

    private int section = 0;
    private List<BirdSeedController> seedBoxes = new List<BirdSeedController>();
    private CageDoorController cageDoor;
    private Animator drawerAnimator;

    // Start is called before the first frame update
    // close bedroom door and put bird on chair
    // add food to two drawers.
    void Start()
    {
        CloseDoor("BedroomDoor");

        Player.transform.SetPositionAndRotation(StartPosition, StartRotation);
        StartCoroutine(Intro());

        seedBoxes.Add(CreateBox(FoodPosition1, FoodRotation, "bed_draw01", 1));
        seedBoxes.Add(CreateBox(FoodPosition2, FoodRotation, "bed_draw02", 2));
        cageDoor = GameObject.Find("tuere").GetComponent<CageDoorController>();
        drawerAnimator = GameObject.Find("PFB_ChestOfDraws").GetComponent<Animator>();
        hungerBar.drainHealth = false;
        hungerBar.SetHealth(50.0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (section)
        {
            case 0:
                return;
            case 1:
                OpenedCage();
                return;
            case 2:
                Escape();
                return;
            case 3:
                OpenDrawer();
                return;
            case 4:
                Eating();
                return;
            case 5:
                Eaten();
                return;
            case 6:
                Eating2();
                return;
            case 7:
                Eaten2();
                return;
        }
    }

    IEnumerator Intro()
    {
        info.text = LevelStrings.LevelThree.Welcome;
        yield return new WaitForSeconds(5);
        section = 1;
        info.text = LevelStrings.LevelThree.OpenCage;
    }

    private void OpenedCage()
    {
        if (cageDoor.GetState() == CageDoorController.State.Open)
        {
            section = 2;
            info.text = LevelStrings.LevelThree.LeaveCage;
        }
    }

    private void Escape()
    {
        if (Player.transform.position.x > 1.3)
        {
            section = 3;
            info.text = LevelStrings.LevelThree.OpenDrawer;
        }
    }
    private void OpenDrawer()
    {
        if (drawerAnimator.GetBool(PlayerController.animBoolName + 1))
        {
            section = 4;
            info.text = LevelStrings.LevelThree.Eat;
        }
    }

    private void Eating()
    {
        if (GetPlayerStatus() == BeakAndClawStatus.Eating)
        {
            section = 5;
            hungerBar.drainHealth = true;
            //do not change the message this time
        }
    }

    private void Eaten()
    {
        if (GetPlayerStatus() == BeakAndClawStatus.Empty)
        {
            section = 6;
            info.text = LevelStrings.LevelThree.Again;
        }
    }

    private void Eating2()
    {
        if (GetPlayerStatus() == BeakAndClawStatus.Eating)
        {
            section = 7;
            //do not change the message this time
        }
    }

    private void Eaten2()
    {
        if (GetPlayerStatus() == BeakAndClawStatus.Empty)
        {
            section = 8;
            info.text = LevelStrings.LevelThree.Complete;
            Invoke("Completed", 5);
        }
    }
}
