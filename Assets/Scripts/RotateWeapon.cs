using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    private PlayerShipBehavior _playerShip;
    private Vector3 _parentCenter;
    
    // Start is called before the first frame update
    void Awake()
    {
        _playerShip = FindObjectOfType<PlayerShipBehavior>();

        _parentCenter = FindObjectOfType<GenericFunctions>().GetClosestCircle(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        // If in different bubble or out of range do nothing
        if (_playerShip.shipLocation != _parentCenter) return;
        if (!_playerShip.shipIsWithinEngagementRange) return;
        
        // Point weapon towards player
        Vector3 targ = _playerShip.transform.position;
        targ.z = 0f;
 
        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;
 
        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
