using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Architect : MonoBehaviour
{
    public GameObject bubble;
    private readonly List<Vector3> _visitedBubbleLocations = new List<Vector3>();
    private readonly List<Vector3> _conqueredBubbleLocations = new List<Vector3>();
    
    // Start is called before the first frame update
    private void Awake()
    {
        // Add the starting bubble
        _conqueredBubbleLocations.Add(Vector3.zero);
        _visitedBubbleLocations.Add(Vector3.zero);
    }

    public void GenerateWorld(Vector3 location)
    {
        // If there is a bubble here, do not make a new one
        if (_visitedBubbleLocations.Contains(location)) return;
        
        Instantiate(bubble, location, Quaternion.identity);
        _visitedBubbleLocations.Add(location);
    }

    public void ConquerBubble(Vector3 bubbleCoordinates)
    {
        _conqueredBubbleLocations.Add(bubbleCoordinates);
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
