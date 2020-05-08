using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    private float _selfDestructTimer = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _selfDestructTimer -= Time.deltaTime;

        if (_selfDestructTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
