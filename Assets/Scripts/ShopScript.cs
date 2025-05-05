using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public static ShopScript instance;

    #region Coins Data
    public int[] coinsAdNum;
    public Text Coins2RewardText;
    public Text Coins3RewardText;
    public int coinsoffernum;
    #endregion

    #region Cash Data
    public int[] cashAdNum;
    public Text Cash2RewardText;
    public Text Cash3RewardText;
    public int cashoffernum;
    #endregion


    public Image[] toggleImage;
    public GameObject[] shopPanel;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Coins2RewardText.text = PlayerPrefs.GetInt("RewardCoins1").ToString();
        Coins3RewardText.text = PlayerPrefs.GetInt("RewardCoins2").ToString();

        Cash2RewardText.text = PlayerPrefs.GetInt("RewardCash1").ToString();
        Cash3RewardText.text = PlayerPrefs.GetInt("RewardCash2").ToString();
    }

    #region Coins Data
    public void GetCoinsByReward(int num)
    {
        coinsoffernum = num;
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowAdmobRewardedAd("GetCoins");
        }
        //GotCoinsByReward(num);
    }

    public void GotCoinsByReward(int num)
    {
        PlayerPrefs.SetInt("RewardCoins" + num, PlayerPrefs.GetInt("RewardCoins" + num)+1);
        Coins2RewardText.text = PlayerPrefs.GetInt("RewardCoins1").ToString();
        Coins3RewardText.text = PlayerPrefs.GetInt("RewardCoins2").ToString();
        if (PlayerPrefs.GetInt("RewardCoins" + num) >= coinsAdNum[num])
        {
            if(num == 0)
            {
                print("Coins1 Rewarded");
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1000);
            }
            else if (num == 1)
            {
                print("Coins2 Rewarded");
                PlayerPrefs.SetInt("RewardCoins1", 0);
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 2000);
                Coins2RewardText.text = "0";
            }
            else if (num == 2)
            {
                print("Coins3 Rewarded");
                PlayerPrefs.SetInt("RewardCoins2", 0);
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 3000);
                Coins3RewardText.text = "0";
            }
        }
    }
    #endregion


    #region Cash Data
    public void GetCashByReward(int num)
    {
        cashoffernum = num;
        if (AdsManager.instance != null)
        {
            AdsManager.instance.ShowAdmobRewardedAd("GetCash");
        }
        //GotCashByReward(num);
    }

    public void GotCashByReward(int num)
    {
        PlayerPrefs.SetInt("RewardCash" + num, PlayerPrefs.GetInt("RewardCash" + num) + 1);
        Cash2RewardText.text = PlayerPrefs.GetInt("RewardCash1").ToString();
        Cash3RewardText.text = PlayerPrefs.GetInt("RewardCash2").ToString();
        if (PlayerPrefs.GetInt("RewardCash" + num) >= cashAdNum[num])
        {
            if (num == 0)
            {
                print("Cash1 Rewarded");
                PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 1000);
            }
            else if (num == 1)
            {
                print("Cash2 Rewarded");
                PlayerPrefs.SetInt("RewardCash1", 0);
                PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 2000);
                Cash2RewardText.text = "0";
            }
            else if (num == 2)
            {
                print("Cash3 Rewarded");
                PlayerPrefs.SetInt("RewardCash2", 0);
                PlayerPrefs.SetInt("Cash", PlayerPrefs.GetInt("Cash") + 3000);
                Cash3RewardText.text = "0";
            }
        }
    }
    #endregion

    public void PanelOn(int num)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i == num)
            {
                toggleImage[i].GetComponent<Image>().enabled = true;
                shopPanel[i].SetActive(true);
            }
            else
            {
                toggleImage[i].GetComponent<Image>().enabled = false;
                shopPanel[i].SetActive(false);
            }
        }
       

    }


}
