using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBackgroundBubbles : MonoBehaviour
{
    public GameObject backgroundBubble;
    public float positionX;

    private float _interval;
    private float _cooldownTimer = 0;

    void Awake()
    {
        // ReSharper disable once PossibleLossOfFraction
        _cooldownTimer =  Random.Range(5, 30) / 10;
        _interval = Random.Range(5, 30);
    }

    // Update is called once per frame
    void Update()
    {
        _cooldownTimer -= Time.deltaTime;
        
        if (_cooldownTimer <= 0)
        {
            _cooldownTimer = _interval / 10;
            GameObject clone = Instantiate(backgroundBubble, new Vector3(positionX, -8f , 0), Quaternion.identity);
        }
    }
}
