using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("Main Variables")]
    [SerializeField] private Transform finishLine;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject stickman;
    [SerializeField] private GameObject winParticleEffect;
    [SerializeField] private float winningSpeed;
    
    [Header("Helper Variables - PRIVATE")]
    private Vector3 initPos;
    private bool isWon;
    private StickmanController stickmanController;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stickmanController.IsSticked() && (stickman.transform.position.x < -5 || stickman.transform.position.y < -6))
        {
                ResetGame();
        }

        if (finishLine.position.x < stickman.transform.position.x && !isWon)
        {
            isWon = true;
            Win();
        }
    }

    private void Init()
    {
        stickmanController = stickman.GetComponent<StickmanController>();
        initPos = stickman.transform.position;
    }
    
    private void ResetGame()
    {
        stickmanController.Reset(initPos);
    }

    private void Win()
    {
        stickmanController.Win(winningSpeed);
        
        winParticleEffect.SetActive(true);
        winParticleEffect.transform.parent = null;
        
        cameraController.Win();
        
        StartCoroutine(LoadLevel());
    }
    
    IEnumerator LoadLevel ()
    {
        yield return new WaitForSeconds (3);
        SceneManager.LoadScene (0); // current scene because we dont have another scene yet
    }
}
