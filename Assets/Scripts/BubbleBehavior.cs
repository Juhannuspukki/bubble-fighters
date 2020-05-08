using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBehavior : MonoBehaviour
{
    public GameObject[] classOneHostiles;
    public GameObject[] classTwoHostiles;
    public GameObject[] classThreeHostiles;
    public GameObject[] classFourHostiles;
    public GameObject[] classFiveHostiles;
    public GameObject[] classSixHostiles;

    private Architect _architect;
    private bool _hasSpawnedEnemies = false;
    private float _distanceFromCenter;
    private GameObject[] _selectedEnemies;

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

        DetermineEnemyClass();
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        Vector3 position = transform.position;
        
        // Spawn enemies if player is close enough
        // But not on the starting bubble
        if (_hasSpawnedEnemies == false && position != Vector3.zero)
        {
            int numberOfBubbles;
            
            if (_distanceFromCenter < 50)
            {
                numberOfBubbles = UnityEngine.Random.Range(3, 6);
            }
            else
            {
                numberOfBubbles = UnityEngine.Random.Range(4, 6);
            }

            for (int i = 0; i < numberOfBubbles; i++)
            {
                // Randomize spawn position within bubble
                float randomDistanceX = position.x + UnityEngine.Random.Range(-5, 5);
                float randomDistanceY = position.y + UnityEngine.Random.Range(-5, 5);
            
                Vector3 spawnPosition = new Vector3(randomDistanceX, randomDistanceY, 0);
                
                GameObject randomSprite = _selectedEnemies[UnityEngine.Random.Range(0, _selectedEnemies.Length)];
                
                GameObject clone = Instantiate(randomSprite, spawnPosition, Quaternion.identity);
                clone.transform.parent = gameObject.transform;
                clone.gameObject.SetActive(false);
            }

            _hasSpawnedEnemies = true;
        }
    }

    private void DetermineEnemyClass()
    {

        if (_distanceFromCenter < 50)
        {
            _selectedEnemies = classOneHostiles;
        }
        else if (_distanceFromCenter < 100)
        {
            _selectedEnemies = classTwoHostiles;
        }
        else if (_distanceFromCenter < 150)
        {
            _selectedEnemies = classThreeHostiles;
        }
        else if (_distanceFromCenter < 200)
        {
            _selectedEnemies =  classFourHostiles;
        }
        else if (_distanceFromCenter < 250)
        {
            _selectedEnemies = classFiveHostiles;
        }
        else
        {
            _selectedEnemies = classSixHostiles;
        }
    }
}
