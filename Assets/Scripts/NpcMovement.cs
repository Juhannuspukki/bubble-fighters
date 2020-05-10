using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMovement : MonoBehaviour
{
    private const float RotSpeed = 1f;
    private float _rotationTimer = 0;
    private Quaternion _qTo;
    private Rigidbody2D _rb;
    
    private Vector3 _parentCenter;


    // Start is called before the first frame update
    void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _parentCenter = FindObjectOfType<GenericFunctions>().GetClosestCircle(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        _rotationTimer += Time.deltaTime;
        
        if (_rotationTimer >= 1f)
        {
            float distance = Vector3.Distance(_parentCenter, transform.position);

            if (distance > 9.5f )
            { 
                // Add force towards the center on edges
                Vector3 direction = _parentCenter - transform.position;
                _rb.AddForce(direction * 0.025f, ForceMode2D.Impulse);
                
                // Also rotate to face the center
                float angle = Mathf.Atan2(_parentCenter.y-transform.position.y, _parentCenter.x-transform.position.x)*180 / Mathf.PI;
                
                Vector3 rotation = new Vector3(0, 0, angle);
                _qTo = Quaternion.Euler(rotation);
                _rotationTimer = 0.0f;
            }
            else
            {
                // Rotate randomly
                Vector3 rotation = new Vector3(0, 0, Random.Range(-180f, 180f));
                _qTo = Quaternion.Euler(rotation);
                _rotationTimer = 0.0f;
            }
        }
        
        _rb.transform.rotation = Quaternion.Slerp(transform.rotation, _qTo, Time.deltaTime * RotSpeed);

    }
    
    void FixedUpdate()
    {
        // Move forward
        _rb.transform.position += transform.right * (Time.deltaTime * 1.5f);
    }

}
