using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyChaser : MonoBehaviour
{
    //chases player down using navmesh AI
    [Header("Editable Values")]
    //After a certain amount of time the enemy will respawn on its own. This is to prevent all the enemies from getting stck following the player from behind
    [SerializeField] float lifetimeMax;
    float lifeTimeCurrent;

    [Header("References")]
    [SerializeField] Transform player;
    [SerializeField] GameObject hitPlayerEffect;
    [SerializeField] GameObject deathEffect;
    NavMeshAgent agent;
   
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
            GameObject.Instantiate(deathEffect, transform.position, transform.rotation);
            transform.position = GameManager.instance.enemySpawns[Random.Range(0, GameManager.instance.enemySpawns.Length)].position;

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
