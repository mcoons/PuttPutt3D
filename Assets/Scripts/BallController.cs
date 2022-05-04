/*
 
Listens for:


Invokes:
    OnPowerBarSizeChange
    OnGameStateChange
    OnStroke
    OnInitializeGreen

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : Singleton<BallController>
{
    public Transform holeTarget;
    public Transform putterT;
    public float thrust = 0.0f;  // 0 to 1
    public float thrustMultiplier = 50.0f;

    Rigidbody ballRB;
    Vector3 direction;

    [SerializeField] float powerBarHeight;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        ballRB = GetComponent<Rigidbody>();
        putterT = transform.Find("Putter").transform;
        holeTarget = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;

        EventManager.Instance.OnPowerBarSizeChange.Invoke(new Vector2(30, 0));
    }

    private void Update()
    {
        if (ballRB.IsSleeping() && GameManager.Instance.gameState == GameManager.State.Moving)
        {
            holeTarget = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;
            transform.LookAt(holeTarget);
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);
        }
        else
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.gameState == GameManager.State.Idle)
        {
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Putting);
            StartCoroutine("GetPuttStrength");
        }
        else
        if (Input.GetKeyUp(KeyCode.Space) && GameManager.Instance.gameState == GameManager.State.Putting)
        {
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Moving);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            EventManager.Instance.OnInitializeGreen.Invoke();
            EventManager.Instance.OnPowerBarSizeChange.Invoke(new Vector2(30, 0));
        }
    }

}
