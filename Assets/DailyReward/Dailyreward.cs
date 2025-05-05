using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Dailyreward : MonoBehaviour
{
    public static Dailyreward instance;
    // The time when the player last claimed their reward
    private DateTime lastClaimTime;

    //public int[] rewardAmounts;


    public GameObject[] weekdays;
    public GameObject[] claimed;

    public int currentDay;
    public bool RewardClicked = false;
    public Text text;
    // The duration of the cooldown period (in hours)
    private float cooldownHours = 24f;

    // The time remaining until the player can claim their next reward
    private TimeSpan timeUntilNextReward;

    public Button ClaimRewardButton;

    // Whether or not the player can currently claim their reward
    public bool CanClaimReward
    {
        get { return timeUntilNextReward <= TimeSpan.Zero; }
    }

    // The text object to display the time remaining until the next reward can be claimed
    public TMPro.TextMeshProUGUI timeRemainingText;

    void Start()
    {
        instance = this;
        RewardClicked = false;
        if (!PlayerPrefs.HasKey("currentDay"))
        {
            PlayerPrefs.SetInt("currentDay", 0);
        }


        // Load the last claim time from player preferences
        lastClaimTime = DateTime.Parse(PlayerPrefs.GetString("lastClaimTime", DateTime.MinValue.ToString()));

        // Calculate the time until the player can claim their next reward
        timeUntilNextReward = lastClaimTime.AddHours(cooldownHours) - DateTime.UtcNow;    // 24 hours------------
        //timeUntilNextReward = lastClaimTime.AddSeconds(cooldownHours) - DateTime.UtcNow;  // After your given second

        // If the time until the next reward is negative, reset it to zero
        if (timeUntilNextReward < TimeSpan.Zero)
        {
            timeUntilNextReward = TimeSpan.Zero;
        }

        if (!PlayerPrefs.HasKey("Claimedno"))
        {
            RewardClicked = false;
        }
        else
        {
            RewardClicked = true;
        }

        if (CanClaimReward)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            ClaimRewardButton.interactable = true;
            isAnimate = true;
            ClaimRewardButton.gameObject.SetActive(true);
        }
        else
        {
            ClaimRewardButton.gameObject.SetActive(false);
        }
    }
    bool isAnimate;
    void Update()
    {
        if (!CanClaimReward)
        {
            // Update the time remaining until the player can claim their next reward

             timeUntilNextReward = lastClaimTime.AddHours(cooldownHours) - DateTime.UtcNow;    // 24 hours------------

           // timeUntilNextReward = lastClaimTime.AddSeconds(cooldownHours) - DateTime.UtcNow;  // After your given second 

            // Update the time remaining text
            if (timeRemainingText != null)
            {
                timeRemainingText.text = "You may Claim Reward In : " + timeUntilNextReward.ToString("hh\\:mm\\:ss");
            }

           // weekdays[PlayerPrefs.GetInt("currentDay")].GetComponent<DOTweenAnimation>().DOKill();
        }
        else
        {
            // Hide the time remaining text if the player can claim their reward
            if (timeRemainingText != null)
            {
                timeRemainingText.gameObject.SetActive(false);

                // unclock all reward afer completing 7 days 
                if (PlayerPrefs.GetInt("currentDay") == 7)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        print("For---");
                        if (i <= PlayerPrefs.GetInt("currentDay"))
                        {
                            weekdays[i].transform.GetChild(1).gameObject.SetActive(false);
                        }
                    }
                    PlayerPrefs.SetInt("Claimedno", 0);
                    PlayerPrefs.SetInt("currentDay", 0);
                }
            }
            //if(isAnimate)
            //weekdays[PlayerPrefs.GetInt("currentDay")].GetComponent<DOTweenAnimation>().DOPlay();
        }

        // Check Claimed reward
        if (RewardClicked == true)
        {
            for (int i = 0; i < PlayerPrefs.GetInt("Claimedno"); i++)
            {
                weekdays[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        print("Reward Day : " + PlayerPrefs.GetInt("currentDay"));

       // if (isAnimate)
          //  weekdays[PlayerPrefs.GetInt("currentDay")].GetComponent<DOTweenAnimation>().DOPlay();
    }
    //int CoinsAmount
    public int no = 0;
    public void ClaimReward()
    {
        if (CanClaimReward)
        {
            // Give the player their reward and update the last claim time
            // ...

            if (PlayerPrefs.GetInt("currentDay") == 0)
            {
                // text.text = "" + 10;
                PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 100);
                print("Day_1");
            }
            else if (PlayerPrefs.GetInt("currentDay") == 1)
            {
                //text.text = "" + 20;
                PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 500);
                print("Day_2");
            }
            else if (PlayerPrefs.GetInt("currentDay") == 2)
            {
                // text.text = "" + 30;
                PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 750);
                print("Day_3");
            }
            else if (PlayerPrefs.GetInt("currentDay") == 3)
            {
                // text.text = "" + 40;
                PlayerPrefs.SetInt("Bus4" , 1);
                print("Day_4");
            }
            else if (PlayerPrefs.GetInt("currentDay") == 4)
            {
                // text.text = "" + 50;
                PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 1000);
                print("Day_5");
            }
            else if (PlayerPrefs.GetInt("currentDay") == 5)
            {
                //text.text = "" + 60;
                PlayerPrefs.SetInt("RemoveAds", 1);
                MainMenu.instance.RemoveAdsButton.SetActive(false);
                if(AdsManager.instance != null)
                {
                    AdsManager.instance.RemoveAllBanners();
                }
                print("Day_6");
            }
            else if (PlayerPrefs.GetInt("currentDay") == 6)
            {
                //text.text = "" + 70;
                PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 2000);
                print("Day_7");
            }

            // Take all Claimed image in array
            if (claimed == null)
            {
                claimed = GameObject.FindGameObjectsWithTag("Claimed");
            }
            // Setactive Claimed Reward image
            if (PlayerPrefs.GetInt("Claimedno") == PlayerPrefs.GetInt("currentDay"))
            {
                weekdays[PlayerPrefs.GetInt("Claimedno")].transform.GetChild(1).gameObject.SetActive(true);
            }

            PlayerPrefs.SetInt("currentDay", PlayerPrefs.GetInt("currentDay") + 1);
            PlayerPrefs.SetInt("Claimedno", PlayerPrefs.GetInt("currentDay"));

            lastClaimTime = DateTime.UtcNow;
               timeUntilNextReward = TimeSpan.FromHours(cooldownHours);  // 24 hours---------

           // timeUntilNextReward = TimeSpan.FromSeconds(cooldownHours);   // After your given second 

            // Save the last claim time to player preferences
            PlayerPrefs.SetString("lastClaimTime", lastClaimTime.ToString());
            PlayerPrefs.Save();

            //timeRemainingText.gameObject.SetActive(true);

            // Show the time remaining text again
            if (timeRemainingText != null)
            {
                timeRemainingText.gameObject.SetActive(true);
            }

            for (int i = 0; i < MainMenu.instance.totalCashText.Length; i++)
            {
                MainMenu.instance.totalCashText[i].text = PlayerPrefs.GetInt("Cash").ToString();
            }

            isAnimate = false;

        }
            weekdays[PlayerPrefs.GetInt("currentDay")].GetComponent<DOTweenAnimation>().enabled = false;
        ClaimRewardButton.gameObject.SetActive(false);
    }
}