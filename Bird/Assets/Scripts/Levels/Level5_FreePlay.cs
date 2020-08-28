using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level5_FreePlay : Level
{
    public GameObject BedroomDoor;
    private static readonly Vector3 StartPosition = new Vector3(2.0f, 1.484119f, -3.6f);
    private static readonly Quaternion StartRotation = Quaternion.Euler(new Vector3(0, -90, 0));

    private static readonly Vector3 BedroomFoodPosition1 = new Vector3(0.1f, 04582069f, 0.0f);
    private static readonly Vector3 BedroomFoodPosition2 = new Vector3(-0.9f, 0.3181914f, 0.0f);
    private static readonly Vector3 BedroomFoodPosition3 = new Vector3(0.2f, -0.04777834f, 0.01f);
    private static readonly Vector3 BedroomFoodPosition4 = new Vector3(0.0f, -0.02233437f, 0.07f);
    private static readonly Vector3 BedroomFoodPosition5 = new Vector3(2.0f, 0.1551253f, -3.8f);
    private static readonly Vector3 KitchenFoodPosition1 = new Vector3(3.12f, 0.3289094f, 1.75f);
    private static readonly Vector3 KitchenFoodPosition2 = new Vector3(-4.14f, 0.3956565f, -1.20f);
    private static readonly Vector3 KitchenFoodPosition3 = new Vector3(-4.1f, 0.3956565f, 1.11f);
    private static readonly Vector3 KitchenFoodPosition4 = new Vector3(0.0f, -0.04510951f, 0.1f);
    private static readonly Vector3 BathroomFoodPosition = new Vector3(-4.6f, 0.1823957f, -6.17f);

    private static readonly Quaternion FoodRotationFlat1 = Quaternion.Euler(new Vector3(90, 0, 0));
    private static readonly Quaternion FoodRotationFlat2 = Quaternion.Euler(new Vector3(90, 0, 90));
    private static readonly Quaternion FoodRotationUpright1 = Quaternion.Euler(new Vector3(0, 5, 0));
    private static readonly Quaternion FoodRotationUpright2 = Quaternion.Euler(new Vector3(0, 89, 0));


    private List<BirdSeedController> seedBoxes = new List<BirdSeedController>();

    // Start is called before the first frame update
    // add food to ten places
    void Start()
    {
        FailOnStarving = false;
        Player.transform.SetPositionAndRotation(StartPosition, StartRotation);
        StartCoroutine(Intro());

        seedBoxes.Add(CreateBox(BedroomFoodPosition1, FoodRotationFlat2, "bed_draw01", 1));
        seedBoxes.Add(CreateBox(BedroomFoodPosition2, FoodRotationFlat1, "bed_draw03", 2));
        seedBoxes.Add(CreateBox(BedroomFoodPosition3, FoodRotationFlat1, "bed_draw06", 3));
        seedBoxes.Add(CreateBox(BedroomFoodPosition4, FoodRotationFlat2, "side1_draw", 4));
        seedBoxes.Add(CreateBox(BedroomFoodPosition5, FoodRotationFlat2, null, 5)); //under drawers
        seedBoxes.Add(CreateBox(KitchenFoodPosition1, FoodRotationUpright2, null, 6)); //tvStandDoor_R
        seedBoxes.Add(CreateBox(KitchenFoodPosition2, FoodRotationUpright1, null, 7)); //k door01
        seedBoxes.Add(CreateBox(KitchenFoodPosition3, FoodRotationUpright2, null, 8)); //k door04
        seedBoxes.Add(CreateBox(KitchenFoodPosition4, FoodRotationFlat2, "k_draw03", 9));
        seedBoxes.Add(CreateBox(BathroomFoodPosition, FoodRotationFlat1, null, 10)); //basket
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override string CalculateStats()
    {
        return string.Format(LevelStrings.LevelFive.CompleteStats, TimeSpan.FromSeconds(Time.timeSinceLevelLoad).ToString(@"mm\:ss"), seedBoxes.Count(box => box.GetStatus() == BirdSeedStatus.Empty));
    }

    IEnumerator Intro()
    {
        info.text = LevelStrings.LevelFive.Welcome;
        yield return new WaitForSeconds(5);
        StartCoroutine(Intro2());
    }
    IEnumerator Intro2()
    {
        info.text = LevelStrings.LevelFive.Food;
        yield return new WaitForSeconds(5);
        info.text = null;
    }

    //todo complete
}
