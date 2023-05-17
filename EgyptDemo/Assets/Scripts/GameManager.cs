using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int coins;
    [SerializeField] int lives;
    public ParticleSystem coinEffect;
    [SerializeField] Animator coinAnim;

    //UI
   [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] Image[] hearts;
    [SerializeField] GameObject[] brokenHearts;
    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
            instance = this;
        lives = 3;
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
        coinsText.text = coins.ToString();
    }
    public void LoseLife(int lives_ = 1)
    {
        lives -= lives_;
        if(lives<=0)
        {
            return;
        }
        hearts[lives].enabled = false;
        brokenHearts[lives].SetActive(true);
    }
}
