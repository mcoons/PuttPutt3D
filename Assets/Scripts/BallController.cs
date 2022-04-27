using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.Mathematics;

public class BallController : MonoBehaviour
{
    public GameManager gameManager;

    Rigidbody ballRB;
    public Transform holeTarget;
    public bool moving;

    //public Transform ballT;
    public RectTransform powerBarT;
    public float powerBarHeight;

    public Transform putterT;
    public bool putting = false;

    Vector3 direction;

    public float thrust = 1000f;
    public float velocityCutoff = 0.1f;



    void Start()
    {

        Debug.Log("Current Green Name: " + gameManager.greens[gameManager.currentGreenIndex].name);
        ballRB = GetComponent<Rigidbody>();
        ballRB.maxAngularVelocity = 100;  // needed ?
        putterT = transform.Find("Putter").transform;
        holeTarget = gameManager.greens[gameManager.currentGreenIndex].transform.Find("Hole").transform;

        powerBarT.sizeDelta = new Vector2(30, 0);
    }

    void Update()
    {
        if (ballRB.velocity.magnitude <= velocityCutoff && ballRB.velocity != Vector3.zero && ballRB.angularVelocity != Vector3.zero)
        {
            Debug.Log("Setting velocity to zero");
            ballRB.velocity = Vector3.zero;
            ballRB.angularVelocity = Vector3.zero; 
            transform.LookAt(holeTarget);
            putterT.gameObject.SetActive(true);
            moving = false;
            powerBarT.sizeDelta = new Vector2(30, 0);

        }

        if (Input.GetKeyDown(KeyCode.Space) && !moving && !putting)
        {
            Debug.Log("Space pressed in Ball Controller");

            putting = true;
            StartCoroutine("GettingPuttStrength");
        }

        if (Input.GetKeyUp(KeyCode.Space) && !moving && putting)
        {
            putting = false;
        }
    }

    private float MyRemap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    IEnumerator GettingPuttStrength()
    {
        thrust = 0.0f;
        powerBarHeight = 0.0f;

        while (putting)
        {
            thrust += 0.5f * Time.deltaTime;
            powerBarHeight = MyRemap(thrust, 0, 1.0f, 0, 200.0f);
            powerBarT.sizeDelta = new Vector2(30, powerBarHeight);

            if (thrust >= 1.0f)
            {
                putting = false;
            }
            yield return null;
        }
        PuttBall();
    }

    void PuttBall()
    {
        moving = true;
        direction = transform.position - putterT.position;

        putterT.gameObject.SetActive(false);
        ballRB.AddForce(direction * thrust * 50, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hole");
    }


}
