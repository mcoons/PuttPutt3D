using UnityEngine;
using Cinemachine;

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

        //BallController.Instance.moving = false;
        BallController.Instance.powerBarT.sizeDelta = new Vector2(30, 0);

        ballRB.velocity = Vector3.zero;
        ballRB.angularVelocity = Vector3.zero;

        ballT.position = currentTeeStartPosition;
        ballT.eulerAngles = currentTeeStartRotation;
        ballT.LookAt(holeTarget);
        ballT.Find("Putter").transform.gameObject.SetActive(true);

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
