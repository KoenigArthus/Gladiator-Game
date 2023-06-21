using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissabearBox : MonoBehaviour
{
    public GameObject[] objectsToDissapear;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < objectsToDissapear.Length; i++)
        {
            objectsToDissapear[i].SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        for (int i = 0; i < objectsToDissapear.Length; i++)
        {
            objectsToDissapear[i].SetActive(true);
        }
    }
}
