using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject stickman;
    [SerializeField] private float speed;

    private Rigidbody2D stickmanRb;
    
    [HideInInspector] public float offset;
    public float maxOffset;
    public float minX, maxX;
    public float winningOrthographicSize = 2.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Init()
    {
        offset = 0;
        stickmanRb = stickman.GetComponent<Rigidbody2D>();
        gameObject.transform.position = new Vector3(stickman.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }
    
    private void Move()
    {
        if (stickmanRb.velocity.x > 0)
        {
            offset += Time.deltaTime * speed;

            if (offset > maxOffset)
            {
                offset = maxOffset;
            }
        }
        
        else if (stickmanRb.velocity.x < 0)
        {
            offset -= Time.deltaTime * speed;

            if (offset < -maxOffset)
            {
                offset = -maxOffset;
            }
        }
        
        
        float nextX = stickman.transform.position.x + offset;
        if(nextX < minX) nextX = minX;
        if(nextX > maxX) nextX = maxX;

        gameObject.transform.position = new Vector3 (nextX, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    public void Win()
    {
        maxOffset = 0;
        gameObject.transform.position = new Vector3(stickman.transform.position.x, stickman.transform.position.y, gameObject.transform.position.z);

        Camera mainCam = GetComponent<Camera>();

        mainCam.orthographicSize /= winningOrthographicSize;
        
    }
}
