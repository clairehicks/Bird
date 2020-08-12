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
            default :
                gameObject.AddComponent<Level1_Movement>();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
