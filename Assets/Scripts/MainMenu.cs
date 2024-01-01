using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject minPlayersDecrementButton;
    [SerializeField] private GameObject minPlayersIncrementButton;
    [SerializeField] private GameObject maxPlayersDecrementButton;
    [SerializeField] private GameObject maxPlayersIncrementButton;

    [SerializeField] private TextMeshProUGUI minPlayersText;
    [SerializeField] private TextMeshProUGUI maxPlayersText;
    [SerializeField] private TextMeshProUGUI enableAudienceText;

    [SerializeField] private int fixedMinPlayers;
    [SerializeField] private int fixedMaxPlayers;
    
    private int minPlayers;
    private int maxPlayers;
    private bool enableAudience;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MinPlayers"))
        {
            int mp = PlayerPrefs.GetInt("MinPlayers");
            minPlayers = mp < fixedMinPlayers ? fixedMinPlayers : mp > fixedMaxPlayers ? fixedMaxPlayers : mp;
        }
        else minPlayers = fixedMinPlayers;
        if (PlayerPrefs.HasKey("MaxPlayers"))
        {
            int mp = PlayerPrefs.GetInt("MaxPlayers");
            maxPlayers = mp < fixedMinPlayers ? fixedMinPlayers : mp > fixedMaxPlayers ? fixedMaxPlayers : mp;
        }
        else maxPlayers = fixedMaxPlayers;
        if (PlayerPrefs.HasKey("Audience")) enableAudience = PlayerPrefs.GetInt("Audience") > 0;
        else enableAudience = true;
        UpdateSettingsUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateSettingsUI()
    {
        minPlayersDecrementButton.SetActive(minPlayers > fixedMinPlayers);
        minPlayersIncrementButton.SetActive(minPlayers < maxPlayers);
        maxPlayersDecrementButton.SetActive(maxPlayers > minPlayers);
        maxPlayersIncrementButton.SetActive(maxPlayers < fixedMaxPlayers);
        minPlayersText.text = minPlayers + "";
        maxPlayersText.text = maxPlayers + "";
        enableAudienceText.text = enableAudience ? "Enabled" : "Disabled";
    }

    public void SetMinPlayers(bool increment)
    {
        if (increment && minPlayers < maxPlayers) minPlayers++;
        else if (!increment && minPlayers > fixedMinPlayers) minPlayers--;
        UpdateSettingsUI();
    }

    public void SetMaxPlayers(bool increment)
    {
        if (increment && maxPlayers < fixedMaxPlayers) maxPlayers++;
        else if (!increment && maxPlayers > minPlayers) maxPlayers--;
        UpdateSettingsUI();
    }

    public void SetEnableAudience()
    {
        enableAudience = !enableAudience;
        UpdateSettingsUI();
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("MinPlayers", minPlayers);
        PlayerPrefs.SetInt("MaxPlayers", maxPlayers);
        PlayerPrefs.SetInt("Audience", enableAudience ? 1 : 0);
        SceneManager.LoadScene("Game");
    }
}
