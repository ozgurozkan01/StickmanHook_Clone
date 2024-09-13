using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    [SerializeField] private Material shadowMaterial;
    public Vector2 shadowOffset;

    protected SpriteRenderer spriteRenderer;
    protected GameObject shadowObject;
    protected SpriteRenderer shadowRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        spriteRenderer = GetComponent < SpriteRenderer>();
        shadowObject = new GameObject("Shadow2D");

        shadowRenderer = shadowObject.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = spriteRenderer.sprite;

        shadowRenderer.material = shadowMaterial;
        shadowRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        shadowRenderer.sortingOrder = spriteRenderer.sortingOrder;

        shadowObject.transform.parent = gameObject.transform;
        shadowObject.transform.localScale = Vector3.one;
        shadowObject.transform.rotation = gameObject.transform.rotation;
        shadowObject.transform.position = gameObject.transform.position + (Vector3)shadowOffset;
        
    }
}
