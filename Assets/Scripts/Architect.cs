using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Architect : MonoBehaviour
{
    public GameObject bubble;
    
    private readonly List<Vector3> _bubbleLocations = new List<Vector3>();
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateWorld();
    }
    
    private void GenerateWorld()
    {
        for (int y = -25; y <= 25; y += 25)
        {
            for (int x = -25; x <= 25; x += 25)
            {
                Vector3 newBubbleLocation = new Vector3(x, y, 0);
                _bubbleLocations.Add(newBubbleLocation);
                Instantiate(bubble, newBubbleLocation, Quaternion.identity);
            }
        }
    }

    public void GenerateSurroundingBubbles(Vector3 initialBubbleCoords)
    {
        
        // Detemine where the center of the closest circle is
        float closestCenterX = Convert.ToSingle(Math.Floor((initialBubbleCoords.x + 12.5) / 25) * 25f);
        float closestCenterY = Convert.ToSingle(Math.Floor((initialBubbleCoords.y + 12.5) / 25) * 25f);

        // Add surrounding bubble coords to a list
        Vector3[] closestBubbleLocations =
        {
            new Vector3(closestCenterX + 25f, closestCenterY, 0),
            new Vector3(closestCenterX - 25f, closestCenterY, 0),
            new Vector3(closestCenterX, closestCenterY + 25f, 0),
            new Vector3(closestCenterX, closestCenterY - 25f, 0)
        };
        
        // If bubbles don't exist at these locations, create them
        foreach (Vector3 location in closestBubbleLocations)
        {
            if (!_bubbleLocations.Contains(location))
            {
                Instantiate(bubble, location, Quaternion.identity);
                _bubbleLocations.Add(location);
            }
        }
    }
}
