using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndReveal : MonoBehaviour
{
    public GameObject[] itemsToHide;
    public GameObject[] itemsToReveal;

    public void HideAndRevealItems()
    {
        foreach (GameObject itemToHide in itemsToHide)
        {
            itemToHide.SetActive(false);
        }
        
        foreach (GameObject itemToReveal in itemsToReveal)
        {
            itemToReveal.SetActive(true);
        }
    }
}
