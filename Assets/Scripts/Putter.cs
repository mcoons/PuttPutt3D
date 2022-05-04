/*
 
Listens for:
    OnGameStateChange

*/


using UnityEngine;

public class Putter : Singleton<Putter>
{
    Transform ballT;
    public float putterRotationSpeed = 20.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ballT = GameObject.Find("Ball").transform;
        EventManager.Instance.OnGameStateChange.AddListener(HandleOnGameStateChange);
    }

    protected override void OnDestroy()
    {
        EventManager.Instance.OnGameStateChange.RemoveListener(HandleOnGameStateChange);
        base.OnDestroy();
    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.State.Idle)
        {
            return;
        }
        else
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(ballT.position, Vector3.up, -putterRotationSpeed * Time.deltaTime);
        }
        else 
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(ballT.position, Vector3.up, putterRotationSpeed * Time.deltaTime);
        }
    }

    void HandleOnGameStateChange(GameManager.State newState)
    {
        //if (GameManager.Instance.gameState == GameManager.State.Moving && gameObject.activeSelf)
        if (newState == GameManager.State.Moving && gameObject.activeSelf)
            {
                gameObject.SetActive(false);
        }
        else
        //if (GameManager.Instance.gameState == GameManager.State.Idle && !gameObject.activeSelf)
        if (newState == GameManager.State.Idle && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

}
