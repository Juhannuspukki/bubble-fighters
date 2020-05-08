using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

struct ShipUpgrade
{
    public readonly string Modifier;
    public readonly string UpgradeId;
    public readonly string Title;
    public readonly string Description;
    public readonly int Cost;
    public readonly List<string> Prerequisites;

    public ShipUpgrade(
        string modifier,
        string upgradeId,
        string title,
        string description,
        int cost,
        List<string> prerequisites
    )
    {
        Modifier = modifier;
        UpgradeId = upgradeId;
        Title = title;
        Description = description;
        Cost = cost;
        Prerequisites = prerequisites;
    }
}

public class GameEventHandler : MonoBehaviour
{
    public Text pointLabel;
    public List<string> unlockedUpgrades;
    public GameObject pauseMenu;
    public GameObject upgradeButton;
    public GameObject installedUpgradeLabel;
    public GameObject availableUpgradeView;
    public GameObject installedUpgradeView;
    public GameObject shipController;
    public AudioSource npcGetHit;
    public AudioSource playerGetHit;
    public AudioSource playerWeapon;
    public AudioSource explosion;

    private readonly List<string> _installedUpgrades = new List<string>();
    private int _pointCount = 0;
    private bool _isPaused = false;

    private readonly List<ShipUpgrade> _upgradeData = new List<ShipUpgrade>{
        new ShipUpgrade(
            "weapon",
            "weapon_1", 
            "Excalibur turret", 
            "Improves weapon rate of fire.", 
            10, 
            new List<string>()),
        new ShipUpgrade(
            "weapon",
            "weapon_2", 
            "Adamant turret", 
            "Double turret with medium rate of fire.", 
            10,
            new List<string>{"weapon_1"}),
        new ShipUpgrade(
            "weapon",
            "weapon_3", 
            "BlueFyre turret", 
            "Double turret with awe-inspiring rate of fire!", 
            10,
            new List<string>{"weapon_1", "weapon_2"}),
        new ShipUpgrade(
            "projectile",
            "projectile_1", 
            "RowanBerry projectiles", 
            "Improves projectile damage and speed.", 
            10, 
            new List<string>()),
        new ShipUpgrade(
            "projectile",
            "projectile_2", 
            "Exocet projectiles", 
            "Improves projectile damage and speed.", 
            10,
            new List<string>{"projectile_1"}),
        new ShipUpgrade(
            "projectile",
            "projectile_3", 
            "BlueFyre projectiles", 
            "Greatly improves projectile damage and speed. Neat!", 
            10,
            new List<string>{"projectile_1", "projectile_2"}),
        new ShipUpgrade(
            "ship",
            "ship_1", 
            "Bubble fighter", 
            "Faster and more maneuverable than a standard tank.", 
            10,
            new List<string>()),
        new ShipUpgrade(
            "ship",
            "ship_2", 
            "Advanced Fighter", 
            "Bubble fighter with improved handling.", 
            10,
            new List<string>{"ship_1"}),
        new ShipUpgrade(
            "ship",
            "ship_3", 
            "Talon fighter", 
            "The best bubble fighter bubbles can buy.", 
            10,
            new List<string>{"ship_1", "ship_2"}),
    };

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
    
    private void CreatePauseMenuItems()
    {
        // Create buttons from list of upgrades the player has unlocked
        foreach (string upgradeId in unlockedUpgrades)
        {

            ShipUpgrade upgrade = _upgradeData.Find(item => item.UpgradeId == upgradeId);

            bool prereqsMet = true;
            bool isInstalled = _installedUpgrades.Contains(upgradeId);
            
            // Do not show list item if prereqs are not met
            foreach (string prereq in upgrade.Prerequisites)
            {
                if (!_installedUpgrades.Contains(prereq))
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
                    upgrade.Cost <= _pointCount && !_installedUpgrades.Contains(upgradeId);
    
                clone.GetComponent<Button>().onClick.AddListener(() =>
                {
                    PurchaseUpgrade(upgrade);
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

    private void DeletePauseMenuItems()
    {
        foreach (Transform child in availableUpgradeView.transform) {
            Destroy(child.gameObject);
        }
        
        foreach (Transform child in installedUpgradeView.transform) {
            Destroy(child.gameObject);
        }
    }

    private void PurchaseUpgrade(ShipUpgrade upgrade)
    {
        switch (upgrade.Modifier)
        {
            case "weapon":
                shipController.GetComponent<PlayerShipBehavior>().UpdateWeapon(upgrade.UpgradeId);
                break;
            
            case "projectile":
                shipController.GetComponent<PlayerShipBehavior>().UpdateProjectile(upgrade.UpgradeId);
                break;
            
            case "ship":
                shipController.GetComponent<PlayerShipBehavior>().UpdateShip(upgrade.UpgradeId);
                break;
        }
        
        // Add upgrade to list of installed upgrades and remove bubbles
        _installedUpgrades.Add(upgrade.UpgradeId);
        _pointCount -= upgrade.Cost;
        pointLabel.text = _pointCount.ToString();
        
        // Re-render menu
        DeletePauseMenuItems();
        CreatePauseMenuItems();
    }
    
    public void AddPoint()
    {
        _pointCount++;
        pointLabel.text = _pointCount.ToString();
    }
    
    public void RemovePoint(int point)
    {
        if (_pointCount <= 0 && _installedUpgrades.Count == 0)
        {
            return;
        };

        if (_pointCount <= 0)
        {
            DeleteUpgrade();
        }
        
        _pointCount -= point;
        pointLabel.text = _pointCount.ToString();
    }

    private void DeleteUpgrade()
    {
        // Delete item
        string latestUpgradeId = _installedUpgrades[_installedUpgrades.Count - 1];
        ShipUpgrade latestUpgradedItem = _upgradeData.Find(item => item.UpgradeId == latestUpgradeId);

        // Find the item that is one tier worse than what the player has
        string[] splittedString = latestUpgradeId.Split('_');
        string modifier = splittedString[0];

        string downgradeToItemId = modifier + "_" + (Int32.Parse(splittedString[1]) - 1);

        // Replace the item with the one that is one tier worse
        switch (modifier)
        {
            case "weapon":
                shipController.GetComponent<PlayerShipBehavior>().UpdateWeapon(downgradeToItemId);
                break;
            
            case "projectile":
                shipController.GetComponent<PlayerShipBehavior>().UpdateProjectile(downgradeToItemId);
                break;
            
            case "ship":
                shipController.GetComponent<PlayerShipBehavior>().UpdateShip(downgradeToItemId);
                break;
        }
        
        // Refund item
        _pointCount += latestUpgradedItem.Cost;
        
        _installedUpgrades.RemoveAt(_installedUpgrades.Count - 1);
    }
}
