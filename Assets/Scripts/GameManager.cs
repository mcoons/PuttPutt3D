using UnityEngine;
using Cinemachine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public enum State
    {
        Menu,
        Idle,
        Putting,
        Moving,
        Win
    }

    public Hashtable result = new Hashtable()
    {
        {"-4", "Condor"},
        {"-3", "Albatross"},
        {"-2", "Eagle"},
        {"-1", "Birdie"},
        {"0", "Par"},
        {"1", "Bogie"},
        {"2", "Double Bogie"},
        {"3", "Triple Bogie"},
        {"4", "Quadruple Bogie"}
    };

    public int par = 0;
    public int stroke = 0;
    public string description;

    public State gameState;

    [Header("Ball Settings")]
    public Transform ballT;
    public Rigidbody ballRB;

    public Vector3 currentTeeStartPosition;
    public Vector3 currentTeeStartRotation;

    Vector3 holeTarget;

    public GameObject[] greens;

    public int currentGreenIndex = 0;
    public GameObject currentGreenObject;

    public CinemachineVirtualCamera ballCam;
    public Transform followParentT;


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        EventManager.Instance.OnGameStateChange.AddListener(HandleOnGameStateChange);
        EventManager.Instance.OnStroke.AddListener(HandleOnStroke);
        EventManager.Instance.OnNextGreen.AddListener(HandleOnNextGreen);
        EventManager.Instance.OnInitializeGreen.AddListener(InitializeGreen);

        ballCam = GameObject.Find("CM BallCam").GetComponent<CinemachineVirtualCamera>();
        InitializeGreen();
        stroke = 0;

        gameState = State.Menu;
    }

    void Update()
    {
        followParentT.position = ballT.position;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("(R)eset pressed");
            InitializeGreen();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("(N)ext Green pressed");
            NextGreen();
        }
    }

    // Event Handlers

    void HandleOnGameStateChange(State newState)
    {
        Debug.Log("State changed to " + newState);
        gameState = newState;

    }

    void HandleOnStroke()
    {
        Debug.Log("Stroke");
        stroke++;
    }

    void HandleOnNextGreen()
    {
        NextGreen();
    }


    void NextGreen()
    {
        currentGreenIndex = currentGreenIndex == greens.Length - 1 ? 0 : currentGreenIndex + 1;
        InitializeGreen();
        stroke = 0;

    }


    void InitializeGreen()
    {

        currentGreenObject = greens[currentGreenIndex];
        Debug.Log("Current Green Name: " + greens[currentGreenIndex].name);

        currentTeeStartPosition = currentGreenObject.transform.Find("Tee").transform.position;
        currentTeeStartRotation = currentGreenObject.transform.Find("Tee").transform.eulerAngles;
        holeTarget = currentGreenObject.transform.Find("Hole").transform.position;

        par = currentGreenObject.GetComponent<Data>().par;
        description = currentGreenObject.GetComponent<Data>().description;


        //BallController.Instance.moving = false;
        //BallController.Instance.powerBarT.sizeDelta = new Vector2(30, 0);

        //EventManager.Instance.OnInitializeGreen.Invoke();

        ballRB.velocity = Vector3.zero;  // message?
        ballRB.angularVelocity = Vector3.zero;  // message?

        ballT.position = currentTeeStartPosition;  // message?
        ballT.eulerAngles = currentTeeStartRotation;  // message?
        ballT.LookAt(holeTarget);  // message?
        ballT.Find("Putter").transform.gameObject.SetActive(true);  // message?

        SetVCam();
        gameState = State.Idle;
    }

    void SetVCam()
    {
        ballCam.LookAt = currentGreenObject.transform.Find("Hole").transform;  // message?
        followParentT.position = ballT.position;  // message?
        followParentT.eulerAngles = ballT.eulerAngles;  // message?
    }

}
