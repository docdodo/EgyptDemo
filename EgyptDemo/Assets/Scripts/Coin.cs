using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
     
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
