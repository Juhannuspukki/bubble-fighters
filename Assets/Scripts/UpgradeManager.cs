using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShipUpgrade
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

public class UpgradeManager : MonoBehaviour
{
    private PlayerShipBehavior _playerShipBehavior;
    private GameEventHandler _gameEventHandler;

    public readonly List<string> InstalledUpgrades = new List<string>();
    public readonly List<ShipUpgrade> UpgradeData = new List<ShipUpgrade>{
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
        new ShipUpgrade(
            "defense",
            "defense_1", 
            "Evasion I", 
            "5% chance to not take damage when you get hit.", 
            10,
            new List<string>()),
        new ShipUpgrade(
            "defense",
            "defense_2", 
            "Evasion II", 
            "25% chance to not take damage when you get hit.", 
            10,
            new List<string>{"defense_1"}),
    };

    private void Awake()
    {
        _playerShipBehavior = FindObjectOfType<PlayerShipBehavior>();
        _gameEventHandler = FindObjectOfType<GameEventHandler>();
    }

    public void PurchaseUpgrade(ShipUpgrade upgrade)
    {
        switch (upgrade.Modifier)
        {
            case "weapon":
                _playerShipBehavior.UpdateWeapon(upgrade.UpgradeId);
                break;
            
            case "projectile":
                _playerShipBehavior.UpdateProjectile(upgrade.UpgradeId);
                break;
            
            case "ship":
                _playerShipBehavior.UpdateShip(upgrade.UpgradeId);
                break;
            
            case "defense":
                _playerShipBehavior.UpdateShip(upgrade.UpgradeId);
                break;
        }
        
        // Add upgrade to list of installed upgrades and remove bubbles
        InstalledUpgrades.Add(upgrade.UpgradeId);
        _gameEventHandler.RemovePoints(upgrade.Cost);
        
        // Re-render menu
        _gameEventHandler.DeletePauseMenuItems();
        _gameEventHandler.CreatePauseMenuItems();
    }
    
    public void DeleteUpgrade()
    {
        // Delete item
        string latestUpgradeId = InstalledUpgrades[InstalledUpgrades.Count - 1];
        ShipUpgrade latestUpgradedItem = UpgradeData.Find(item => item.UpgradeId == latestUpgradeId);

        // Find the item that is one tier worse than what the player has
        string[] splittedString = latestUpgradeId.Split('_');
        string modifier = splittedString[0];

        string downgradeToItemId = modifier + "_" + (Int32.Parse(splittedString[1]) - 1);

        // Replace the item with the one that is one tier worse
        switch (modifier)
        {
            case "weapon":
                _playerShipBehavior.UpdateWeapon(downgradeToItemId);
                break;
            
            case "projectile":
                _playerShipBehavior.UpdateProjectile(downgradeToItemId);
                break;
            
            case "ship":
                _playerShipBehavior.UpdateShip(downgradeToItemId);
                break;
        }
        
        // Refund item
        _gameEventHandler.AddPoints(latestUpgradedItem.Cost);
        
        // Remove it from installed upgrades
        InstalledUpgrades.RemoveAt(InstalledUpgrades.Count - 1);
    }
}
