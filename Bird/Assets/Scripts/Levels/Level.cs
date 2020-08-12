
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public GameObject Player;
    public const string animBoolName = "isOpen_Obj_";

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }

    public void ReturnToMenu(bool complete)
    {
        if (complete)
        {
            PlayerData.LevelCompleted(PlayerData.CurrentLevel);
        }
        SceneManager.LoadScene("Menu");
    }
}
