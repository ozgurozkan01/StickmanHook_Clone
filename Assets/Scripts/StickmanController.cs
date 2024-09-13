using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanController : MonoBehaviour
{
    [Header("Stickman Sprites")] 
    [SerializeField] private Sprite ballSprite;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite startSwingSprite;
    [SerializeField] private Sprite endSwingSprite;
    [SerializeField] private Sprite swingingSprite;

    [Header("Stickman Components")] 
    private HingeJoint2D hJoint;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private SpriteRenderer spriteRenderer;

    [Header("Anchor")] 
    [SerializeField] private GameObject anchor;

    [Header("Helper Variables - PRIVATE")] 
    private int lastBestOptionJoint;
    private int lastBestOptionSelected;
    private int bestOptionIndex;
    private float bestDistance;
    private Vector3 actualJointPosition;
    
    [Header("Helper Variables - PUBLIC")]
    [SerializeField] private float gravityRope = 1.3f;
    [SerializeField] private float gravityAir = .3f;
    [SerializeField] private float factorX = 1.2f;
    [SerializeField] private float factorY = 1f;

    [Header("Booleans")] 
    private bool isSticked;
    private bool isWon;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Update()
    {
        FindBestPositionAndDistance();
        if (!isWon)
        {
            Input();
        }
        UpdateAnchorSelection();
    }

    void Init()
    {
        hJoint = GetComponent<HingeJoint2D>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        lastBestOptionJoint = 0;
        lastBestOptionSelected = 0;
        
        anchor.transform.GetChild(lastBestOptionSelected).gameObject.GetComponent<JointController>().Selected();        
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void Input()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0)) { Stick();   }
        if (UnityEngine.Input.GetMouseButtonUp(0))   { Unstick(); }
    }

    private void Stick()
    {
        lineRenderer.enabled = true;
        hJoint.enabled = true;
        rb.gravityScale = gravityRope;
            
        // Connect the joing rb to the hinge
        hJoint.connectedBody = anchor.transform.GetChild(bestOptionIndex).transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>();
        actualJointPosition = anchor.transform.GetChild(bestOptionIndex).transform.position;
        anchor.transform.GetChild(bestOptionIndex).gameObject.GetComponent<JointController>().SetSticked();
        anchor.transform.GetChild(bestOptionIndex).gameObject.GetComponent<JointController>().Unselected();

        lastBestOptionJoint = bestOptionIndex;
        rb.angularVelocity = 0;
        isSticked = !isSticked;
    }

    private void Unstick()
    {
        lineRenderer.enabled = false;
        hJoint.enabled = false;
        rb.velocity = new Vector2(rb.velocity.x * factorX, rb.velocity.y * factorY);
        rb.gravityScale = gravityAir;
            
        anchor.transform.GetChild(lastBestOptionJoint).gameObject.GetComponent<JointController>().SetUnsticked();

        if (bestOptionIndex == lastBestOptionJoint)
        {
            anchor.transform.GetChild(bestOptionIndex).gameObject.GetComponent<JointController>().Selected();
            anchor.transform.GetChild(lastBestOptionSelected).gameObject.GetComponent<JointController>().Unselected();
        }

        spriteRenderer.sprite = ballSprite;
        rb.AddTorque(-rb.velocity.magnitude);
        isSticked = !isSticked;
    }

    private void FindBestPositionAndDistance()
    {
        bestOptionIndex = 0;
        bestDistance = float.MaxValue;
        
        for (int i = 0; i < anchor.transform.childCount; i++)
        {
            float actualDistance = Vector2.Distance(gameObject.transform.position, anchor.transform.GetChild(i).transform.position);

            if (actualDistance < bestDistance)
            {
                bestOptionIndex = i;
                bestDistance = actualDistance;
            }
        }
    }

    private void UpdateAnchorSelection()
    {
        if (isSticked)
        {
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, actualJointPosition);
            
            UpdateSprite();
        }

        if (lastBestOptionSelected != bestOptionIndex)
        {
            anchor.transform.GetChild(lastBestOptionSelected).gameObject.GetComponent<JointController>().Unselected();
            anchor.transform.GetChild(bestOptionIndex).gameObject.GetComponent<JointController>().Selected();
        }
        
        lastBestOptionSelected = bestOptionIndex;
    }
    
    private void UpdateSprite()
    {
        if(rb.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else // for left
        {
            spriteRenderer.flipX = true;
        }

        if(rb.velocity.x < 0.7f && rb.velocity.x > -0.7f && gameObject.transform.position.y < actualJointPosition.y)
        {
            spriteRenderer.sprite = swingingSprite;
        }
        else
        {
            if(rb.velocity.y < 0)
            {
                spriteRenderer.sprite = startSwingSprite;
            }
            else
            {
                spriteRenderer.sprite = endSwingSprite;
            }
        }

        gameObject.transform.eulerAngles = LookAt2D(actualJointPosition - gameObject.transform.position);
    }
    
    public Vector3 LookAt2D (Vector3 vec)
    {
        return new Vector3 (gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, Vector2.SignedAngle(Vector2.up, vec));
    }

    public bool IsSticked()
    {
        return isSticked;
    }
    
    public void Reset (Vector3 initPos)
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        gameObject.transform.position = initPos;
        gameObject.transform.rotation = new Quaternion (0f, 0f, 0f, 0f);
    }

    // called in game manager
    public void Win(float speedWin)
    {
        isWon = true;
        spriteRenderer.flipX = false;
        rb.gravityScale = 0;
        gameObject.transform.eulerAngles = LookAt2D(rb.velocity);
        rb.velocity = rb.velocity.normalized * speedWin;
        rb.angularVelocity = 0f;
        spriteRenderer.sprite = winSprite;
    }
}
