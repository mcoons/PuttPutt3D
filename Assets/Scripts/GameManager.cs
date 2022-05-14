/*
 
Listens for:
    OnGameStateChange
    OnStroke
    OnNextGreen
    OnInitializeGreen

Invokes:

*/

using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public enum State
    {
        Menu,
        Idle,
        Putting,
        Putting2,
        Moving,
        Hole,
        GameOver,
        Restart
    }

    //public Hashtable result = new Hashtable()
    //{
    //    {"-4", "Condor"},
    //    {"-3", "Albatross"},
    //    {"-2", "Eagle"},
    //    {"-1", "Birdie"},
    //    {"0", "Par"},
    //    {"1", "Bogie"},
    //    {"2", "Double Bogie"},
    //    {"3", "Triple Bogie"},
    //    {"4", "Quadruple Bogie"}
    //};

    public struct Score
    {
        public string description;
        public int par;
        public int strokes;
    }

    public State gameState;
    public Score[] scores;

    public GameObject[] greens;
    public int currentGreenIndex = 0;
    public GameObject currentGreenObject;

    [SerializeField] int totalStrokes = 0;
    [SerializeField] int totalPar = 0;

    Vector3 holeTargetPosition;
    Vector3 currentTeeStartPosition;
    Vector3 currentTeeStartRotation;

    #region Unity Callbacks

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        EventManager.Instance.OnGameStateChange.AddListener(HandleOnGameStateChange);
        EventManager.Instance.OnStroke.AddListener(HandleOnStroke);
        EventManager.Instance.OnNextGreen.AddListener(HandleOnNextGreen);
        EventManager.Instance.OnOutOfBounds.AddListener(HandleOnOutOfBounds);
        EventManager.Instance.OnGameRestart.AddListener(HandleOnGameRestart);

        gameState = State.Menu;
        scores = new Score[greens.Length];

        foreach(GameObject green in greens)
        {
            totalPar += green.GetComponent<Data>().par;
        }

        InitializeGreen();
    }

    protected override void OnDestroy()
    {
        EventManager.Instance.OnGameStateChange.RemoveListener(HandleOnGameStateChange);
        EventManager.Instance.OnStroke.RemoveListener(HandleOnStroke);
        EventManager.Instance.OnNextGreen.RemoveListener(HandleOnNextGreen);
        EventManager.Instance.OnOutOfBounds.RemoveListener(HandleOnOutOfBounds);
        EventManager.Instance.OnGameRestart.RemoveListener(HandleOnGameRestart);

        StopAllCoroutines();
        base.OnDestroy();
    }

    #endregion

    private void InitializeGreen()
    {
        currentGreenObject = greens[currentGreenIndex];

        currentTeeStartPosition = currentGreenObject.transform.Find("Tee").transform.position;
        currentTeeStartRotation = currentGreenObject.transform.Find("Tee").transform.eulerAngles;
        holeTargetPosition = currentGreenObject.transform.Find("Hole").transform.position;

        scores[currentGreenIndex].par = currentGreenObject.GetComponent<Data>().par;
        scores[currentGreenIndex].description = currentGreenObject.GetComponent<Data>().description;

        EventManager.Instance.OnNewGreenInfo.Invoke(currentTeeStartPosition, currentTeeStartRotation, holeTargetPosition);
        EventManager.Instance.OnSetVCam.Invoke(currentGreenObject.transform.Find("Hole").transform);
        EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Idle);
    }

    private void HandleOnGameStateChange(State newState)
    {
        gameState = newState;
    }

    private void HandleOnStroke()
    {
        scores[currentGreenIndex].strokes++;
        totalStrokes++;
    }

    private void HandleOnNextGreen()
    {
        currentGreenIndex = currentGreenIndex == greens.Length - 1 ? 0 : currentGreenIndex + 1;
        InitializeGreen();
    }

    private void HandleOnOutOfBounds()
    {
        InitializeGreen();
    }

    private void HandleOnGameRestart()
    {
        currentGreenIndex = 0;
        gameState = State.Menu;
        scores = new Score[greens.Length];
        totalStrokes = 0;
        InitializeGreen();
    }
}
