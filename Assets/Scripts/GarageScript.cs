using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageScript : MonoBehaviour
{
    public static GarageScript instance;

    public GameObject[] bus;
    public int busNum;
    public GameObject[] specs;
    public int[] adsNum;
    public int[] coinsNum;
    public int[] cashNum;
    public GameObject NextButton;
    public GameObject PreviousButton;
    public GameObject RewardButton;
    public Text RewardText;
    public GameObject CashButton;
    public Text CashText;
    public GameObject CoinsButton;
    public Text CoinsText;
    public Button GaragePlayButton;
    public Text TotalCashText;
    public Text TotalCoinsText;
    public GameObject UnlockAllBuses;
    public GameObject InsufficientCoins;
    public GameObject InsufficientCash;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        PlayerPrefs.SetInt("Bus0", 1);
        busNum = PlayerPrefs.GetInt("Bus");
        ActivateBus();
        TotalCashText.text = PlayerPrefs.GetInt("Cash").ToString();
        TotalCoinsText.text = PlayerPrefs.GetInt("Coins").ToString();

        if (PlayerPrefs.GetInt("Bus0") == 1 &&
            PlayerPrefs.GetInt("Bus1") == 1 &&
            PlayerPrefs.GetInt("Bus2") == 1 &&
            PlayerPrefs.GetInt("Bus3") == 1 &&
            PlayerPrefs.GetInt("Bus4") == 1)
        {
            UnlockAllBuses.SetActive(false);
        }
    }


    public void ActivateBus()
    {
        #region Activating Bus Object
        for (int i = 0; i < bus.Length; i++)
        {
            if (busNum == i)
            {
                bus[i].SetActive(true);
                specs[i].SetActive(true);
            }
            else
            {
                bus[i].SetActive(false);
                specs[i].SetActive(false);
            }
        }
        #endregion

        #region Activate Or Deactivate Next And Previous BUttons
        if (busNum < bus.Length - 1)
        {
            NextButton.SetActive(true);
        }
        else
        {
            NextButton.SetActive(false);
        }

        if(busNum > 0)
        {
            PreviousButton.SetActive(true);
        }
        else
        {
            PreviousButton.SetActive(false); 
        }
        #endregion

        #region Activate Or Deactivate RewardedVideo Button
        if (PlayerPrefs.GetInt("Bus"+busNum) == 1)
        {
            RewardButton.SetActive(false);
        }
        else
        {
            RewardButton.SetActive(true);
            RewardText.text = "GET BUS (" + PlayerPrefs.GetInt("RewardBus" + busNum) +"/" + adsNum[busNum]+")";
        }
        #endregion

        #region Activate Cash Or Coins Button
        if (PlayerPrefs.GetInt("Bus"+busNum) == 1)
        {
            CashButton.SetActive(false);
            CoinsButton.SetActive(false);
        }
        else
        {
            if(busNum == 1 || busNum == 3)
            {
                CoinsButton.SetActive(true);
                CashButton.SetActive(false);
                CoinsText.text = coinsNum[busNum].ToString();
            }
            else
            {
                CoinsButton.SetActive(false);
                CashButton.SetActive(true);
                CashText.text = cashNum[busNum].ToString();
            }
        }
        #endregion

        #region Activate Or Deactivate Play Button
        if (PlayerPrefs.GetInt("Bus" + busNum) == 1)
        {
            GaragePlayButton.gameObject.SetActive(true);
        }
        else
        {
            GaragePlayButton.gameObject.SetActive(false);
        }
        #endregion
    }

    public void NextBus()
    {
        if(busNum < bus.Length - 1)
        {
            busNum += 1; 
        }
        ActivateBus();
    }

    public void PreviousBus()
    {
        if(busNum > 0)
        {
            busNum -= 1;
        }
        ActivateBus();
    }


    public void UnlockByReward()  // Here We Send Call For Rewarded Ad
    {
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowAdmobRewardedAd("BusAd");
        }
    }


    public void UnlockedBusByVideo(int num)  // Bus From Array 0   // Here We Get Reward After Watching Ad
    {
        PlayerPrefs.SetInt("RewardBus"+num, PlayerPrefs.GetInt("RewardBus"+num) + 1);
        if (PlayerPrefs.GetInt("RewardBus"+num) == adsNum[num])
        {
            PlayerPrefs.SetInt("Bus"+num, 1);
        }
        ActivateBus();
    }

    public void UnlockByCash()
    {
        if(PlayerPrefs.GetInt("Cash") >= cashNum[busNum])
        {
            PlayerPrefs.SetInt("Bus"+busNum , 1);
            PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") - cashNum[busNum]);
            TotalCashText.text = PlayerPrefs.GetInt("Cash").ToString();
        }
        else
        {
            ActivateInsufficientCash();
        }
        ActivateBus();
    }

    public void UnlockByCoins()
    {
        if (PlayerPrefs.GetInt("Coins") >= coinsNum[busNum])
        {
            PlayerPrefs.SetInt("Bus"+busNum, 1);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - coinsNum[busNum]);
            TotalCoinsText.text = PlayerPrefs.GetInt("Coins").ToString();
        }
        else
        {
            ActivateInsufficientCoins();
        }
        ActivateBus();
    }
   

    void ActivateInsufficientCash()
    {
        InsufficientCash.SetActive(true);
        Invoke("DeactivateInsufficientCash", 1);
    }
    void DeactivateInsufficientCash()
    {
        InsufficientCash.SetActive(false);
    }

    void ActivateInsufficientCoins()
    {
        InsufficientCoins.SetActive(true);
        Invoke("DeactivateInsufficientCoins", 1);
    }
    void DeactivateInsufficientCoins()
    {
        InsufficientCoins.SetActive(false);
    }

}
