using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] int coins;
    [SerializeField] int lives;
    public ParticleSystem coinEffect;
    [SerializeField] Animator coinAnim;
    public Transform enemySpawn;
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
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void PauseGame(bool pauseGame)
    {
        if (pauseGame)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
