using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class JointController : MonoBehaviour
{
    [Header("Joint Sprites")] 
    [SerializeField] private Sprite stickedSprite;
    [SerializeField] private Sprite unstickedSprite;
    [SerializeField] private float animateTime = 0.1f;
    [SerializeField] private AnimationCurve animationCurve;

    private SpriteRenderer spriteRenderer;
    private GameObject dashLine;
    private bool isSticked = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dashLine = gameObject.transform.GetChild(1).gameObject;
    }
    
    public void SetSticked()
    {
        spriteRenderer.sprite = stickedSprite;
        isSticked = true;
    }
    
    public void SetUnsticked()
    {
        spriteRenderer.sprite = unstickedSprite;
        isSticked = false;
        Unselected();
    }

    public void Selected()
    {
        if (!isSticked)
        {
            StartCoroutine(SelectJoint());
        }
        else
        {
            dashLine.transform.localScale = Vector3.zero;
        }
    }

    public void Unselected()
    {
        StartCoroutine(SelectJoint());
        dashLine.transform.localScale = Vector3.zero;
    }

    IEnumerator SelectJoint()
    {
        float elapsedTime = 0f;
        
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = new Vector3(1.2f, 1.2f, 1.2f);

        while (elapsedTime <= animateTime)
        {
            elapsedTime += Time.deltaTime;
            dashLine.transform.localScale = Vector3.Lerp(startScale, endScale, animationCurve.Evaluate(animateTime));
            yield return null;
        }
    }
}
