using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinReactivator : MonoBehaviour
{
    //reactivates coins over time, value an be changed in inspector
   
    [Header("Editable Values")]
    public float timeBetweenReactivations;
    private float currentTime;

    [Header("References")]
    [SerializeField] GameObject myCoin;
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
