using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("Ball Settings")]
    public Transform ballT;
    public Rigidbody ballRB;

    public Vector3 currentTeeStartPosition;
    public Vector3 currentTeeStartRotation;

    Vector3 holeTarget;

    public GameObject[] greens;

    public int currentGreenIndex = 0;
    public GameObject currentGreenObject;

    public BallController ballController;

    public CinemachineVirtualCamera ballCam;

    private void OnEnable()
    {
        ballCam = GameObject.Find("CM BallCam").GetComponent<CinemachineVirtualCamera>();
        InitializeHole();
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("(R)eset pressed");
            InitializeHole();
        }
    }


    public void InitializeHole()
    {
        currentGreenObject = greens[currentGreenIndex];

        currentTeeStartPosition = currentGreenObject.transform.Find("Tee").transform.position;
        currentTeeStartRotation = currentGreenObject.transform.Find("Tee").transform.eulerAngles;

        holeTarget = currentGreenObject.transform.Find("Hole").transform.position;

        ballT.position = currentTeeStartPosition;
        ballT.eulerAngles = currentTeeStartRotation;

        ballRB.velocity = Vector3.zero;
        ballRB.angularVelocity = Vector3.zero;

        ballController.moving = false;

        ballCam.LookAt = currentGreenObject.transform.Find("Hole").transform;


        ballT.LookAt(holeTarget);
        ballT.Find("Putter").transform.gameObject.SetActive(true);
        ballController.powerBarT.sizeDelta = new Vector2(30, 0);
    }
}
