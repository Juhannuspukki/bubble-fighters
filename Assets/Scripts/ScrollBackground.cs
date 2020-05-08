using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Material mat = _meshRenderer.material;

        Vector3 offset = mat.mainTextureOffset;

        offset.x = transform.position.x / transform.localScale.x;
        offset.y = transform.position.y / transform.localScale.y;

        mat.mainTextureOffset = offset;
        
        
    }
}
