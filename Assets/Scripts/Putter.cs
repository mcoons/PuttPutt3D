using UnityEngine;

//public class Putter : MonoBehaviour
public class Putter : Singleton<Putter>
{
    Transform ballT;

    private void Start()
    {
        ballT = GameObject.Find("Ball").transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(ballT.position, Vector3.up, 20 * Time.deltaTime);
        }
        else 
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(ballT.position, Vector3.up, -20 * Time.deltaTime);
        }

        // Rotate the putter every frame so it keeps looking at the ball
        transform.LookAt(ballT);
    }

}
