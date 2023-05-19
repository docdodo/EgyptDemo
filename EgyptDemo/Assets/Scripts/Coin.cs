using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    //simple coin class that adds score when collected
     
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            CollectCoin();
        }
    }
    void CollectCoin()
    {
        GameManager.instance.AddCoin();
        gameObject.SetActive(false);
    }
}
