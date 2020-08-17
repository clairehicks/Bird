
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public GameObject Player;
    private PlayerController PlayerController;
    public const string animBoolName = "isOpen_Obj_";

    private void Awake()
    {
        Player = GameObject.Find("Player");
        PlayerController = Player.GetComponent<PlayerController>();
    }

    public void CloseDoor(string doorName)
    {
        Animator door = GameObject.Find(doorName).GetComponent<Animator>();
        door.speed = float.MaxValue;
        door.enabled = true;
        door.SetBool(animBoolName + "1", false);

        StartCoroutine("Disable",door);
    }

    private IEnumerator Disable(Animator door)
    {
        yield return new WaitForEndOfFrame();
        door.enabled = false;
    }

    private void Update()
    {
        //todo hunger check
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
}
