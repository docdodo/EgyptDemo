using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyChaser : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] GameObject hitPlayerEffect;
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
            GameManager.instance.LoseLife();
            GameObject.Instantiate(hitPlayerEffect, transform.position, transform.rotation);
            transform.position = GameManager.instance.enemySpawn.position;
        }
    }
}
