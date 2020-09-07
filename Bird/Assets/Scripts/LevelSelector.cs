using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    // Start is called before the first frame update

    private List<TMP_Dropdown.OptionData> allLevels;
    [SerializeField] int currentMaxLevel;
    TMP_Dropdown dropdown;
    [SerializeField] ScoreLoader scoreLoader;
    [SerializeField] TMP_Text DifficultyTitle;
    [SerializeField] Slider DifficultySlider;
    private const int FREE_PLAY = 3;

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
        allLevels = new List<TMP_Dropdown.OptionData> {
        new TMP_Dropdown.OptionData("Level 1: Learn to fly"),
        new TMP_Dropdown.OptionData("Level 2: Eating seed"),
        new TMP_Dropdown.OptionData("Level 3: Open the door"),
        new TMP_Dropdown.OptionData("Free to explore")};

        dropdown.AddOptions(allLevels.GetRange(0, currentMaxLevel));

        PlayerData.CurrentLevel = currentMaxLevel;
        dropdown.SetValueWithoutNotify(currentMaxLevel);
        ShowHideDifficulty();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetProgress()
    {
        PlayerData.ClearData();
        SceneManager.LoadScene("Menu");

    }

    public void ShowHideDifficulty()
    {
        bool show = dropdown.value == FREE_PLAY;
        DifficultySlider.gameObject.SetActive(show);
        DifficultyTitle.enabled = show;
    }

    public void LoadLevel()
    {
        PlayerData.CurrentLevel = dropdown.value + 1;
        if (dropdown.value == FREE_PLAY)
        {
            PlayerData.Difficulty = DifficultySlider.value;
        }
        SceneManager.LoadScene("Main");
    }
}
