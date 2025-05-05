using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Analytics;

public class GameplayScript : MonoBehaviour
{
    public static GameplayScript instance;

    public GameObject[] LevelData;
    public GameObject FadeImage;
    public GameObject pausePanel;
    public GameObject victoryPanel;
    public GameObject completePanel;
    public Text totalCashText;
    public Text totalCoinsText;
    public GameObject failPanel;
    public GameObject level2TutorialPanel;
    public GameObject level3TutorialPanel;
    public GameObject EnvironmentPlane;
    public Material[] envmat;
    public Material[] skymat;
    public GameObject directionalLight;
    public GameObject Rain;
    public GameObject Snow;
    public GameObject DayBuildings;
    public GameObject NightBuildings;
    public Vector3[] busPosition;
    public GameObject playerBus;
    public GameObject[] Bus;

    public int busHealth = 3;
    public Text playerHealthText;

    public GameObject MainControllerCam;

    public GameObject HitFadeImage;
    public GameObject Controlls;
    public GameObject[] CanvasButtons;

    [HideInInspector] public bool isTutorialTimer;
    float TimerCount = 4;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Time.timeScale = 1;
        busHealth = 3;
        playerHealthText.text = busHealth.ToString();
        LevelData[GameManager.Instance.CurrentLevel - 1].SetActive(true);
        EnvironmentPlane.GetComponent<MeshRenderer>().material = envmat[UnityEngine.Random.Range(0, envmat.Length)];
        playerBus = Instantiate(Bus[PlayerPrefs.GetInt("Bus")], Bus[PlayerPrefs.GetInt("Bus")].transform.position, Bus[PlayerPrefs.GetInt("Bus")].transform.rotation);
        Weather();
        Invoke("placement", 0.01f);

