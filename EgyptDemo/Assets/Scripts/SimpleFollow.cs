using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    //Used to have a camera target follow the player ball
   [SerializeField] Transform target;

    private void Update()
    {
        transform.position = target.position;
    }
}
