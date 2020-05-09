using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFunctions : MonoBehaviour
{
    public Vector3 GetClosestCircle(Vector3 position)
    {
        // Detemine where the center of the closest circle is
        float closestCenterX = Convert.ToSingle(Math.Floor((position.x + 14) / 28) * 28f);
        float closestCenterY = Convert.ToSingle(Math.Floor((position.y + 14) / 28) * 28f);

        return new Vector3(closestCenterX, closestCenterY, 0);
    }
    
    public GameObject FindClosestEnemy(Vector3 position)
    {
        GameObject closestEnemy = null;
        
        // Find all game objects with tag Enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            return null;
        }

        // Initialize with infinite distance
        float smallestDistance = Mathf.Infinity;
   
        // Iterate through enemies and find the closest one
        foreach (GameObject enemy in enemies)
        {
            Vector3 enemyBubble = GetClosestCircle(enemy.transform.position);
            Vector3 positionBubble = GetClosestCircle(position);

            if (Vector3.Distance(positionBubble, enemyBubble) < 12f )
            {
                Vector3 diff = (enemy.transform.position - position); 
                float currentDistance = diff.sqrMagnitude; 
                if (currentDistance < smallestDistance) 
                { 
                    closestEnemy = enemy; 
                    smallestDistance = currentDistance; 
                } 
            }
        }

        return closestEnemy;
    }
}
