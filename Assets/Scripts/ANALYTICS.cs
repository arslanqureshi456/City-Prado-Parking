using UnityEngine;
using UnityEngine.UI;
public class ANALYTICS : MonoBehaviour
{
    public static ANALYTICS instance;
//	public Text info;	
    void Awake()
    {
        
			instance = this;
			DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
                Debug.Log("Correctly configured firebase//////////////////////////////");
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

    }
    public void levelStartAnalytics()
    {
        try
        {
            Debug.Log("LevelStart_" + GameManager.Instance.CurrentLevel);
            Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelStart" + GameManager.Instance.CurrentLevel);
            //			info.text="data send";
        }
        catch
        {
            //     // Debug.Log("analytics not sent");
            //          info.text="data not send";
        }

    }
    public void CustomAnalytics(string s)
    {
        try
        {
            Debug.Log(s);
            Firebase.Analytics.FirebaseAnalytics.LogEvent(s);
            //			info.text="data send";
        }
        catch
        {
            //     // Debug.Log("analytics not sent");
            //          info.text="data not send";
        }

    }
    public void levelFailAnalytics()
	{
        Debug.Log("LevelFail_" + GameManager.Instance.CurrentLevel);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelFail" + GameManager.Instance.CurrentLevel);
    }
   public void levelCompleteAnalytics()
	{
        Debug.Log("LevelComplete_" + GameManager.Instance.CurrentLevel);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelComplete" + GameManager.Instance.CurrentLevel);
    }

    public void levelPausedAnalytics()
    {
        Debug.Log("LevelPaused_" + GameManager.Instance.CurrentLevel);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelPaused" + GameManager.Instance.CurrentLevel);
    }

    public void levelSkippedAnalytics()
    {
        Debug.Log("LevelSkipped_" + GameManager.Instance.CurrentLevel);
        Firebase.Analytics.FirebaseAnalytics.LogEvent("LevelSkipped" + GameManager.Instance.CurrentLevel);
    }

}
