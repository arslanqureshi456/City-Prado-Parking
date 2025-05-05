using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{

    public static MainMenu instance;

    public AudioSource btnsound;
    public GameObject LoadingPanel;
    public GameObject[] levels;
    public GameObject levelPlayButton;
    public GameObject MainMenuPanel;
    public GameObject ModeSelection;
    public GameObject LevelSelection;
    public GameObject BusSelection;
    public GameObject QuitPanel;
    public Text[] totalCashText;
    public Text[] totalCoinsText;
    public GameObject RemoveAdsButton;
    public GameObject UnlockEverythingButton;
    public Toggle musicToggle;
    public GameObject toggleImage;
    public GameObject MainMusic;
    public GameObject settingsPanel;
    public GameObject dailyRewardPanel;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Time.timeScale = 1;

        if (GameManager.Instance.isNextLevel)
        {
            PlayModeSelection();
        }
        else
        {
            if (AdsManager.instance != null)
            {
                AdsManager.instance.ShowTopSmallBanner();
            }
        }

        int l = PlayerPrefs.GetInt("unlocklevel");
        int c = PlayerPrefs.GetInt("completed");

       
        for (int i = 0; i <= l; i++)
        {
            levels[i].GetComponent<Button>().interactable = true;
            levels[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        for (int i = 1;i <= c; i++)
        {
            levels[i-1].transform.GetChild(0).gameObject.GetComponent<Text>().text = "COMPLETED";
            levels[i - 1].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.green;
        }

        for(int i = 0; i < totalCashText.Length; i++)
        {
            totalCashText[i].text = PlayerPrefs.GetInt("Cash").ToString();
        }
        for (int i = 0; i < totalCoinsText.Length; i++)
        {
            totalCoinsText[i].text = PlayerPrefs.GetInt("Coins").ToString();
        }

        if(PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            RemoveAdsButton.SetActive(false);
        }
        if(PlayerPrefs.GetInt("Bus0") == 1 &&
            PlayerPrefs.GetInt("Bus1") == 1 &&
            PlayerPrefs.GetInt("Bus2") == 1 &&
            PlayerPrefs.GetInt("Bus3") == 1 &&
            PlayerPrefs.GetInt("Bus4") == 1 &&
            PlayerPrefs.GetInt("RemoveAds") == 1)
        {
            UnlockEverythingButton.SetActive(false);
        }

        if(PlayerPrefs.GetInt("Music") == 1) // set Int 1 to turn OFF the music
        {
            musicToggle.isOn = false;
            MainMusic.SetActive(false);
            toggleImage.SetActive(false);
        }
        else // set Int 0 to turn ON the music
        {
            musicToggle.isOn = true;
            MainMusic.SetActive(true);
            toggleImage.SetActive(true);
        }

        //if (Dailyreward.instance.CanClaimReward)
        //{
        //    dailyRewardPanel.SetActive(true);
        //}

    }

    public void PlayButton()
    {
        //LoadingPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        BusSelection.SetActive(true);
        //Invoke("AwaitPlayButton", 2);
        //if (AdsManager.instance != null)
        //{
        //    AdsManager.instance.ShowBottomRightCubeBanner();
        //    AdsManager.instance.RemoveTopSmallBanner();
        //}
        if(ANALYTICS.instance != null)
        {
            ANALYTICS.instance.CustomAnalytics("MainPlayButton");
        }
    }

    void AwaitPlayButton()
    {
        LoadingPanel.SetActive(false);
        BusSelection.SetActive(true);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
            AdsManager.instance.ShowTopSmallBanner();
        }
    }

    public void PlayBusSelection()
    {
        LoadingPanel.SetActive(true);
        BusSelection.SetActive(false);
       // Invoke("AwaitPlayBusSelection", 2);
        Invoke("AwaitPlayModeSelection", 2);
        PlayerPrefs.SetInt("Bus", GarageScript.instance.busNum);

        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowBottomRightCubeBanner();
            AdsManager.instance.RemoveTopSmallBanner();
            AdsManager.instance.ShowBothInterstitial();
        }
        if (ANALYTICS.instance != null)
        {
            ANALYTICS.instance.CustomAnalytics("GaragePlayButton");
        }
    }
    void AwaitPlayBusSelection()
    {
        LoadingPanel.SetActive(false);
        ModeSelection.SetActive(true);
        //PlayerPrefs.SetInt("Bus", GarageScript.instance.busNum);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
            AdsManager.instance.ShowTopSmallBanner();
        }
    }

    public void PlayModeSelection()
    {
        GameManager.Instance.isNextLevel = false;
       // LoadingPanel.SetActive(true);
        ModeSelection.SetActive(false);
        LevelSelection.SetActive(true);
        //Invoke("AwaitPlayModeSelection", 2);
        //if (AdsManager.instance != null)
        //{
        //    AdsManager.instance.ShowBottomRightCubeBanner();
        //    AdsManager.instance.RemoveTopSmallBanner();
        //}
        if (ANALYTICS.instance != null)
        {
            ANALYTICS.instance.CustomAnalytics("ModePlayButton");
        }
    }

    void AwaitPlayModeSelection()
    {
        LoadingPanel.SetActive(false);
        LevelSelection.SetActive(true);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
            AdsManager.instance.ShowTopSmallBanner();
        }
    }


    public void GameLevel(int level)
    {
        GameManager.Instance.CurrentLevel = level;
        levelPlayButton.SetActive(true);
    }

    public void PlayLevelButton()
    {
        LoadingPanel.SetActive(true);
        Invoke("AwaitGameplay", 2);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowBottomRightCubeBanner();
            AdsManager.instance.RemoveTopSmallBanner();
        }
        if (ANALYTICS.instance != null)
        {
            ANALYTICS.instance.CustomAnalytics("LevelsPlayButton");
        }
    }

    void AwaitGameplay()
    {
        SceneManager.LoadScene(2);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowBothInterstitial();
        }
    }

    public void QuitGame()
    {
        MainMenuPanel.SetActive(false);
        QuitPanel.SetActive(true);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowBottomRightCubeBanner();
            AdsManager.instance.RemoveTopSmallBanner();
        }
    }

    public void YesQuit()
    {
        QuitPanel.SetActive(false);
        Application.Quit();
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
        }
    }

    public void NoQuit()
    {
        MainMenuPanel.SetActive(true);
        QuitPanel.SetActive(false);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
            AdsManager.instance.ShowTopSmallBanner();
        }
    }

    public void Get100FreeCoins()
    {
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowAdmobRewardedAd("FreeCoins");
        }
    }

    public void PlayButtonSound()
    {
        btnsound.Play();
    }

    public void OnMusicToggle(Toggle toggle)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt("Music", 0); // set Int 0 to turn ON the music
            MainMusic.SetActive(false);
            toggleImage.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1); // set Int 1 to turn OFF the music
            MainMusic.SetActive(true) ;
            toggleImage.SetActive(true);
        }
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveTopSmallBanner();
            AdsManager.instance.ShowBottomRightCubeBanner();
        }
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowTopSmallBanner();
            AdsManager.instance.RemoveBottomRightCubeBanner();
        }
    }

    public void RateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id="+Application.identifier);
    }

    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=9204408892431752941");
    }

    public void PrivacyPolicy()
    {
        Application.OpenURL("https://trilogixs.com/trilogixs/");
    }
}
