using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBoxButtons : MonoBehaviour
{
    public GameObject GearForward;
    public GameObject GearBackward;
    // Start is called before the first frame update
    void Update()
    {
        if(this.GetComponent<RCC_UIDashboardButton>().gearDirection == 0)
        {
            GearForward.SetActive(true);
            GearBackward.SetActive(false);
        }
        else if (this.GetComponent<RCC_UIDashboardButton>().gearDirection == 2)
        {
            GearForward.SetActive(false);
            GearBackward.SetActive(true);
        }
        else
        {
            GearForward.SetActive(true);
            GearBackward.SetActive(false);
        }
    }

}
