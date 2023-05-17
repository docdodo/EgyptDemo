using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject shootingCam;
    [SerializeField] GameObject movingCam;
    public bool isMoving;

    private void Update()
    {
        if (isMoving)
            Moving();
    }
    void Moving()
    {
        if(rb.velocity.magnitude<1)
        {
            SetMoving(false);
        }
    }
    public void SetMoving(bool isMoving_)
    {
        if(isMoving)
        {
            shootingCam.SetActive(false);
            movingCam.SetActive(true);
        }
        else
        {
            shootingCam.SetActive(true);
            movingCam.SetActive(false);
        }
    }
}
