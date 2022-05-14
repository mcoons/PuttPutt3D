/*
 
Listens for:
    OnGameStateChange

*/


using UnityEngine;
using System.Collections;


public class Putter : Singleton<Putter>
{
    public float putterRotationSpeed = 20.0f;
    public bool rotating = false;

    Transform ballT;

    #region Unity Callbacks

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ballT = GameObject.Find("Ball").transform;
        EventManager.Instance.OnGameStateChange.AddListener(HandleOnGameStateChange);
    }

    protected override void OnDestroy()
    {
        EventManager.Instance.OnGameStateChange.RemoveListener(HandleOnGameStateChange);
        StopAllCoroutines();
        base.OnDestroy();
    }

    #endregion

    public void HandlePointerDown(int dir)
    {
        if (rotating || !gameObject.activeSelf || GameManager.Instance.gameState != GameManager.State.Idle)
            return;

        rotating = true;

        StartCoroutine(Rotate(dir));
    }

    public void HandlePointerUp()
    {
        rotating = false;
    }

    private IEnumerator Rotate(int dir)
    {
        while (rotating)
        {
            transform.RotateAround(ballT.position, Vector3.up, dir * putterRotationSpeed * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    private void HandleOnGameStateChange(GameManager.State newState)
    {
        if (newState == GameManager.State.Moving && gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        if (newState == GameManager.State.Idle && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

}
