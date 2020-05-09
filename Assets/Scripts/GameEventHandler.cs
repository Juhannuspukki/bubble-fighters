using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;


public class GameEventHandler : MonoBehaviour
{
    public Text pointLabel;
    public GameObject pauseMenu;
    public GameObject upgradeButton;
    public GameObject installedUpgradeLabel;
    public GameObject availableUpgradeView;
    public GameObject installedUpgradeView;
    public AudioSource npcGetHit;
    public AudioSource playerGetHit;
    public AudioSource playerWeapon;
    public AudioSource explosion;
    
    public int pointCount = 0;

    public List<string> unlockedUpgrades;

    private bool _isPaused = false;
    private UpgradeManager _upgradeManager;


    public void Awake()
    {
        _upgradeManager = FindObjectOfType<UpgradeManager>();
    }

    public void PauseGame()
    {
        Time.timeScale = _isPaused ? 1 : 0;
        _isPaused = !_isPaused;
        
        pauseMenu.SetActive(_isPaused);
        
        // Create menu items when the game is paused
        if (_isPaused)
        {
            CreatePauseMenuItems();
        }
        
        // Delete menu items when the game is no longer paused
        else
        {
            DeletePauseMenuItems();
        }
        
    }
    
    public void CreatePauseMenuItems()
    {
        // Create buttons from list of upgrades the player has unlocked
        foreach (string upgradeId in unlockedUpgrades)
        {

            ShipUpgrade upgrade = _upgradeManager.UpgradeData.Find(item => item.UpgradeId == upgradeId);

            bool prereqsMet = true;
            bool isInstalled = _upgradeManager.InstalledUpgrades.Contains(upgradeId);
            
            // Do not show list item if prereqs are not met
            foreach (string prereq in upgrade.Prerequisites)
            {
                if (!_upgradeManager.InstalledUpgrades.Contains(prereq))
                {
                    prereqsMet = false;
                }
            }
            
            if (prereqsMet && !isInstalled)
            {
                // Create button
                GameObject clone = Instantiate(upgradeButton, Vector3.zero, Quaternion.identity);
    
                
                // If player can afford the upgrade, enable the button
                clone.GetComponent<Button>().interactable =
                    upgrade.Cost <= pointCount && !_upgradeManager.InstalledUpgrades.Contains(upgradeId);
    
                clone.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _upgradeManager.PurchaseUpgrade(upgrade);
                    // Re-render menu
                    DeletePauseMenuItems();
                    CreatePauseMenuItems();
                });
                    
                // Set button texts
                Text[] buttonTexts = clone.GetComponentsInChildren<Text>();
                buttonTexts[0].text = upgrade.Title;
                buttonTexts[1].text = upgrade.Description;
                buttonTexts[2].text = "Cost: " + upgrade.Cost + " bubbles";
    
                // Set object as the child of availableUpgradeView
                clone.transform.SetParent(availableUpgradeView.transform, false);
            }

            if (prereqsMet && isInstalled)
            {
                // Create button
                GameObject clone = Instantiate(installedUpgradeLabel, Vector3.zero, Quaternion.identity);

                // Set button texts
                Text[] buttonTexts = clone.GetComponentsInChildren<Text>();
                buttonTexts[0].text = upgrade.Title;
    
                // Set object as the child of installedUpgradeView
                clone.transform.SetParent(installedUpgradeView.transform, false);
            }
            
        }
        
    }

    public void DeletePauseMenuItems()
    {
        foreach (Transform child in availableUpgradeView.transform) {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in installedUpgradeView.transform) {
            Destroy(child.gameObject);
        }
    }

    public void AddPoints(int points)
    {
        pointCount += points;
        pointLabel.text = pointCount.ToString();
    }
    
    public void RemovePoints(int points)
    {
        if (pointCount <= 0 && _upgradeManager.InstalledUpgrades.Count == 0)
        {
            return;
        };

        if (pointCount <= 0)
        {
            _upgradeManager.DeleteUpgrade();
        }
        
        pointCount -= points;

        if (pointCount < 0)
        {
            pointCount = 0;
        }
        pointLabel.text = pointCount.ToString();
    }
}
