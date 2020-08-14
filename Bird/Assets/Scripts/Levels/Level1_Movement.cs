using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level1_Movement : Level
{
    public GameObject BedroomDoor;
    private static readonly Vector3 StartPosition = new Vector3(-0.088f, 1.126457f, -9.082f);
    private static readonly Quaternion StartRotation = Quaternion.Euler(new Vector3(0, 32, 0));

    private int section = 0;
    private TMP_Text info;

    // Start is called before the first frame update
    //close bedroom door and put bird on chair
    void Start()
    {
        Animator bedroomDoor = GameObject.Find("BedroomDoor").GetComponent<Animator>();
        bedroomDoor.enabled = true;
        bedroomDoor.SetBool(animBoolName + "1", false);

        Player.transform.SetPositionAndRotation(StartPosition, StartRotation);
        info = GameObject.Find("Info").GetComponent<TMP_Text>();
        StartCoroutine(Intro());
    }

    // Update is called once per frame
    void Update()
    {
        switch (section)
        {
            case 0:
                return;
            case 1:
                Flap1();
                return;
            case 2:
                FlapLess2();
                return;
            case 3:
                Forwards3();
                return;
            case 4:
                Turn4();
                return;
            case 5:
                return;
        }
    }

    IEnumerator Intro()
    {
        info.text = LevelStrings.LevelOne.Welcome;
        yield return new WaitForSeconds(5);
        section = 1;
        info.text = LevelStrings.LevelOne.Flap;
    }

    private void Flap1()
    { //this section is complete if the player gets above 2m
        if (Player.transform.position.y > 2.0f)
        {
            section = 2;
            info.text = LevelStrings.LevelOne.FlapLess;
        }
    }

    private void FlapLess2()
    {
        if (Player.transform.position.y < 1.2)
        {
            section = 3;
            info.text = LevelStrings.LevelOne.Forward;
        }
    }

    private void Forwards3()
    {
        if (Player.transform.position.x > 1)
        {
            section = 4;
            info.text = LevelStrings.LevelOne.Turn;
        }
    }

    private void Turn4()
    {
        if (Player.transform.rotation.eulerAngles.y > 50 || Player.transform.rotation.eulerAngles.y < 10)
        {
            section = 5;
            info.text = LevelStrings.LevelOne.Complete;
            Invoke("Completed", 30);
        }
    }
}
