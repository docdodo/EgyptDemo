using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinReactivator : MonoBehaviour
{
   [SerializeField] GameObject myCoin;
    public float timeBetweenReactivations;
    float currentTime;
    private void Start()
    {
        currentTime = timeBetweenReactivations;
    }
    private void Update()
    {
        if(myCoin.active==false)
        {
            currentTime -= Time.deltaTime;
            if(currentTime<=0)
            {
                currentTime = timeBetweenReactivations;
                myCoin.SetActive(true);
            }
        }

    }
}
