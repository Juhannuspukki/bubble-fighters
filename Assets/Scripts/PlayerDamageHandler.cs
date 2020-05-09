using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private GameEventHandler _eventHandler;
    public GameObject damageParticle;


    private void Awake()
    {
        _eventHandler = FindObjectOfType<GameEventHandler>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only take damage from Npc weapons
        if (other.CompareTag("NpcProjectile"))
        {
            _eventHandler.RemovePoints(other.GetComponent<ProjectileBehavior>().damage);
            Destroy(other.gameObject);
            
            _eventHandler.playerGetHit.Play();
            SpawnDamageParticles(other.gameObject.transform.position);
        }
    }
    
    void SpawnDamageParticles(Vector3 position)
    {
        int numberOfParticles = Random.Range(5, 7);
        
        // Spawn collectible bubbles
        for (int i = 0; i < numberOfParticles; i++) 
        {
            Instantiate(damageParticle, position, transform.rotation);
        }
    }

}
