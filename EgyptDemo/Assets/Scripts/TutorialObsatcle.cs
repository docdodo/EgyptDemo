using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TutorialObsatcle : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }
    private void Update()
    {
        agent.SetDestination(player.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            TutorialManager.instance.HitByEnemy();

        }
    }
}
