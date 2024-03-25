using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   public void playGame()
   {
       UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
   }
   public void Statistics()
   {
       UnityEngine.SceneManagement.SceneManager.LoadScene("Stats");
   }
   
   public void quitGame()
   {
       Debug.Log("QUIT!");
       Application.Quit();
   }
   public void ReasoningGame()
   {
       UnityEngine.SceneManagement.SceneManager.LoadScene("ReasoningScene");
   }
}
