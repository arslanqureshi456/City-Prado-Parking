using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BusController : MonoBehaviour
{
    public static BusController instance;

    public GameObject VariantBus;
    public Material L2ContainerMobile;
    public Material L2ContainerTransparent;


    // Start is called before the first frame update
    void Start()
    {
       instance = this;
    }

    int trig = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            if (trig == 0)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = true;
                GameplayScript.instance.Controlls.SetActive(false);
                GameplayScript.instance.HideCanvasButtons();
                


                Invoke("StartRotate", 2);
                Invoke("Complete", 5);
                trig = 1;
            }
        }

        else if(other.tag == "Level2Tutorial")
        {
            GameplayScript.instance.level2TutorialPanel.SetActive(true);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.SetActive(false);
            GameplayScript.instance.isTutorialTimer = true;
        }
        else if (other.tag == "Level3Tutorial")
        {
            GameplayScript.instance.level3TutorialPanel.SetActive(true);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.SetActive(false);
            GameplayScript.instance.isTutorialTimer = true;
        }

        else if(other.tag == "TransparentContainer")
        {
            other.GetComponent<MeshRenderer>().material = L2ContainerTransparent;
        }

        else if ( other.tag == "Lifter")
        {
            other.gameObject.SetActive(false) ;
            GameplayScript.instance.FadeImage.SetActive(true);
            StartCoroutine(LifterAnimation());
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "TransparentContainer")
        {
            other.GetComponent<MeshRenderer>().material = L2ContainerMobile;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Hurdle")
        {
            GameplayScript.instance.HealthDecrement();
            GameObject.Find("HurdleSound").GetComponent<AudioSource>().Play();
            GameplayScript.instance.HitFadeImage.SetActive(true);
#if UNITY_EDITOR
            print("Get Damage : " + collision.gameObject.name);
#endif
        }
    }

    public IEnumerator LifterAnimation()
    {
        yield return new WaitForSecondsRealtime(1);
        GameplayScript.instance.Controlls.SetActive(false);
        GetComponent<RCC_CarControllerV3>().canControl = false;
        GetComponent<RCC_CarControllerV3>().speed = 0;
        GetComponent<Rigidbody>().isKinematic = true;
        if (GameManager.Instance.CurrentLevel == 7)
        {
            transform.position = new Vector3(7.35f, 1.56f, -28.38f);
        }else
        {
            transform.position = new Vector3(-12.9f, 1.56f, -5.9f);
            transform.rotation = Quaternion.Euler(0,-173f,0);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSecondsRealtime(2);
        GameplayScript.instance.LevelData[GameManager.Instance.CurrentLevel-1].transform.GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3);
        GetComponent<RCC_CarControllerV3>().canControl = true;
        GameplayScript.instance.Controlls.SetActive(true);
        GameplayScript.instance.FadeImage.SetActive(false);
    }

    void StartRotate()
    {
        GameplayScript.instance.MainControllerCam.SetActive(false);
        Instantiate(VariantBus , new Vector3 (transform.position.x, transform.position.y, transform.position.z) , transform.rotation);
        gameObject.SetActive(false);
    }
    void Complete()
    {
        GameplayScript.instance.LevelComplete();
    }



}
