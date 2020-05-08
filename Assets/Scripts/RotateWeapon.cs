using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    private GameObject _playerObject;
    
    // Start is called before the first frame update
    void Awake()
    {
        _playerObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
        // Point weapon towards player
        Vector3 targ = _playerObject.transform.position;
        targ.z = 0f;
 
        Vector3 objectPos = transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;
 
        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
