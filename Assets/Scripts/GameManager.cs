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
        Putting2,
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


    public CinemachineVirtualCamera ballCam;
    public Transform followParentT;
    public Transform ballT;
    public Rigidbody ballRB;
    public GameObject[] greens;
    public GameObject currentGreenObject;
    public int currentGreenIndex = 0;
    public int par = 0;
    public int stroke = 0;

    public State gameState;

    Vector3 holeTarget;

    [SerializeField] string description;
    [SerializeField] Vector3 currentTeeStartPosition;
    [SerializeField] Vector3 currentTeeStartRotation;

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
            InitializeGreen();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextGreen();
        }
    }

    protected override void OnDestroy()
    {
        EventManager.Instance.OnGameStateChange.RemoveListener(HandleOnGameStateChange);
        EventManager.Instance.OnStroke.RemoveListener(HandleOnStroke);
        EventManager.Instance.OnNextGreen.RemoveListener(HandleOnNextGreen);
        EventManager.Instance.OnInitializeGreen.RemoveListener(InitializeGreen);
        base.OnDestroy();
    }

    // Event Handlers

    void HandleOnGameStateChange(State newState)
    {
        gameState = newState;
    }

    void HandleOnStroke()
    {
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

        currentTeeStartPosition = currentGreenObject.transform.Find("Tee").transform.position;
        currentTeeStartRotation = currentGreenObject.transform.Find("Tee").transform.eulerAngles;
        holeTarget = currentGreenObject.transform.Find("Hole").transform.position;

        par = currentGreenObject.GetComponent<Data>().par;
        description = currentGreenObject.GetComponent<Data>().description;

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
