using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndReveal : MonoBehaviour
{
    public GameObject itemToHide;
    public GameObject itemToReveal;
    
    public void HideAndRevealItems()
    {
        itemToHide.SetActive(false);
        itemToReveal.SetActive(true);
    }
}
