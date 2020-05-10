using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public GameObject mapBubble;

    private Architect _architect;
    private void Awake()
    {
        _architect = FindObjectOfType<Architect>();
    }

    public void UpdateMap(Vector3 shipLocation)
    {
        Vector3 simpleShipLocation = new Vector3((shipLocation.x / 28) * 10, (shipLocation.y / 28) * 10, 0);
            
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        
        List<Vector3> mapcoords = _architect.SimpleBubbleCoordinates();
        foreach (var coord in  mapcoords)
        {
            Vector3 centeredCoord = new Vector3(coord.x - simpleShipLocation.x, coord.y - simpleShipLocation.y, 0);
            GameObject clone = Instantiate(mapBubble, centeredCoord, Quaternion.identity);
            clone.transform.SetParent(transform, false);
        }
    }
}
