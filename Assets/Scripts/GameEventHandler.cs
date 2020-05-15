using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameEventHandler : MonoBehaviour
{
    public Text pointLabel;
    public Text availableUpgradeCountLabel;
    public GameObject availableUpgradeCountIndicator;
    public GameObject[] nextPrevButtons;
    
    public Text availableUpgradeHeadline;
    public Image availableUpgradeImage;
    public Text availableUpgradeTitle;
    public Text availableUpgradeDescription;
    public Text availableUpgradeCost;

    public GameObject purchaseButton;

    public AudioSource npcGetHit;
    public AudioSource playerGetHit;
    public AudioSource playerWeapon;
    public AudioSource explosion;

    public Sprite unavailableSprite;
    public Sprite[] projectileSprites;
    public Sprite[] weaponSprites;
    public Sprite[] defenseSprites;
    public Sprite[] shipSprites;
    
    public GameObject[] itemsToHideOnPause;
    public GameObject[] itemsToRevealOnPause;
    public GameObject[] itemsToHideOnResume;
    public GameObject[] itemsToRevealOnResume;
    
    public int pointCount = 0;
    public bool isPaused = false;

    public List<string> unlockedUpgrades;
    public List<ShipUpgrade> availableUpgrades = new List<ShipUpgrade>();

    private int _currentUpgradeIndex = 0;
    private string _menuType = "available";
        
    private PlayerUpgradeManager _upgradeManager;
    private Architect _architect;
    private HideAndReveal _hideAndReveal;
    private Rigidbody2D _shipRigidBody;

    private void Awake()
    {
        _upgradeManager = FindObjectOfType<PlayerUpgradeManager>();
        _architect = FindObjectOfType<Architect>();
        _hideAndReveal = FindObjectOfType<HideAndReveal>();

        _shipRigidBody = _upgradeManager.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = isPaused ? 1 : 0;
        isPaused = !isPaused;
        
        // Create menu items when the game is paused
        if (isPaused)
        {
            _hideAndReveal.itemsToHide = itemsToHideOnPause;
            _hideAndReveal.itemsToReveal = itemsToRevealOnPause;
            _hideAndReveal.HideAndRevealItems();
        }
        
        // Delete menu items when the game is no longer paused
        else
        {
            _hideAndReveal.itemsToHide = itemsToHideOnResume;
            _hideAndReveal.itemsToReveal = itemsToRevealOnResume;
            _hideAndReveal.HideAndRevealItems();
            
            // Also stop ship to prevent funny movements
            _shipRigidBody.velocity = Vector3.zero;
            
        }
        
    }

    public void SetAvailableUpgradeData()
    {
        // Reset the available upgrade list
        availableUpgrades = new List<ShipUpgrade>();
        
        foreach (string upgradeId in unlockedUpgrades)
        {
            ShipUpgrade upgrade = _upgradeManager.UpgradeData.Find(item => item.UpgradeId == upgradeId);
            
            bool allPrerequisitiesMet = true;
            bool isInstalled = _upgradeManager.InstalledUpgrades.Contains(upgradeId);
            
            // Do not show list item if prereqs are not met
            foreach (string prerequisite in upgrade.Prerequisites)
            {
                if (!_upgradeManager.InstalledUpgrades.Contains(prerequisite))
                {
                    allPrerequisitiesMet = false;
                }
            }
            
            if (allPrerequisitiesMet && !isInstalled)
            {
                availableUpgrades.Add(upgrade);
            }
        }
        
        availableUpgradeCountIndicator.SetActive(availableUpgrades.Count > 0);
        availableUpgradeCountLabel.text = availableUpgrades.Count.ToString();
    }
    
    
    
    public void CreateUpgradeMenuItem()
    {
        // If _menuType happens to be "available"
        ShipUpgrade thisUpgrade = new ShipUpgrade();
        int count = 0;
        
        if (_menuType == "installed")
        {
            count = _upgradeManager.InstalledUpgrades.Count;
            ShowHideNextPrevButtons(count);
            
            if (count == 0)
            {
                ShowNothingAvailable();
                return;
            }
            
            purchaseButton.SetActive(false);
            string upgradeId = _upgradeManager.InstalledUpgrades[_currentUpgradeIndex];
            thisUpgrade = _upgradeManager.UpgradeData.Find(item => item.UpgradeId == upgradeId);
        }
        if (_menuType == "available")
        {
            count = availableUpgrades.Count;
            ShowHideNextPrevButtons(count);
            
            if (count == 0)
            {
                ShowNothingAvailable();
                return;
            }
            
            purchaseButton.SetActive(true);
            thisUpgrade = availableUpgrades[_currentUpgradeIndex];
        }

        purchaseButton.GetComponent<Button>().interactable = pointCount >= thisUpgrade.Cost;
        
        availableUpgradeTitle.text = thisUpgrade.Title;
        availableUpgradeDescription.text = thisUpgrade.Description;
        availableUpgradeCost.text = "Cost: " + thisUpgrade.Cost + " bubbles";
        
        int upgradeNumber = Int32.Parse(thisUpgrade.UpgradeId.Split('_')[1]);
        
        switch (thisUpgrade.Modifier)
        {
            case "ship":
                availableUpgradeImage.sprite = shipSprites[upgradeNumber];
                break;
            case "weapon":
                availableUpgradeImage.sprite = weaponSprites[upgradeNumber];
                break;
            case "projectile":
                availableUpgradeImage.sprite = projectileSprites[upgradeNumber];
                break;
            case "defense":
                availableUpgradeImage.sprite = defenseSprites[upgradeNumber];
                break;
        }
        
        availableUpgradeImage.SetNativeSize();
    }

    private void ShowNothingAvailable()
    {
        availableUpgradeTitle.text = _menuType == "available" ? "No upgrades available" : "No upgrades installed";
        availableUpgradeDescription.text = "";
        availableUpgradeCost.text = "";
        availableUpgradeImage.sprite = unavailableSprite;
        availableUpgradeImage.SetNativeSize();
        purchaseButton.SetActive(false);
    }
    
    public void SelectUpgradeMenu(string type)
    {
        _menuType = type;
        _currentUpgradeIndex = 0;
        SetAvailableUpgradeData();
        CreateUpgradeMenuItem();
        availableUpgradeHeadline.text = _menuType == "available" ? "Available Upgrades" : "Installed Upgrades";
    }

    public void ShowHideNextPrevButtons(int count)
    {
        // Hide Next/Prev buttons if there is only 1 or no items to browse
        if (count <= 1)
        {
            nextPrevButtons[0].SetActive(false);
            nextPrevButtons[1].SetActive(false);
        }
        else
        {
            nextPrevButtons[0].SetActive(true);
            nextPrevButtons[1].SetActive(true);
        }
    }
    
    public void PreviousUpgradeMenuItem()
    {
        int count = _menuType == "available" ? availableUpgrades.Count : _upgradeManager.InstalledUpgrades.Count;
        
        if (_currentUpgradeIndex == 0)
        {
            _currentUpgradeIndex = count - 1;
        }
        else
        {
            _currentUpgradeIndex--;
        }
        
        CreateUpgradeMenuItem();
    }
    
    public void NextUpgradeMenuItem()
    {
        int count = _menuType == "available" ? availableUpgrades.Count : _upgradeManager.InstalledUpgrades.Count;

        if (_currentUpgradeIndex == count - 1)
        {
            _currentUpgradeIndex = 0;
        }
        else
        {
            _currentUpgradeIndex++;
        }
        
        CreateUpgradeMenuItem();
    }

    public void PurchaseUpgrade()
    {
        _upgradeManager.InstallUpgrade(availableUpgrades[_currentUpgradeIndex]);
        SetAvailableUpgradeData();
        PreviousUpgradeMenuItem();
    }

    public void AddPoints(int points)
    {
        pointCount += points;
        pointLabel.text = pointCount.ToString();
    }
    
    public void RemovePoints(int points)
    {
        if (pointCount - points >= 0)
        {
            pointCount -= points;
            pointLabel.text = pointCount.ToString();
            return;
        }

        // Only executes if point count would be < 0 and there are some upgrades installed
        if (_upgradeManager.InstalledUpgrades.Count != 0)
        {
            _upgradeManager.DeleteUpgrade();
                
            SetAvailableUpgradeData();
            
            pointCount -= points;
            pointLabel.text = pointCount.ToString();
            return;
        }
        
        // Only executes if the point count would be < 0 and there are no upgrades installed
        // If player has run out of health, move to closest conquered bubble
        Vector3 closestConqueredBubble = _architect.GetClosestConqueredBubble(_upgradeManager.transform.position);
        StartCoroutine(MovePlayer(closestConqueredBubble));
        
        pointCount = 0;
        pointLabel.text = pointCount.ToString();
    }

    // Fire on "death"
    IEnumerator MovePlayer(Vector3 closestConqueredBubble)
    {
        while(true)
        {
            float distanceToTarget = Vector3.Distance(_upgradeManager.transform.position, closestConqueredBubble);
            _upgradeManager.transform.position = Vector3.MoveTowards(_upgradeManager.transform.position, closestConqueredBubble, 50f * Time.deltaTime);

            if (distanceToTarget < 1)
            {
                break;
            }
            
            yield return new WaitForFixedUpdate();
        }
    } 
}
