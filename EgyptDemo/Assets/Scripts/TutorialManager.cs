using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    private int tutorialPhase;
    private float currentTime;
    private int coinsCollected;
    [SerializeField] float timeRequiredToAvoidEnemy;
    [SerializeField] GameObject coins;
    [SerializeField] GameObject enemy;
    //tutorial text settings
    [SerializeField] string[] tutorialMessages;
    [SerializeField] TextMeshProUGUI tutorialTextField;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }
    private void Update()
    {
        if(tutorialPhase==2)
        {
            currentTime -= Time.deltaTime;
            if(currentTime<=0)
            {
                tutorialPhase = 3;
                AdvanceTutorialPhase();
            }
        }
        else if (tutorialPhase == 3)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                currentTime = 30;
                UnityEngine.SceneManagement.SceneManager.LoadScene(2);
            }
        }
    }
    private void AdvanceTutorialPhase()
    {
        tutorialTextField.text = tutorialMessages[tutorialPhase];
       switch(tutorialPhase)
        {
            
            //coin collection
            case 1:
                coins.SetActive(true);
                break;
            //enemy chase
            case 2:
                currentTime = timeRequiredToAvoidEnemy;
                enemy.SetActive(true);
                break;
            //tutDone
            case 3:
                currentTime = 5;
                enemy.SetActive(false);
                break;
        }
    }
    public void CollectCoin()
    {
        if(tutorialPhase==0)
        {
            tutorialPhase = 1;
            AdvanceTutorialPhase();
        }
        else
        {
            coinsCollected++;
            GameManager.instance.AddCoin();
            if(coinsCollected>=5)
            {
                tutorialPhase = 2;
                AdvanceTutorialPhase();
            }
        }
    }
    public void HitByEnemy()
    {
        currentTime = timeRequiredToAvoidEnemy;
    }
}
