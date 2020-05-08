using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    public bool defaultCollectible = true;

    private GameEventHandler _eventHandler;
    private GameObject _playerObject;
    
    private void Awake()
    {
        _eventHandler = FindObjectOfType<GameEventHandler>();
        _playerObject = GameObject.FindWithTag("Player");
    }
    
    private void Start()
    {
        // Move to random direction after spawning
        Vector3 impulse = new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), 0);
        GetComponent<Rigidbody2D>().AddForce(impulse, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        // Move towards the player
        Vector3 position = transform.position;

        if (Vector3.Distance(position, _playerObject.transform.position) < 2f)
        {
            position = Vector2.MoveTowards(position, _playerObject.transform.position, 10f * Time.deltaTime);
            transform.position = position;
        }
    }

    // Collect items and increase score
    private void OnTriggerEnter2D(Collider2D hit)
    {
        // Ignore hits by non-players
        if (!hit.CompareTag("Player")) return;
        
        Destroy(gameObject);
        
        // If this collectible is of default type, add a bubble
        if (defaultCollectible)
        {
            _eventHandler.AddPoint();
        }
        else
        {
            _eventHandler.AddPoint();
            _eventHandler.AddPoint();
            _eventHandler.AddPoint();
            _eventHandler.AddPoint();
            _eventHandler.AddPoint();
        }
    }
}
