/*
 
Listens for:


Invokes:
    OnPowerBarSizeChange
    OnGameStateChange
    OnStroke
    OnInitializeGreen

*/

using System.Collections;
using UnityEngine;
using Cinemachine;


public class BallController : Singleton<BallController>
{

    public float thrust = 0.0f;  // 0 to 1
    public float thrustMultiplier = 50.0f;
    public Transform followParentT;

    CinemachineVirtualCamera ballCam;
    Transform holeTargetT;
    Transform putterT;
    Rigidbody ballRB;
    Vector3 direction;
    Vector3 currentTeeStartPosition;
    Vector3 currentTeeStartRotation;
    Vector3 holeTargetPosition;
    float powerBarHeight;

    float sleepStart;
    bool longSleep = false;
    bool checkingSleep = false;
    public float sleepThreshold = 1.5f;

    #region Unity Callbacks

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        EventManager.Instance.OnNewGreenInfo.AddListener(HandleOnNewGreenInfo);
        EventManager.Instance.OnSetVCam.AddListener(HandleOnSetVCam);

        ballRB = GetComponent<Rigidbody>();
        putterT = transform.Find("Putter").transform;
        holeTargetT = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;
        ballCam = GameObject.Find("CM BallCam").GetComponent<CinemachineVirtualCamera>();

        EventManager.Instance.OnPowerBarSizeChange.Invoke(new Vector2(30, 0));
    }

    //private void FixedUpdate()
    //{
    //    if (!ballRB.IsSleeping())
    //    {
    //        checkingSleep = false;
    //    }

    //    if (ballRB.IsSleeping() && GameManager.Instance.gameState == GameManager.State.Moving && !checkingSleep)
    //    {
    //        checkingSleep = true;
    //        sleepStart = Time.time;
    //    }

    //    if (ballRB.IsSleeping() && GameManager.Instance.gameState == GameManager.State.Moving && checkingSleep)
    //    {
    //        if (Time.time - sleepStart > sleepThreshold)
    //        {
    //            checkingSleep = false;

    //            holeTargetT = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;
    //            transform.LookAt(holeTargetT);
    //            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);
    //        }
    //    }
    //}

    private void Update()
    {
        //if (!ballRB.IsSleeping())
        //{
        //    checkingSleep = false;
        //}

        //if (ballRB.IsSleeping() && GameManager.Instance.gameState == GameManager.State.Moving && !checkingSleep)
        //{
        //    checkingSleep = true;
        //    sleepStart = Time.time;
        //}

        //if (ballRB.IsSleeping() && GameManager.Instance.gameState == GameManager.State.Moving && checkingSleep)
        //{
        //    if (Time.time - sleepStart > sleepThreshold)
        //    {
        //        checkingSleep = false;

        //        holeTargetT = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;
        //        transform.LookAt(holeTargetT);
        //        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);
        //    }
        //}


        if (ballRB.IsSleeping() && GameManager.Instance.gameState == GameManager.State.Moving)
        {
            holeTargetT = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;
            transform.LookAt(holeTargetT);
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);
        }

        followParentT.position =  transform.position;

    }

    protected override void OnDestroy()
    {
        EventManager.Instance.OnNewGreenInfo.RemoveListener(HandleOnNewGreenInfo);
        EventManager.Instance.OnSetVCam.RemoveListener(HandleOnSetVCam);
        StopAllCoroutines();
        base.OnDestroy();
    }

    #endregion

    public void StartPutt()
    {
        if (GameManager.Instance.gameState != GameManager.State.Idle)
        {
            return;
        }

        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Putting);
        StartCoroutine("GetPuttStrength");
    }

    public void EndPutt()
    {
        if (GameManager.Instance.gameState != GameManager.State.Putting)
        {
            return;
        }

        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Putting2);
    }


    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    IEnumerator GetPuttStrength()
    {
        thrust = 0.0f;
        powerBarHeight = 0.0f;

        while (GameManager.Instance.gameState == GameManager.State.Putting)
        {
            thrust += 0.5f * Time.deltaTime;
            powerBarHeight = Remap(thrust, 0, 1.0f, 0, 200.0f);
            EventManager.Instance.OnPowerBarSizeChange.Invoke(new Vector2(30, powerBarHeight));

            if (thrust >= 1.0f)
            {
                EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Putting2);
            }
            yield return null;
        }
        PuttBall();
    }

    void PuttBall()
    {
        direction = transform.position - putterT.position;
        ballRB.AddForce(direction * thrust * thrustMultiplier, ForceMode.Impulse);

        EventManager.Instance.OnStroke.Invoke();
        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Moving);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HoleTrigger")
        {
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Hole);
        } 
    }

    void HandleOnNewGreenInfo(Vector3 teeStartPosition, Vector3 teeStartRotation, Vector3 targetPosition)
    {
        currentTeeStartPosition = teeStartPosition;
        currentTeeStartRotation = teeStartRotation;
        holeTargetPosition = targetPosition;

        ballRB.velocity = Vector3.zero;  
        ballRB.angularVelocity = Vector3.zero;  

        transform.position = currentTeeStartPosition;  
        transform.eulerAngles = currentTeeStartRotation;  
        transform.LookAt(holeTargetPosition);  
        transform.Find("Putter").transform.gameObject.SetActive(true);  
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            EventManager.Instance.OnOutOfBounds.Invoke();
            EventManager.Instance.OnPowerBarSizeChange.Invoke(new Vector2(30, 0));
        }
    }

    void HandleOnSetVCam(Transform holeTransform)
    {
        ballCam.LookAt = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;  
        followParentT.position = currentTeeStartPosition;  
        followParentT.eulerAngles = currentTeeStartRotation;  
    }

}
