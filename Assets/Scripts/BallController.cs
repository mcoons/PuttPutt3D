using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : Singleton<BallController>
{
    public Transform holeTarget;
    public Transform putterT;
    public float thrust = 0.0f;  // 0 to 1
    public float thrustMultiplier = 50.0f;
    public float velocityCutoff = 0.2f;

    Rigidbody ballRB;
    Vector3 direction;

    [SerializeField] float powerBarHeight;
    [SerializeField] int touchedGreenCount = 0;
    [SerializeField] int touchedRampCount = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        //EventManager.Instance.OnInitializeGreen.AddListener(HandleOnInitializeGreen);

        ballRB = GetComponent<Rigidbody>();
        putterT = transform.Find("Putter").transform;
        holeTarget = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;

        EventManager.Instance.OnPowerBarSizeChange.Invoke(new Vector2(30, 0));
    }

    void FixedUpdate()
    {
        if (ballRB.velocity.magnitude <= velocityCutoff &&
            ballRB.velocity != Vector3.zero &&
            ballRB.angularVelocity != Vector3.zero &&
            //GameManager.Instance.gameState == GameManager.State.Moving &&
            GameManager.Instance.gameState != GameManager.State.Win &&
            OnGreen() &&
            !OnRamp())
        {
            Debug.Log("BallController: FixedUpdate: Setting velocity to zero");
            Debug.Log("-- BallController: FixedUpdate: Invoking OnGameStateChange to Idle");
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);

            holeTarget = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;

            ballRB.velocity = Vector3.zero;
            ballRB.angularVelocity = Vector3.zero;

            transform.LookAt(holeTarget);

            // if easy mode aim putter at hole
            putterT.localPosition = new Vector3(0, 0, -0.936f);
            putterT.localRotation = Quaternion.identity;
            putterT.gameObject.SetActive(true);

            Debug.Log("-- BallController: FixedUpdate: Invoking OnPowerBarSizeChange");
            EventManager.Instance.OnPowerBarSizeChange.Invoke(new Vector2(30, 0));
        }
    }

    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.State.Moving && putterT.gameObject.activeSelf)
        {
            putterT.gameObject.SetActive(false);
        }

        holeTarget = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;

        transform.LookAt(holeTarget);

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.gameState == GameManager.State.Idle)
        {
            Debug.Log("-- BallController: Update: Invoking OnGameStateChange to Putting");
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Putting);
            StartCoroutine("GetPuttStrength");
        }
        else
        if (Input.GetKeyUp(KeyCode.Space) && GameManager.Instance.gameState == GameManager.State.Putting)
        {
            Debug.Log("-- BallController: Update: Invoking OnGameStateChange to Moving");
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Moving);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }


    //public void HandleOnInitializeGreen()
    //{
    //    Debug.Log("Ball Controller: HandleOnResetGreen");
    //    powerBarT.sizeDelta = new Vector2(30, 0);

    //    ballRB.velocity = Vector3.zero;
    //    ballRB.angularVelocity = Vector3.zero;

    //    transform.position = GameManager.Instance.currentTeeStartPosition;
    //    transform.eulerAngles = GameManager.Instance.currentTeeStartRotation;
    //    holeTarget = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;

    //    transform.LookAt(holeTarget);
    //    transform.Find("Putter").transform.gameObject.SetActive(true);
    //}

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
            Debug.Log("-- BallController: GetPuttStrength: Invoking OnPowerBarSizeChange");
            EventManager.Instance.OnPowerBarSizeChange.Invoke(new Vector2(30, powerBarHeight));

            if (thrust >= 1.0f)
            {
                Debug.Log("-- BallController: GetPuttStrength: Invoking OnGameStateChange to Moving");
                EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Moving);
            }
            yield return null;
        }
        PuttBall();
    }

    void PuttBall()
    {
        Debug.Log("-- BallController: PuttBall: Invoking OnStroke");
        EventManager.Instance.OnStroke.Invoke();

        direction = transform.position - putterT.position;

        putterT.gameObject.SetActive(false);  // message?
        ballRB.AddForce(direction * thrust * thrustMultiplier, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HoleTrigger")
        {
            Debug.Log("BallController: OnTriggerEnter: Tag = HoleTrigger");
            Debug.Log("-- BallController: OnTriggerEnter: Invoking OnGameStateChange to Win");
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Win);
        }
    }

    bool OnGreen()
    {
        return touchedGreenCount > 0;
    }

    bool OnRamp()
    {
        return touchedRampCount > 0;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Green")
        {
            Debug.Log("BallController: OnCollisionEnter: Tag = Green  touchedRampCount++");
            touchedGreenCount++;
        } else
        if (other.gameObject.tag == "Ramp")
        {
            Debug.Log("BallController: OnCollisionEnter: Tag = Ramp  touchedRampCount++");
            touchedRampCount++;
        } else
        if (other.gameObject.tag == "Terrain")
        {
            Debug.Log("BallController: OnCollisionEnter: Tag = Terrain");
            Debug.Log("-- BallController: OnCollisionEnter: Invoking OnInitializeGreen");
            EventManager.Instance.OnInitializeGreen.Invoke();
            Debug.Log("-- BallController: OnCollisionEnter: Invoking OnPowerBarSizeChange");
            EventManager.Instance.OnPowerBarSizeChange.Invoke(new Vector2(30, 0));
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Green")
        {
            Debug.Log("BallController: OnCollisionExit: Tag = Green  touchedRampCount--");
            touchedGreenCount--;
        }else
        if (other.gameObject.tag == "Ramp")
        {
            Debug.Log("BallController: OnCollisionExit: Tag = Ramp  touchedRampCount--");
            touchedRampCount--;
        }
    }


}
