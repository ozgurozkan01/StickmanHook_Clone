using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicShadow : Shadow
{
    [SerializeField] private bool isDynamic;
    [SerializeField] private float maxOffset;
    [SerializeField] private CameraController cameraController;

    private void LateUpdate()
    {
        UpdateDynamicShadow();
    }

    private void UpdateDynamicShadow()
    {
        if (isDynamic)
        {
            shadowRenderer.sprite = spriteRenderer.sprite;
            shadowRenderer.flipX = spriteRenderer.flipX;

            float offset = 0.1f;
            
            if (cameraController.maxOffset != 0)
            {
                offset = (cameraController.offset / cameraController.maxOffset) * maxOffset * 100;
                offset /= 100;
            }

            shadowObject.transform.position = gameObject.transform.position + new Vector3(shadowOffset.x,
                shadowOffset.y) + new Vector3(-offset, 0f, 0f);
        }
    }
}
