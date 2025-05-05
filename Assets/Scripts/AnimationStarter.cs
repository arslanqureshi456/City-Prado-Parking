using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimationStarter : MonoBehaviour
{
    public GameObject[] gameObjects;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].GetComponent<DOTweenAnimation>().DOPlay();
            }
            Destroy(gameObject);
        }
    }
}
