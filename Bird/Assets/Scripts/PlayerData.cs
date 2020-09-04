using System;
using System.Linq;
using UnityEngine;

//Handles Player Info which is stored in PlayerPrefs
public static class PlayerData
{
    public static int CurrentLevel; //this does not need saving
    public const int MostFood = 10;
    private const int MaxLevel = 4;
    private const string CurrentMaxLevel = "CurrentMaxLevel";
    private const string FoodFoundString = "FoodFound";
    private const string FoodFoundEverString = "FoodFoundEver";
    private const string BestTimeString = "BestTime";
    private const string CumalativeTimeString = "CumalativeTime";

    public static void LevelCompleted(int level)
    {
        if (level == PlayerPrefs.GetInt(CurrentMaxLevel) && level < MaxLevel)
        {
            PlayerPrefs.SetInt(CurrentMaxLevel, level + 1);
            PlayerPrefs.Save();
        }
    }

    public static void UpdateStats(bool[] foodFound, float duration)
    {
        if (foodFound.Length != MostFood)
        {
            return; //something unexpected has occurred
        }

        bool[] previousFood = SplitFoodFoundEverArray();

        if (previousFood.Any(b => b == false))
        {
            for (int i = 0; i < MostFood; i++)
            {
                previousFood[i] = foodFound[i] || previousFood[i];
            }
        }

        PlayerPrefs.SetString(FoodFoundEverString, string.Join(",", previousFood));
        PlayerPrefs.SetInt(FoodFoundString, Math.Max(PlayerPrefs.GetInt(FoodFoundString), foodFound.Count(b => b == true)));
        PlayerPrefs.SetFloat(BestTimeString, Math.Max(PlayerPrefs.GetFloat(BestTimeString), duration));
        PlayerPrefs.SetFloat(CumalativeTimeString, PlayerPrefs.GetFloat(CumalativeTimeString) + duration);
        PlayerPrefs.Save();
    }

    public static int GetFoodFoundEver()
    {
        return SplitFoodFoundEverArray().Count(b => b == true);
    }

    public static int GetFoodFound()
    {
        return PlayerPrefs.GetInt(FoodFoundString);
    }
    public static float GetBestTime()
    {
        return PlayerPrefs.GetFloat(BestTimeString);
    }
    public static float GetCumulativeTime()
    {
        return PlayerPrefs.GetFloat(CumalativeTimeString);
    }

    private static bool[] SplitFoodFoundEverArray()
    {
        return PlayerPrefs.GetString(FoodFoundEverString).Split(',').Select(b => Convert.ToBoolean(b)).ToArray();
    }

    public static int GetCurrentMaxLevel()
    {
        if (!PlayerPrefs.HasKey(CurrentMaxLevel))
        {
            ClearData();
        }
        return PlayerPrefs.GetInt(CurrentMaxLevel);

    }

    public static void ClearData()
    {
        PlayerPrefs.SetInt(CurrentMaxLevel, 1);
        PlayerPrefs.SetFloat(BestTimeString, 0);
        PlayerPrefs.SetFloat(CumalativeTimeString, 0);
        PlayerPrefs.SetInt(FoodFoundString, 0);
        PlayerPrefs.SetString(FoodFoundEverString, string.Join(",", new bool[MostFood] { false, false, false, false, false, false, false, false, false, false }));
        PlayerPrefs.Save();
    }
}
