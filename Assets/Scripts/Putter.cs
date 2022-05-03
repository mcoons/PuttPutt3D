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
        //EventManager.Instance.OnPutterEnable.AddListener(HandleOnPutterEnable);
        //EventManager.Instance.OnPutterDisable.AddListener(HandleOnPutterDisable);
        EventManager.Instance.OnGameStateChange.AddListener(HandleOnGameStateChange);

    }

    protected override void OnDestroy()
    {
        //EventManager.Instance.OnPutterEnable.RemoveListener(HandleOnPutterEnable);
        //EventManager.Instance.OnPutterDisable.RemoveListener(HandleOnPutterDisable);
        EventManager.Instance.OnGameStateChange.RemoveListener(HandleOnGameStateChange);

        base.OnDestroy();
    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.State.Idle)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(ballT.position, Vector3.up, -putterRotationSpeed * Time.deltaTime);
        }
        else 
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(ballT.position, Vector3.up, putterRotationSpeed * Time.deltaTime);
        }

        // Rotate the putter every frame so it keeps looking at the ball
        //transform.LookAt(ballT);
    }

    //void HandleOnPutterEnable()
    //{

    //    transform.LookAt(ballT);

    //}

    //void HandleOnPutterDisable()
    //{

    //}

    void HandleOnGameStateChange(GameManager.State newState)
    {
        if (GameManager.Instance.gameState == GameManager.State.Moving && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        if (GameManager.Instance.gameState == GameManager.State.Idle && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            //transform.LookAt(ballT);

        }
    }

}
