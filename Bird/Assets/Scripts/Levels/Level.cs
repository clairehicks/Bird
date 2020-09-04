using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public GameObject Player;
    private PlayerController PlayerController;
    public const string animBoolName = "isOpen_Obj_";
    private GameObject foodPrefab;
    public bool FailOnStarving = true;
    public Healthbar hungerBar;
    public TMP_Text info;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        hungerBar = GameObject.Find("HungerBar").GetComponent<Healthbar>();
        PlayerController = Player.GetComponent<PlayerController>();
        foodPrefab = Resources.Load<GameObject>(BirdSeedController.SeedPrefabPath);
        info = GameObject.Find("Info").GetComponent<TMP_Text>();
    }

    public void CloseDoor(string doorName)
    {
        Animator door = GameObject.Find(doorName).GetComponent<Animator>();
        door.speed = float.MaxValue;
        door.enabled = true;
        door.SetBool(animBoolName + "1", false);
    }

    private void LateUpdate()
    {
        if (hungerBar.health > 0) { return; }
        //bird is hungry so end level
        StartCoroutine(EndLevel());
    }

    IEnumerator EndLevel()
    {
        Time.timeScale = 0;
        info.text = FailOnStarving ? LevelStrings.Failure : CalculateStats();
        yield return new WaitForSecondsRealtime(10.0f);

        if (FailOnStarving)
        {
            Failure();
        }
        Completed();
    }


    public virtual string CalculateStats()
    {
        throw new NotImplementedException();
    }

    public BirdSeedController CreateBox(Vector3 position, Quaternion rotation, string doorOrDrawer, int suffix)
    {
        GameObject parent = doorOrDrawer != null ? GameObject.Find(doorOrDrawer) : null;
        var box = Instantiate(foodPrefab, position, rotation);
        box.name = "seedbox" + suffix;
        if (parent == null)
        {
            box.GetComponent<Rigidbody>().isKinematic = false;
        }
        else
        {
            box.transform.SetParent(parent.transform, false);
        }
        return box.GetComponent<BirdSeedController>();
    }

    public void Completed()
    {
        ReturnToMenu(true);
    }

    public void Failure()
    {
        ReturnToMenu(false);
    }

    private void ReturnToMenu(bool complete)
    {
        if (complete)
        {
            PlayerData.LevelCompleted(PlayerData.CurrentLevel);
        }
        SceneManager.LoadScene("Menu");
    }

    public BeakAndClawStatus GetPlayerStatus()
    {
        return PlayerController.currentAction;
    }

    public string GetTagFromHolding()
    {
        return PlayerController.holding.tag;
    }

    public void SetEnabledKeys(bool motion, bool beak, bool claw)
    {
        SetEnabledKeys(motion, motion, motion, beak, claw);
    }

    public void SetEnabledKeys(bool upDown, bool leftRight, bool forwards, bool beak, bool claw)
    {
        PlayerController.UpDownEnabled = upDown;
        PlayerController.TurnEnabled = leftRight;
        PlayerController.ForwardEnabled = forwards;
        PlayerController.BeakEnabled = beak;
        PlayerController.ClawEnabled = claw;
    }
}
