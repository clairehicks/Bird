using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public GameObject Player;
    private PlayerController PlayerController;
    public const string animBoolName = "isOpen_Obj_";
    private GameObject foodPrefab;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        PlayerController = Player.GetComponent<PlayerController>();
        foodPrefab = Resources.Load<GameObject>(BirdSeedController.SeedPrefabPath);
    }

    public void CloseDoor(string doorName)
    {
        Animator door = GameObject.Find(doorName).GetComponent<Animator>();
        door.speed = float.MaxValue;
        door.enabled = true;
        door.SetBool(animBoolName + "1", false);
    }

    private void Update()
    {
        //todo hunger check
    }

    public BirdSeedController CreateBox(Vector3 position, Quaternion rotation, string doorOrDrawer)
    {
        GameObject parent = GameObject.Find(doorOrDrawer);
        var box = parent!=null? Instantiate(foodPrefab, position, rotation, parent.transform): Instantiate(foodPrefab, position, rotation);
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
}
