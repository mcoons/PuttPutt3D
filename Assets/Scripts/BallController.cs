using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : Singleton<BallController>
{
    Rigidbody ballRB;
    public Transform holeTarget;

    public RectTransform powerBarT;
    public float powerBarHeight;

    public Transform putterT;

    Vector3 direction;

    public float thrust = 0.0f;  // 0 to 1
    public float thrustMultiplier = 50.0f;
    public float velocityCutoff = 0.1f;



    void Start()
    {
        ballRB = GetComponent<Rigidbody>();
        ballRB.maxAngularVelocity = 100;  // needed ?
        putterT = transform.Find("Putter").transform;
        holeTarget = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;

        powerBarT.sizeDelta = new Vector2(30, 0);
    }

    void Update()
    {
        if (ballRB.velocity.magnitude <= velocityCutoff &&
            ballRB.velocity != Vector3.zero &&
            ballRB.angularVelocity != Vector3.zero &&
            GameManager.Instance.gameState == GameManager.State.Moving)
        {
            Debug.Log("Setting velocity to zero");
            //GameManager.Instance.gameState = GameManager.State.Idle;
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);

            holeTarget = GameManager.Instance.currentGreenObject.transform.Find("Hole").transform;

            ballRB.velocity = Vector3.zero;
            ballRB.angularVelocity = Vector3.zero; 
            transform.LookAt(holeTarget);
            putterT.gameObject.SetActive(true);
            powerBarT.sizeDelta = new Vector2(30, 0);

        }


        if (Input.GetKeyDown(KeyCode.Space) &&
            GameManager.Instance.gameState == GameManager.State.Idle)
        {
            Debug.Log("Space pressed in Ball Controller");
            //GameManager.Instance.gameState = GameManager.State.Putting;
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Putting);

            StartCoroutine("GetPuttStrength");
        }

        if (Input.GetKeyUp(KeyCode.Space) && GameManager.Instance.gameState == GameManager.State.Putting)
        {
            //GameManager.Instance.gameState = GameManager.State.Moving;
            EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Moving);
        }
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
            powerBarT.sizeDelta = new Vector2(30, powerBarHeight);

            if (thrust >= 1.0f)
            {
                //GameManager.Instance.gameState = GameManager.State.Moving;
                EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Moving);

            }
            yield return null;
        }
        PuttBall();
    }

    void PuttBall()
    {
        //GameManager.Instance.stroke++;
        EventManager.Instance.OnStroke.Invoke();

        direction = transform.position - putterT.position;

        putterT.gameObject.SetActive(false);
        ballRB.AddForce(direction * thrust * thrustMultiplier, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        //GameManager.Instance.gameState = GameManager.State.Win;
        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Win);

        Debug.Log("Hole");
    }


}
