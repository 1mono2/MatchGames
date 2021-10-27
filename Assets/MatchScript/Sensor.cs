using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    internal bool isEnaleToGo = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("stage"))
        {
            isEnaleToGo = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("stage"))
        {
            isEnaleToGo = false ;
        }
    }
}
