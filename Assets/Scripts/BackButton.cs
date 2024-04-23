using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BackButton : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.BackButtonPressed();
                UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
            }
        }
    }
}