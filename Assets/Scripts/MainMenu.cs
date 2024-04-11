using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
   public GameObject guestPanel;

   public void Awake()
   {
       if (GameManager.isGuest)
       {
           guestPanel.SetActive(true);
       }
       else
       {
           guestPanel.SetActive(false);
       }
   }

   public void playGame()
   {
       UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
   }
   public void Statistics()
   {
       if (GameManager.isGuest)
       {
           return;
       }
       UnityEngine.SceneManagement.SceneManager.LoadScene("Stats");
   }
   
   public void quitGame()
   {
       GameManager.isGuest = false;
       guestPanel.SetActive(false);
       Application.Quit();
   }
   public void ReasoningGame()
   {
       UnityEngine.SceneManagement.SceneManager.LoadScene("ReasoningScene");
   }
   public void Help()
   {
       UnityEngine.SceneManagement.SceneManager.LoadScene("Help");
   }
}
