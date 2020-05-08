using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageParticleBehavior : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        // Move to random direction after spawning
        Vector3 impulse = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
        GetComponent<Rigidbody2D>().AddForce(impulse, ForceMode2D.Impulse);

        StartCoroutine(FadeTo(0.0f, 0.2f));
        
        // Destroy particle after a while
        Destroy (gameObject, 0.2f);
    }
    
    
    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = _spriteRenderer.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
            _spriteRenderer.color = newColor;
            yield return null;
        }
    }
}
