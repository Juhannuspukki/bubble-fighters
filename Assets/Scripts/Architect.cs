using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Architect : MonoBehaviour
{
    public GameObject bubble;
    private GenericFunctions _genericFunctions;
    private readonly List<Vector3> _bubbleLocations = new List<Vector3>();
    
    // Start is called before the first frame update
    private void Awake()
    {
        _genericFunctions = FindObjectOfType<GenericFunctions>();
        
        GenerateWorld();
    }
    
    private void GenerateWorld()
    {
        for (int y = -28; y <= 28; y += 28)
        {
            for (int x = -28; x <= 28; x += 28)
            {
                Vector3 newBubbleLocation = new Vector3(x, y, 0);
                _bubbleLocations.Add(newBubbleLocation);
                Instantiate(bubble, newBubbleLocation, Quaternion.identity);
            }
        }
    }

    public void GenerateSurroundingBubbles(Vector3 initialBubbleCoords)
    {

        Vector3 closestCenter = _genericFunctions.GetClosestCircle(initialBubbleCoords);

        // Add surrounding bubble coords to a list
        Vector3[] closestBubbleLocations =
        {
            new Vector3(closestCenter.x + 28f, closestCenter.y, 0),
            new Vector3(closestCenter.x - 28f, closestCenter.y, 0),
            new Vector3(closestCenter.x, closestCenter.y + 28f, 0),
            new Vector3(closestCenter.x, closestCenter.y - 28f, 0)
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
