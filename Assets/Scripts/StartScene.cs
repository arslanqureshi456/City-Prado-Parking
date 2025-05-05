using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        //PlayerPrefs.SetInt("unlocklevel", 24);
        Invoke("LoadScene" , 7);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(1);
        //if(AdsManager.instance != null)
        //{
        //    AdsManager.instance.ShowStaticInterstitial();
        //}
    }

}
