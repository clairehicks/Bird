using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("LevelLoader" + PlayerData.CurrentLevel + " of " + PlayerData.GetCurrentMaxLevel());
        switch (PlayerData.CurrentLevel)
        {
            case 1:
                gameObject.AddComponent<Level1_Movement>();
                break;
            case 2:
                gameObject.AddComponent<Level2_Feeding>();
                break;
            default:
            case 3:
                gameObject.AddComponent<Level3_AdvancedFeeding>();
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
