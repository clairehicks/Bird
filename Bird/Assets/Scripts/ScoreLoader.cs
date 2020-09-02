using System;
using TMPro;
using UnityEngine;

public class ScoreLoader : MonoBehaviour
{
    [SerializeField] TMP_Text LongestRunTime;
    [SerializeField] TMP_Text CumulativeRunTime;
    [SerializeField] TMP_Text FoodFound;
    [SerializeField] TMP_Text FoodFoundEver;

    // Start is called before the first frame update
    void Start()
    {
        LongestRunTime.text = TimeSpan.FromSeconds(PlayerData.GetBestTime()).ToString(@"mm\:ss");
        CumulativeRunTime.text = TimeSpan.FromSeconds(PlayerData.GetCumulativeTime()).ToString(@"mm\:ss");
        FoodFound.text = string.Format("{0} of {1}", PlayerData.GetFoodFound(), PlayerData.MostFood);
        FoodFoundEver.text = string.Format("{0} of {1}", PlayerData.GetFoodFoundEver(), PlayerData.MostFood);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
