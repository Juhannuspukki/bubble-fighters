using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour
{
    public GameObject[] neutralPool;
    public GameObject[] hostilePool;
    
    private Architect _architect;
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

            // Spawn new bubbles surrounding this one
            _architect.GenerateSurroundingBubbles(transform.position);
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
        _architect = FindObjectOfType<Architect>();
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
        
        
        foreach (GameObject enemyShip in _selectedEnemies)
        {
            // Randomize spawn position within bubble
            float randomDistanceX = position.x + UnityEngine.Random.Range(-5, 5);
            float randomDistanceY = position.y + UnityEngine.Random.Range(-5, 5);
            
            Vector3 spawnPosition = new Vector3(randomDistanceX, randomDistanceY, 0);
                
            GameObject clone = Instantiate(enemyShip, spawnPosition, Quaternion.identity);
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
            int[] enemyChances = { 100, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int maxEnemies = UnityEngine.Random.Range(1, 3);
            
            FormEnemyPool(enemyChances, maxEnemies, hostilePool);
            
            int[] neutralChances = { 25, 25, 25, 0, 0, 0, 90, 0 };
            int maxNeutrals = UnityEngine.Random.Range(3, 5);

            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
        
        else if (_distanceFromCenter < 120)
        {
            int[] enemyChances = { 100, 100, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int maxEnemies = UnityEngine.Random.Range(4, 6);
            
            FormEnemyPool(enemyChances, maxEnemies, hostilePool);
            
            int[] neutralChances = { 20, 25, 25, 0, 0, 20, 10, 0 };
            int maxNeutrals = UnityEngine.Random.Range(1, 3);

            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
        
        else if (_distanceFromCenter < 160)
        {
            int[] enemyChances = { 0, 0, 25, 25, 33, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int maxEnemies = UnityEngine.Random.Range(4, 6);
            
            FormEnemyPool(enemyChances, maxEnemies, hostilePool);
            
            int[] neutralChances = { 50, 10, 50, 0, 0, 0, 0, 0 };
            int maxNeutrals = UnityEngine.Random.Range(2, 4);

            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
        
        else if (_distanceFromCenter < 200)
        {
            int[] enemyChances = { 5, 0, 0, 0, 0, 50, 30, 70, 10, 5, 20, 0, 0, 0, 0, 0, 0 };
            int maxEnemies = UnityEngine.Random.Range(4, 6);
            
            FormEnemyPool(enemyChances, maxEnemies, hostilePool);
            
            int[] neutralChances = { 50, 0, 0, 50, 0, 0, 0, 0 };
            int maxNeutrals = UnityEngine.Random.Range(2, 4);

            FormEnemyPool(neutralChances, maxNeutrals, neutralPool);
        }
        else
        {
            int[] enemyChances = { 5, 5, 5, 5, 5, 5, 5, 5, 5, 10, 0, 10, 5, 50, 50, 5, 10 };
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
