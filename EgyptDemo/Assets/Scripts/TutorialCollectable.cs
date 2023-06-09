using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCollectable : MonoBehaviour
{
    //used for the tutorial progression 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TutorialManager.instance.CollectCoin();
            gameObject.SetActive(false);
        }
    }
}
