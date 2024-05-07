using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleSubtract : MonoBehaviour
{
    public AudioSource burst;
    public void OnMouseDown()
    {
        GameManager.TriangleSubtract();
        burst.Play();
    }
}
