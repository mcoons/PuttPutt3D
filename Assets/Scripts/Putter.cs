using UnityEngine;

public class Putter : Singleton<Putter>
{
    Transform ballT;
    public float putterRotationSpeed = 20.0f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void Start()
    {
        ballT = GameObject.Find("Ball").transform;
    }

    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.State.Idle)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(ballT.position, Vector3.up, -putterRotationSpeed * Time.deltaTime);
        }
        else 
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(ballT.position, Vector3.up, putterRotationSpeed * Time.deltaTime);
        }

        // Rotate the putter every frame so it keeps looking at the ball
        transform.LookAt(ballT);
    }

}
