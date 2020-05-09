using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private GameEventHandler _eventHandler;
    private PlayerUpgradeManager _upgradeManager;

    public GameObject damageParticle;


    private void Awake()
    {
        _eventHandler = FindObjectOfType<GameEventHandler>();
        _upgradeManager = FindObjectOfType<PlayerUpgradeManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only take damage from Npc weapons
        if (!other.CompareTag("NpcProjectile")) return;
        
        // Take damage (accounting defenses)
        float randomNumber = Random.Range(1.0f, 0.0f);
        
        if (randomNumber < _upgradeManager.activeDamageChance)
        {
            _eventHandler.RemovePoints(other.GetComponent<ProjectileBehavior>().damage);
        }

        // Destroy incoming projectile and play animations
        Destroy(other.gameObject);
            
        _eventHandler.playerGetHit.Play();
        SpawnDamageParticles(other.gameObject.transform.position);
    }
    
    void SpawnDamageParticles(Vector3 position)
    {
        int numberOfParticles = Random.Range(4, 6);
        
        // Spawn collectible bubbles
        for (int i = 0; i < numberOfParticles; i++) 
        {
            Instantiate(damageParticle, position, transform.rotation);
        }
    }

}
