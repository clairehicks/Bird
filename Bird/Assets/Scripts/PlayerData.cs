
using UnityEngine;

//Handles Player Info which is stored in PlayerPrefs
public static class PlayerData
{
    public static int CurrentLevel; //this does not need saving
    private const int MaxLevel = 2; //todo put this back to 5 for continued play;
    private const string CurrentMaxLevel = "CurrentMaxLevel";
    //todo add high score and achievements

    public static void LevelCompleted(int level)
    {
        if (level == GetCurrentMaxLevel() && level < MaxLevel)
        {
            PlayerPrefs.SetInt(CurrentMaxLevel, GetCurrentMaxLevel() + 1);
        }
    }


    public static int GetCurrentMaxLevel()
    {
        if (!PlayerPrefs.HasKey(CurrentMaxLevel))
        {
            ClearData();
        }
        return PlayerPrefs.GetInt(CurrentMaxLevel);

    }

    public static void ClearData(){
        PlayerPrefs.SetInt(CurrentMaxLevel, 1);
    }
}
