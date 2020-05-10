using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Architect : MonoBehaviour
{
    public GameObject bubble;
    private GenericFunctions _genericFunctions;
    private readonly List<Vector3> _bubbleLocations = new List<Vector3>();
    private readonly List<Vector3> _visitedBubbleLocations = new List<Vector3>();
    private readonly List<Vector3> _conqueredBubbleLocations = new List<Vector3>();
    
    // Start is called before the first frame update
    private void Awake()
    {
        _genericFunctions = FindObjectOfType<GenericFunctions>();
        
        _conqueredBubbleLocations.Add(Vector3.zero);
        _visitedBubbleLocations.Add(Vector3.zero);
        
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

    public void ConquerBubble(Vector3 bubbleCoordinates)
    {
        _conqueredBubbleLocations.Add(bubbleCoordinates);
    }

    public void VisitBubble(Vector3 bubbleCoordinates)
    {
        if (_visitedBubbleLocations.Contains(bubbleCoordinates)) return;

        _visitedBubbleLocations.Add(bubbleCoordinates);

    }
    
    public Vector3 GetClosestConqueredBubble(Vector3 shipCoordinates)
    {
        // Initialize with infinite distance
        float smallestDistance = Mathf.Infinity;
        Vector3 closestConqueredBubble = Vector3.zero;
   
        // Iterate through all conquered bubbles and find the closest one
        foreach (Vector3 bubbleLocation in _conqueredBubbleLocations)
        {
            float currentDistance = Vector3.Distance(bubbleLocation, shipCoordinates);
            
            if (currentDistance < smallestDistance)
            {
                closestConqueredBubble = bubbleLocation;
                smallestDistance = currentDistance; 
            } 
        }

        return closestConqueredBubble;
    }
    
    public List<Vector3> SimpleBubbleCoordinates()
    {
        List<Vector3> simplifiedCoordsList = new List<Vector3>();

        foreach (var visitedBubble in _visitedBubbleLocations)
        {
            Vector3 newCoords = new Vector3((visitedBubble.x / 28) * 10, (visitedBubble.y / 28) * 10, 0);
            simplifiedCoordsList.Add(newCoords);
        }

        return simplifiedCoordsList;
    }
}
