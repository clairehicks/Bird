using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    // Start is called before the first frame update

    private List<TMP_Dropdown.OptionData> allLevels;
    [SerializeField] int currentMaxLevel;
    TMP_Dropdown dropdown;
    [SerializeField] ScoreLoader scoreLoader;

    void Start()
    {
        //todo set player load/save
        currentMaxLevel = PlayerData.GetCurrentMaxLevel();
        dropdown = gameObject.GetComponent<TMP_Dropdown>();

        SetDropdown();
        scoreLoader.enabled = true;
    }

    private void SetDropdown()
    {
        dropdown.ClearOptions();

        //setup all level names
        allLevels = new List<TMP_Dropdown.OptionData>();
        allLevels.Add(new TMP_Dropdown.OptionData("Level 1: Learn to fly"));
        allLevels.Add(new TMP_Dropdown.OptionData("Level 2: Eating seed"));
        allLevels.Add(new TMP_Dropdown.OptionData("Level 3: Open the door"));
        allLevels.Add(new TMP_Dropdown.OptionData("Free to explore"));

        for (int i = allLevels.Count-1; i >= currentMaxLevel ; i--)
        {
            allLevels.Remove(allLevels[i]);
        }
        dropdown.AddOptions(allLevels);

        PlayerData.CurrentLevel = currentMaxLevel;
        dropdown.SetValueWithoutNotify(currentMaxLevel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetProgress()
    {
        PlayerData.ClearData();
        currentMaxLevel = PlayerData.GetCurrentMaxLevel();
        SetDropdown();
    }

    public void LoadLevel()
    {
        PlayerData.CurrentLevel = dropdown.value + 1;
        SceneManager.LoadScene("Main");
    }
}
