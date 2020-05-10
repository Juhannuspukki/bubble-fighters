using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public GameObject mapBubble;
    public GameObject originBubble;

    private Architect _architect;
    private void Start()
    {
        _architect = FindObjectOfType<Architect>();

        // Must be inside start so there is stuff in visitedBubbles
        UpdateMap(Vector3.zero);
    }

    public void UpdateMap(Vector3 shipLocation)
    {
        Vector3 simpleShipLocation = new Vector3((shipLocation.x / 28) * 10, (shipLocation.y / 28) * 10, 0);
            
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        
        List<Vector3> visitedBubbleCoordinates = _architect.SimpleBubbleCoordinates();
        foreach (Vector3 visitedBubbleCoordinate in visitedBubbleCoordinates)
        {
            Vector3 centeredCoord = new Vector3(visitedBubbleCoordinate.x - simpleShipLocation.x, visitedBubbleCoordinate.y - simpleShipLocation.y, 0);
            
            // Limit map size
            if (centeredCoord.x <= -40 || centeredCoord.x >= 40 || centeredCoord.y <= -40 ||centeredCoord.y >= 40) continue;

            // Use a special bubble for the starting position
            if (visitedBubbleCoordinate == Vector3.zero)
            {
                GameObject clone = Instantiate(originBubble, centeredCoord, Quaternion.identity);
                clone.transform.SetParent(transform, false);
            }
            else
            {
                GameObject clone = Instantiate(mapBubble, centeredCoord, Quaternion.identity);
                clone.transform.SetParent(transform, false);
            }
        }
    }
}
