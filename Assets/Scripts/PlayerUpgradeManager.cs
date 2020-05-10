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

public class PlayerUpgradeManager : MonoBehaviour
{
    private PlayerShipBehavior _playerShipBehavior;
    private GameEventHandler _gameEventHandler;
    
    public GameObject[] shipModels;
    public GameObject[] weaponModels;
    public GameObject[] missiles;
    public GameObject[] projectiles;

    public float activeDamageChance = 1.0f;
    public int activeShipModel;
    public GameObject activeWeaponModel;
    public GameObject activeMissile;
    public GameObject activeProjectile;

    public readonly List<string> InstalledUpgrades = new List<string>();
    public readonly List<ShipUpgrade> UpgradeData = new List<ShipUpgrade>{
        new ShipUpgrade(
            "weapon",
            "weapon_1", 
            "Excalibur turret", 
            "Improves weapon rate of fire.", 
            20, 
            new List<string>()),
        new ShipUpgrade(
            "weapon",
            "weapon_2", 
            "Adamant turret", 
            "Double turret with medium rate of fire.", 
            40,
            new List<string>{"weapon_1"}),
        new ShipUpgrade(
            "weapon",
            "weapon_3", 
            "BlueFyre turret", 
            "Double turret with awe-inspiring rate of fire!", 
            200,
            new List<string>{"weapon_1", "weapon_2"}),
        new ShipUpgrade(
            "projectile",
            "projectile_1", 
            "RowanBerry projectiles", 
            "Improves projectile damage and speed.", 
            20, 
            new List<string>()),
        new ShipUpgrade(
            "projectile",
            "projectile_2", 
            "Exocet projectiles", 
            "Improves projectile damage and speed.", 
            60,
            new List<string>{"projectile_1"}),
        new ShipUpgrade(
            "projectile",
            "projectile_3", 
            "BlueFyre projectiles", 
            "Greatly improves projectile damage and speed. Neat!", 
            200,
            new List<string>{"projectile_1", "projectile_2"}),
        new ShipUpgrade(
            "ship",
            "ship_1", 
            "Bubble fighter", 
            "Faster and more maneuverable than the standard fighter.", 
            20,
            new List<string>()),
        new ShipUpgrade(
            "ship",
            "ship_2", 
            "Advanced Fighter", 
            "Bubble fighter with improved handling.", 
            100,
            new List<string>{"ship_1"}),
        new ShipUpgrade(
            "ship",
            "ship_3", 
            "Talon fighter", 
            "This fighter is even faster and can fire guided missiles.", 
            400,
            new List<string>{"ship_1", "ship_2"}),
        new ShipUpgrade(
            "ship",
            "ship_4", 
            "Sceptre fighter", 
            "The phantom menace.", 
            600,
            new List<string>{"ship_1", "ship_2", "ship_3"}),
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
            200,
            new List<string>{"defense_1"}),
    };

    private void Awake()
    {
        _playerShipBehavior = FindObjectOfType<PlayerShipBehavior>();
        _gameEventHandler = FindObjectOfType<GameEventHandler>();
        
        activeWeaponModel = weaponModels[0];
        activeProjectile = projectiles[0];
    }

    public void PurchaseUpgrade(ShipUpgrade upgrade)
    {
        
        int upgradeId = Int32.Parse(upgrade.UpgradeId.Split('_')[1]);
        
        switch (upgrade.Modifier)
        {
            case "weapon":
                UpdateWeapon(upgradeId);
                break;
            case "projectile":
                UpdateProjectile(upgradeId);
                break;
            case "ship":
                UpdateShip(upgradeId);
                break;
            case "defense":
                UpdateDefense(upgradeId);
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

        int downgradeToItemId = Int32.Parse(splittedString[1]) - 1;

        // Replace the item with the one that is one tier worse
        switch (modifier)
        {
            case "weapon":
                UpdateWeapon(downgradeToItemId);
                break;
            case "projectile":
                UpdateProjectile(downgradeToItemId);
                break;
            case "ship":
                UpdateShip(downgradeToItemId);
                break;
            case "defense":
                UpdateDefense(downgradeToItemId);
                break;
        }
        
        // Refund item
        _gameEventHandler.AddPoints(latestUpgradedItem.Cost);
        
        // Remove it from installed upgrades
        InstalledUpgrades.RemoveAt(InstalledUpgrades.Count - 1);
    }
    
    public void UpdateShip(int shipId)
    {
        // Destroy all ship and weapon models
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        List<Vector3> hardPoints = new List<Vector3>{Vector3.zero};
        
        // Adjust movementforce and weapon hardpoints of the ship object
        switch (shipId)
        {
            case 0:
                _playerShipBehavior.movementForce = 0.2f;
                hardPoints = new List<Vector3>{new Vector3(0, 0, 0)} ;
                break;
            case 1:
                _playerShipBehavior.movementForce = 0.35f;
                hardPoints = new List<Vector3>{new Vector3(-0.1f, 0, 0)} ;
                break;
            case 2:
                _playerShipBehavior.movementForce = 0.45f;
                hardPoints = new List<Vector3>{new Vector3(0.2f, 0, 0)} ;
                break;
            case 3:
                _playerShipBehavior.movementForce = 0.6f;
                hardPoints = new List<Vector3>{new Vector3(0.55f, 0, 0)} ;
                activeMissile = missiles[0];
                break;
            case 4:
                _playerShipBehavior.movementForce = 0.7f;
                hardPoints = new List<Vector3>{new Vector3(0, 0, 0)} ;
                activeMissile = missiles[1];
                break;
        }
        
        // Create new ship model
        GameObject ship = Instantiate(shipModels[shipId], transform.position , transform.rotation);
        ship.transform.parent = gameObject.transform;

        // Re-create weapons
        foreach (Vector3 hardpointLocation in hardPoints)
        {
            GameObject weapon = Instantiate(activeWeaponModel, transform.position, transform.rotation);
            weapon.transform.parent = gameObject.transform;
            weapon.transform.localPosition = hardpointLocation;
        }

        activeShipModel = shipId;
    }
    
    private void UpdateWeapon(int weaponId)
    {
        // Make weapon active
        activeWeaponModel = weaponModels[weaponId];
        
        // Let UpdateShip handle the placing of new weapons
        UpdateShip(activeShipModel);
    }

    private void UpdateProjectile(int projectileId)
    {
        // Make new projectile active
        activeProjectile = projectiles[projectileId];
    }
    
    private void UpdateDefense(int defenseId)
    {
        switch (defenseId)
        {
            case 0:
                activeDamageChance = 1.0f;
                break;
            case 1:
                activeDamageChance = 0.9f;
                break;
            case 2:
                activeDamageChance = 0.75f;
                break;
            
        }
    }
}
