using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenuActivation : MonoBehaviour
{
    public Text availableUpgradeCountLabel;
    public GameObject availableUpgradeCountIndicator;
    private GameEventHandler _eventHandler;

    private void Awake()
    {
        _eventHandler = FindObjectOfType<GameEventHandler>();
    }

    private void OnEnable()
    {
        availableUpgradeCountIndicator.SetActive(_eventHandler.availableUpgrades.Count > 0);
        availableUpgradeCountLabel.text = _eventHandler.availableUpgrades.Count.ToString();
    }
}
