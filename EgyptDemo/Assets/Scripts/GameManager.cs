using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int coins;
    public ParticleSystem coinEffect;
    [SerializeField] Animator coinAnim;
    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddCoin(int coins_=1)
    {
        coins += coins_;
        coinEffect.Play();
        coinAnim.SetTrigger("Collect");
    }
}
