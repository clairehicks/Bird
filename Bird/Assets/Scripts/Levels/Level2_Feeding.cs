using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level2_Feeding : Level
{
    public GameObject BedroomDoor;
    private static readonly Vector3 StartPosition = new Vector3(-0.088f, 1.126457f, -9.082f);
    private static readonly Quaternion StartRotation = Quaternion.Euler(new Vector3(0, 80, 0));
    private static readonly Vector3 FoodPosition1 = new Vector3(1.43f, 0.7360903f, -8.9f);
    private static readonly Vector3 FoodPosition2 = new Vector3(4.15f, 0.7360903f, -9.0f);
    private static readonly Quaternion FoodRotation1 = Quaternion.Euler(new Vector3(0, 5, 0));
    private static readonly Quaternion FoodRotation2 = Quaternion.Euler(new Vector3(0, 89, 0));
    
    private int section = 0;
    private TMP_Text info;
    private List<BirdSeedController> seedBoxes = new List<BirdSeedController>();

    // Start is called before the first frame update
    // close bedroom door and put bird on chair
    // add food to both bedside tables
    void Start()
    {
        Animator bedroomDoor = GameObject.Find("BedroomDoor").GetComponent<Animator>();
        bedroomDoor.enabled = true;
        bedroomDoor.SetBool(animBoolName + "1", false);

        Player.transform.SetPositionAndRotation(StartPosition, StartRotation);
        info = GameObject.Find("Info").GetComponent<TMP_Text>();
        StartCoroutine(Intro());

        GameObject foodPrefab = Resources.Load<GameObject>(BirdSeedController.SeedPrefabPath);
        seedBoxes.Add(Instantiate(foodPrefab, FoodPosition1, FoodRotation1).GetComponent<BirdSeedController>());
        seedBoxes.Add(Instantiate(foodPrefab, FoodPosition2, FoodRotation2).GetComponent<BirdSeedController>());
    }

    // Update is called once per frame
    void Update()
    {
        switch (section)
        {
            case 0:
                return;
            case 1:
                GotBox();
                return;
            case 2:
                DroppedBox();
                return;
            case 3:
                Eating();
                return;
            case 4:
                Eaten();
                return;
            case 5:
                Eating2();
                return;
            case 6:
                Eaten2();
                return;
        }
    }

    IEnumerator Intro()
    {
        info.text = LevelStrings.LevelTwo.Welcome;
        yield return new WaitForSeconds(5);
        section = 1;
        info.text = LevelStrings.LevelTwo.GetBox;
    }

    private void GotBox()
    {
        if(GetPlayerStatus()== BeakAndClawStatus.BeakFull && GetTagFromHolding() == BirdSeedController.SeedTag)
        {
            section = 2;
            info.text = LevelStrings.LevelTwo.DropBox;
        }
    }

    private void DroppedBox()
    {
        foreach (var seedScript in seedBoxes)
        {
            if (seedScript.GetStatus() == BirdSeedStatus.Spilled)
            {
                section = 3;
                info.text = LevelStrings.LevelTwo.Eat;
            }
        }
    }

    private void Eating()
    {
        if (GetPlayerStatus() == BeakAndClawStatus.Eating)
        {
            section = 4;
            info.text = LevelStrings.LevelTwo.FoodBar;
            //todo turn on hunger bar
        }
    }

    private void Eaten()
    {
        if (GetPlayerStatus() == BeakAndClawStatus.Empty)
        {
            section = 5;
            info.text = LevelStrings.LevelTwo.FoodBar2;
        }
    }

    private void Eating2()
    {
        if (GetPlayerStatus() == BeakAndClawStatus.Eating)
        {
            section = 6;
            info.text = LevelStrings.LevelTwo.Eat2;
        }
    }


    private void Eaten2()
    {
        if (GetPlayerStatus() == BeakAndClawStatus.Empty)
        {
            section = 7;
            info.text = LevelStrings.LevelTwo.Complete;
            Invoke("Completed", 5);
        }
    }
}
