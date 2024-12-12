using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LinkRedirector : MonoBehaviour
{

    [SerializeField] private Button mailRedirectButton;
    [SerializeField] private Button discordRedirectButton;
    [SerializeField] private Button instagramRedirectButton;
    [SerializeField] private Button facebookRedirectButton;
    [SerializeField] private Button twitterRedirectorButton;
    [SerializeField] private Button marketPlaceRedirectorButton;
    [SerializeField] private TMP_Dropdown qualityDropDown;
    private int dropdownValue;


    void Awake()
    {

        dropdownValue = PlayerPrefs.GetInt("Dropdown", dropdownValue);
        QualitySettings.SetQualityLevel(dropdownValue);
        qualityDropDown.value = PlayerPrefs.GetInt("Dropdown", dropdownValue);

    }
    void Start()
    {

        //Fetch the Dropdown GameObject
        //Add listener for when the value of the Dropdown changes, to take action
        qualityDropDown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(qualityDropDown);
        });

        //Initialise the Text to say the first value of the Dropdown
    }

    void OnEnable()
    {

        mailRedirectButton.onClick.AddListener(SendEmail);
        discordRedirectButton.onClick.AddListener(DiscordRedirector);
        twitterRedirectorButton.onClick.AddListener(TwitterRedirector);
        instagramRedirectButton.onClick.AddListener(InstagramRedirector);
        facebookRedirectButton.onClick.AddListener(TelegramRedirector);
        marketPlaceRedirectorButton.onClick.AddListener(MarketplaceRedirector);

    }


    void SendEmail()
    {
        string email = "info@avaxcars.com";
        string subject = MyEscapeURL("Help");
        string body = MyEscapeURL("Please describe your problem briefly and concisely below:  \r \n");
        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }

    string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    private void DiscordRedirector()
    {

        Application.OpenURL("https://discord.gg/9TTvTPGB");

    }

    private void TwitterRedirector()
    {

        Application.OpenURL("https://twitter.com/AvaxCars");

    }

    private void InstagramRedirector()
    {

        Application.OpenURL("https://www.instagram.com/avaxcarsofficial");

    }

    private void TelegramRedirector()
    {

        Application.OpenURL("https://www.telegram.com");

    }


    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(TMP_Dropdown change)
    {

        QualitySettings.SetQualityLevel(change.value);
        dropdownValue = change.value;
        PlayerPrefs.SetInt("Dropdown", dropdownValue);

    }

    private void MarketplaceRedirector()
    {

        Application.OpenURL("https://testnets.opensea.io/collection/avaxcars-v2");

    }
}
