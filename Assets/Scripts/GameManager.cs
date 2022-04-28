using UnityEngine;
using Cinemachine;
using System.Collections;

public class GameManager : Singleton<GameManager>

//public class GameManager : MonoBehaviour
{
    public enum State
    {
        Menu,
        Idle,
        Putting,
        Moving,
        Win
    }

    Hashtable result = new Hashtable()
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

    public State gameState = State.Menu;

    [Header("Ball Settings")]
    public Transform ballT;
    public Rigidbody ballRB;

    public Vector3 currentTeeStartPosition;
    public Vector3 currentTeeStartRotation;

    Vector3 holeTarget;

    public GameObject[] greens;

    public int currentGreenIndex = 0;
    public GameObject currentGreenObject;

    //public BallController ballController;

    public CinemachineVirtualCamera ballCam;
    public Transform followParentT;
  

    void Start()
    {
        EventManager.Instance.OnGameStateChange.AddListener(HandleOnGameStateChange);
        EventManager.Instance.OnStroke.AddListener(HandleOnStroke);

        ballCam = GameObject.Find("CM BallCam").GetComponent<CinemachineVirtualCamera>();
        InitializeGreen();
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

    public void HandleOnGameStateChange(State newState)
    {
        Debug.Log("State changed to " + newState);
        gameState = newState;
    }

    public void HandleOnStroke()
    {
        Debug.Log("Stroke");
        stroke++;
    }


    public void NextGreen()
    {
        currentGreenIndex = currentGreenIndex == greens.Length - 1 ? 0 : currentGreenIndex + 1;
        InitializeGreen();
    }


    public void InitializeGreen()
    {
        currentGreenObject = greens[currentGreenIndex];
        Debug.Log("Current Green Name: " + greens[currentGreenIndex].name);

        currentTeeStartPosition = currentGreenObject.transform.Find("Tee").transform.position;
        currentTeeStartRotation = currentGreenObject.transform.Find("Tee").transform.eulerAngles;
        holeTarget = currentGreenObject.transform.Find("Hole").transform.position;

        par = currentGreenObject.GetComponent<Data>().par;
        description = currentGreenObject.GetComponent<Data>().description;


        //BallController.Instance.moving = false;
        BallController.Instance.powerBarT.sizeDelta = new Vector2(30, 0);

        ballRB.velocity = Vector3.zero;
        ballRB.angularVelocity = Vector3.zero;

        ballT.position = currentTeeStartPosition;
        ballT.eulerAngles = currentTeeStartRotation;
        ballT.LookAt(holeTarget);
        ballT.Find("Putter").transform.gameObject.SetActive(true);

        stroke = 0;


        SetVCam();
        gameState = State.Idle;
    }

    public void SetVCam()
    {
        ballCam.LookAt = currentGreenObject.transform.Find("Hole").transform;
        followParentT.position = ballT.position;
        followParentT.eulerAngles = ballT.eulerAngles;
    }

}
