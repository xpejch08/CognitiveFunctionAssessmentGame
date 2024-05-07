using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSubtractScript : MonoBehaviour
{
    public AudioSource burst;
    public void OnMouseDown()
    {
        burst.Play();
        GameManager.CircleSubtract();
    }
}
