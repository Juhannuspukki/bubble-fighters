using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour
{
    public GameObject[] neutralPool;
    public GameObject[] hostilePool;
    
    private PlayerShipBehavior _playerShip;
    private float _distanceFromCenter;
    private readonly List<GameObject> _selectedEnemies = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only set active if it is the player entering the bubble
        if (other.CompareTag("Player"))
        {
            // Activate Npc's within this bubble
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        _playerShip = FindObjectOfType<PlayerShipBehavior>();
        _distanceFromCenter = Vector3.Distance(transform.position, Vector3.zero);

        DetermineEnemyClasses();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        Vector3 position = transform.position;
        
        // Spawn enemies if player is close enough
        // But not on the starting bubble
        if (position == Vector3.zero) return;


        // Determine the direction in which player entered the bubble
        // Purpose: spawn enemies on the side of the bubble the player does not see
        float xModifier = 0;
        float yModifier = 0;
        if (_playerShip.previousShipLocation.x == _playerShip.shipLocation.x)
        {
            yModifier = _playerShip.previousShipLocation.y < _playerShip.shipLocation.y ? 5 :-5;
        }
        else if (_playerShip.previousShipLocation.y == _playerShip.shipLocation.y)
        {
            xModifier = _playerShip.previousShipLocation.x < _playerShip.shipLocation.x ? 5 :-5;
        }
            
        foreach (GameObject enemyShip in _selectedEnemies)
        {
            // Randomize spawn position within bubble
            float randomDistanceX = position.x + xModifier + UnityEngine.Random.Range(-5, 5);
            float randomDistanceY = position.y + yModifier + UnityEngine.Random.Range(-5, 5);
            
            Quaternion randomRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0.0f, 360.0f));
            
            Vector3 spawnPosition = new Vector3(randomDistanceX, randomDistanceY, 0);
                
            GameObject clone = Instantiate(enemyShip, spawnPosition, randomRotation);
            clone.transform.parent = gameObject.transform;
            clone.gameObject.SetActive(false);
        }

    }

    private void DetermineEnemyClasses()
    {

        if (_distanceFromCenter < 50)
        {
            int[] neutralChances = { 50, 50, 15, 0, 0, 50, 100, 0 };
            int maxNeutrals = UnityEngine.Random.Range(4, 6);
            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
        
        else if (_distanceFromCenter < 90)
        {
            int[] enemyChances = { 100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int maxEnemies = UnityEngine.Random.Range(1, 3);
            
            FormEnemyPool(enemyChances, maxEnemies, hostilePool);
            
            int[] neutralChances = { 25, 25, 25, 0, 0, 0, 90, 0 };
            int maxNeutrals = UnityEngine.Random.Range(3, 5);

            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
        
        else if (_distanceFromCenter < 120)
        {
            int[] enemyChances = { 100, 100, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int maxEnemies = UnityEngine.Random.Range(4, 6);
            
            FormEnemyPool(enemyChances, maxEnemies, hostilePool);
            
            int[] neutralChances = { 20, 25, 25, 0, 0, 20, 10, 0 };
            int maxNeutrals = UnityEngine.Random.Range(1, 3);

            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
        
        else if (_distanceFromCenter < 160)
        {
            int[] enemyChances = { 0, 0, 25, 25, 33, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int maxEnemies = UnityEngine.Random.Range(4, 6);
            
            FormEnemyPool(enemyChances, maxEnemies, hostilePool);
            
            int[] neutralChances = { 50, 10, 50, 0, 0, 0, 0, 0 };
            int maxNeutrals = UnityEngine.Random.Range(2, 4);

            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
        
        else if (_distanceFromCenter < 200)
        {
            int[] enemyChances = { 5, 0, 0, 0, 0, 50, 30, 70, 10, 5, 20, 0, 0, 0, 0, 0, 0, 0, 0 };
            int maxEnemies = UnityEngine.Random.Range(4, 6);
            
            FormEnemyPool(enemyChances, maxEnemies, hostilePool);
            
            int[] neutralChances = { 50, 0, 0, 50, 0, 0, 0, 0 };
            int maxNeutrals = UnityEngine.Random.Range(2, 4);

            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
        else
        {
            int[] enemyChances = { 5, 5, 5, 5, 5, 5, 5, 5, 5, 10, 5, 10, 5, 20, 20, 10, 10, 10, 2 };
            int maxEnemies = UnityEngine.Random.Range(4, 6);
            
            FormEnemyPool(enemyChances, maxEnemies, hostilePool);
            
            int[] neutralChances = { 10, 10, 50, 20, 10, 0, 0, 5 };
            int maxNeutrals = UnityEngine.Random.Range(2, 4);

            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
    }

    private void FormEnemyPool(int[] probabilities, int maxShips, GameObject[] pool)
    {
        int spawnedCount = 0;
        while (true)
        {
            
            for(int i = 0; i < probabilities.Length; i ++)
            {
                if (UnityEngine.Random.Range(100, 0) < probabilities[i])
                {
                    _selectedEnemies.Add(pool[i]);
                    spawnedCount++;

                }
                if (spawnedCount >= maxShips)
                {
                    return;
                } 
            }
        }
    }
}
