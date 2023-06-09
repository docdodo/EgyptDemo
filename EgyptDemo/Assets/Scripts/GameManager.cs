using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{//controls the gamestate inlcuing coins and lives and enemy spawn locations, etc
    [Header("Editable Values")]
    public Transform[] enemySpawns;
    [Header("References")]
    public static GameManager instance;
    [SerializeField] int coins;
    [SerializeField] int lives;
    [SerializeField] Animator coinAnim;
    [SerializeField] AudioSource coinAudioSource;
    [SerializeField] AudioSource enemyAudioSource;
    [Header("References UI")]
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] Image[] hearts;
    [SerializeField] GameObject[] brokenHearts;
    [SerializeField] GameObject LoseScreenUI;
    [SerializeField] GameObject RegularUI;
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
        coinAnim.SetTrigger("Collect");
        coinsText.text = coins.ToString();
        coinAudioSource.PlayOneShot(coinAudioSource.clip);
    }
    public void LoseLife(int lives_ = 1)
    {
        lives -= lives_;
        enemyAudioSource.PlayOneShot(enemyAudioSource.clip);
        if (lives<=0)
        {
            Time.timeScale = 0;
            
                LoseScreenUI.SetActive(true);
            
            RegularUI.SetActive(false);
            return;
        }
        hearts[lives].enabled = false;
        brokenHearts[lives].SetActive(true);
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void ReplayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
    public void PauseGame(bool pauseGame)
    {
        if (pauseGame)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
