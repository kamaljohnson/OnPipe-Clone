using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstrecle"))
        {
            FindObjectOfType<Game>().GameOver();
        }
    }
}