        if (ANALYTICS.instance != null)
        {
            ANALYTICS.instance.levelStartAnalytics();
        }
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
            AdsManager.instance.ShowTopSmallBanner();
        }
    }

    void placement()
    {
        playerBus.transform.position = busPosition[GameManager.Instance.CurrentLevel - 1];
        if (GameManager.Instance.CurrentLevel == 12)
        {
            playerBus.transform.rotation = Quaternion.Euler(0, -122.2f, 0);
        }
        else if (GameManager.Instance.CurrentLevel == 13 || GameManager.Instance.CurrentLevel == 23)
        {
            playerBus.transform.rotation = Quaternion.Euler(0, -147.7f, 0);
        }
        else if (GameManager.Instance.CurrentLevel == 15)
        {
            playerBus.transform.rotation = Quaternion.Euler(0, -150f, 0);
        }
        else if (GameManager.Instance.CurrentLevel == 17 )
        {
            playerBus.transform.rotation = Quaternion.Euler(0, -125f, 0);
        }
        else if (GameManager.Instance.CurrentLevel == 22)
        {
            playerBus.transform.rotation = Quaternion.Euler(0, -145f, 0);
        }
    }

    void Weather()
    {
        if (GameManager.Instance.CurrentLevel == 1)
        {
            // RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
            RenderSettings.skybox = skymat[0];
            DayBuildings.SetActive(true);
        }
        else
        {
            int rand = UnityEngine.Random.Range(0, 6);
            switch (rand)
            {
                case 0: // Day
                        // RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                    RenderSettings.skybox = skymat[0];
                    DayBuildings.SetActive(true);
                    break;
                case 1: // Night
                        //  RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                    RenderSettings.skybox = skymat[1];
                    directionalLight.SetActive(false);
                    NightBuildings.SetActive(true);
                    break;
                case 2: // DayRain
                    Rain.SetActive(true);
                    // RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                    RenderSettings.skybox = skymat[2];
                    DayBuildings.SetActive(true);
                    break;
                case 3: // NightRain
                    Rain.SetActive(true);
                    // RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                    RenderSettings.skybox = skymat[1];
                    directionalLight.SetActive(false);
                    NightBuildings.SetActive(true);
                    break;
                case 4: // DaySnow
                    Snow.SetActive(true);
                    // RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                    RenderSettings.skybox = skymat[2];
                    DayBuildings.SetActive(true);
                    break;
                case 5: // NightSnow
                    Snow.SetActive(true);
                    // RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                    RenderSettings.skybox = skymat[1];
                    directionalLight.SetActive(false);
                    NightBuildings.SetActive(true);
                    break;
                case 6: // NightSnow
                    Snow.SetActive(true);
                    // RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                    RenderSettings.skybox = skymat[1];
                    directionalLight.SetActive(false);
                    NightBuildings.SetActive(true);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTutorialTimer)
        {
            TimerCount -= Time.deltaTime;
        }

        if (TimerCount < 0)
        {
            if (tutotialbytime == 0)
            {
                TutorialByTimer();
                tutotialbytime = 1;
                TimerCount = 0;
            }
            isTutorialTimer = false;
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowBottomRightCubeBanner();
            AdsManager.instance.RemoveTopSmallBanner();
            AdsManager.instance.ShowBothInterstitial();
        }
        if (ANALYTICS.instance != null)
        {
            ANALYTICS.instance.levelPausedAnalytics();
        }
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        pausePanel.SetActive(false);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
            AdsManager.instance.ShowTopSmallBanner();
        }
    }

    public void LevelFail()
    {
        failPanel.SetActive(true);
        if (ANALYTICS.instance != null)
        {
            ANALYTICS.instance.levelFailAnalytics();
        }
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowBottomRightCubeBanner();
            AdsManager.instance.RemoveTopSmallBanner();
            AdsManager.instance.ShowBothInterstitial();
        }
    }

    public void LevelComplete()
    {
        victoryPanel.SetActive(true);
        if (ANALYTICS.instance != null)
        {
            ANALYTICS.instance.levelCompleteAnalytics();
        }
        // Here we have 14 levels as total levels in prefs , because prefs starts from 0 and ends at 14 here (it means we have 15 levels in total)
        if (PlayerPrefs.GetInt("unlocklevel") < GameManager.Instance.CurrentLevel && PlayerPrefs.GetInt("unlocklevel") < 24)
        {
            PlayerPrefs.SetInt("unlocklevel", GameManager.Instance.CurrentLevel);
        }

        if (PlayerPrefs.GetInt("completed") < GameManager.Instance.CurrentLevel && PlayerPrefs.GetInt("completed") < 25)
        {
            PlayerPrefs.SetInt("completed", GameManager.Instance.CurrentLevel);
        }
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowBottomRightCubeBanner();
            AdsManager.instance.RemoveTopSmallBanner();
            AdsManager.instance.ShowBothInterstitial();
        }
    }

    public void LevelCompleteContinue()
    {
        victoryPanel.SetActive(false);
        completePanel.SetActive(true);
        PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 250);
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1500);
        totalCashText.text = PlayerPrefs.GetInt("Cash").ToString();
        totalCoinsText.text = PlayerPrefs.GetInt("Coins").ToString();
    }
    public void Restart()
    {
        if (failPanel.activeInHierarchy)
        {
            AdsManager.instance.ShowBothInterstitial();
        }
        SceneManager.LoadScene(2);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
        }
    }

    public void Next()
    {
        if (GameManager.Instance.CurrentLevel == 25)
        {
            GameManager.Instance.CurrentLevel = 1;
        }
        else
        {
            GameManager.Instance.CurrentLevel = GameManager.Instance.CurrentLevel + 1;
        }
        SceneManager.LoadScene(2);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
            AdsManager.instance.ShowBothInterstitial();
        }
    }

    public void Home()
    {
        SceneManager.LoadScene(1);
        if (AdsManager.instance != null)
        {
            AdsManager.instance.RemoveBottomRightCubeBanner();
            AdsManager.instance.ShowBothInterstitial();
        }
    }

    public void TutorialPressed()
    {
        BusController.instance.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        GameObject.Find("Finish").GetComponent<BoxCollider>().enabled = true;
        isTutorialTimer = false;
        TimerCount = 4;
    }

    int tutotialbytime;
    void TutorialByTimer()
    {
        level2TutorialPanel.SetActive(false);
        level3TutorialPanel.SetActive(false);
        BusController.instance.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        BusController.instance.gameObject.AddComponent<RCC_AICarController>();
        BusController.instance.gameObject.GetComponent<RCC_AICarController>().useRaycasts = false;
        BusController.instance.gameObject.GetComponent<RCC_AICarController>().nextWaypointPassDistance = 5;
        GameObject.Find("Finish").GetComponent<BoxCollider>().enabled = false;
    }

    public void HealthDecrement()
    {
        busHealth -= 1;
        playerHealthText.text = busHealth.ToString();
        if (busHealth <= 0)
        {
            LevelFail();
        }
    }

    public void GetDoubleReward()
    {
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowAdmobRewardedAd("DoubleReward");
        }
    }

    public void SkipLevel()
    {
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowAdmobRewardedAd("SkipLevel");
        }
    }

    public void GotLevelSkipped()
    {
        if (PlayerPrefs.GetInt("unlocklevel") < GameManager.Instance.CurrentLevel && PlayerPrefs.GetInt("unlocklevel") < 24)
        {
            PlayerPrefs.SetInt("unlocklevel", GameManager.Instance.CurrentLevel);
        }

        if (PlayerPrefs.GetInt("completed") < GameManager.Instance.CurrentLevel && PlayerPrefs.GetInt("completed") < 25)
        {
            PlayerPrefs.SetInt("completed", GameManager.Instance.CurrentLevel);
        }
        if (ANALYTICS.instance != null)
        {
            ANALYTICS.instance.levelSkippedAnalytics();
        }
        if (GameManager.Instance.CurrentLevel == 25)
        {
            GameManager.Instance.CurrentLevel = 1;
        }
        else
        {
            GameManager.Instance.CurrentLevel = GameManager.Instance.CurrentLevel + 1;
        }
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }

    public void HideCanvasButtons()
    {
        for (int i = 0; i < CanvasButtons.Length; i++)
        {
            CanvasButtons[i].SetActive(false);
        }
    }
}
