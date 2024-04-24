using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BackButtonReasoning : MonoBehaviour
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