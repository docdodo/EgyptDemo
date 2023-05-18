using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyChaser : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] GameObject hitPlayerEffect;
    [SerializeField] GameObject deathEffect;
    [SerializeField] float lifetimeMax;
    float lifeTimeCurrent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        lifeTimeCurrent = lifetimeMax;
    }
    private void Update()
    {
        agent.SetDestination(player.position);
        lifeTimeCurrent -= Time.deltaTime;
        if (lifeTimeCurrent <= 0)
        {
            lifeTimeCurrent = lifetimeMax;
            transform.position = GameManager.instance.enemySpawns[Random.Range(0, GameManager.instance.enemySpawns.Length)].position;
            GameObject.Instantiate(deathEffect, transform.position, transform.rotation);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.LoseLife();
            GameObject.Instantiate(hitPlayerEffect, transform.position, transform.rotation);
            transform.position = GameManager.instance.enemySpawns[Random.Range(0, GameManager.instance.enemySpawns.Length)].position;
            lifeTimeCurrent = lifetimeMax;
        }
    }
}
