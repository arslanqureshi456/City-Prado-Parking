using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class spinTimer : MonoBehaviour
{

    DateTime startTime;
    TimeSpan currentTime;
    //TimeSpan oneDay = new TimeSpan(24, 0, 0);
    TimeSpan oneDay = new TimeSpan(0, 2, 0);
    public GameObject reward;


    public GameObject timmer;


    void Start()
    {
        StartTime();
    }

    void Update()
    {
        currentTime = DateTime.UtcNow - startTime;
        if (currentTime >= oneDay)
        {
            //set daily reward to active to give player 500 coins
          //  reward.SetActive(true);
            reward.GetComponent<Button>().interactable = true;
            StartTime();
        }
        else
        {
          //  reward.SetActive(false);
        }
        timmer.GetComponent<Text>().text = " " + oneDay;
    }

    private void StartTime()
    {
        startTime = DateTime.UtcNow;
    }

    //Returns a string with currentTime in a 24:00:00 format
    private string GetTime(TimeSpan time)
    {
        TimeSpan countdown = oneDay - time;
        return countdown.Hours.ToString() + ":" + countdown.Minutes.ToString()
            + ":" + countdown.Seconds.ToString();


     
    }

    public void GiveRewar()
    {
        print("Give_Reward-------");
        reward.GetComponent<Button>().interactable= false;
    }








    //public Button spinButton;
    //// The time when the player last claimed their reward
    //private DateTime lastClaimTime;

    //// The duration of the cooldown period (in hours)
    //private float cooldownHours = 24f;

    //// The time remaining until the player can claim their next reward
    //private TimeSpan timeUntilNextReward;

    //// Whether or not the player can currently claim their reward
    //public bool CanClaimReward
    //{
    //    get { return timeUntilNextReward <= TimeSpan.Zero; }
    //}

    //void Start()
    //{
    //    // Load the last claim time from player preferences
    //    lastClaimTime = DateTime.Parse(PlayerPrefs.GetString("lastClaimTime", DateTime.MinValue.ToString()));

    //    // Calculate the time until the player can claim their next reward
    //    //  timeUntilNextReward = lastClaimTime.AddHours(cooldownHours) - DateTime.UtcNow; ----------------------------

    //    timeUntilNextReward = lastClaimTime.AddMinutes(1) - DateTime.UtcNow;

    //    // If the time until the next reward is negative, reset it to zero
    //    if (timeUntilNextReward < TimeSpan.Zero)
    //    {
    //        timeUntilNextReward = TimeSpan.Zero;
    //    }
    //}

    //void Update()
    //{
    //    if (!CanClaimReward)
    //    {
    //        // Update the time remaining until the player can claim their next reward
    //        //    timeUntilNextReward = lastClaimTime.AddHours(cooldownHours) - DateTime.UtcNow;-----------------------------

    //        timeUntilNextReward = lastClaimTime.AddMinutes(1) - DateTime.UtcNow;
    //    }
    //}

    //public void ClaimReward()
    //{
    //    // Give the player their reward and update the last claim time
    //    // ...

    //    spinButton.interactable = false;

    //    lastClaimTime = DateTime.UtcNow;
    //    timeUntilNextReward = TimeSpan.Zero;

    //    // Save the last claim time to player preferences
    //    PlayerPrefs.SetString("lastClaimTime", lastClaimTime.ToString());
    //    PlayerPrefs.Save();
    //}
}
