using UnityEngine;
using UnityEngine.Events;

//** usage examples **//
// EventManager.Instance.OnObjectMatched.AddListener(HandleOnObjectMatched);
// EventManager.Instance.OnObjectMatched.Invoke(transform.name, type, _globalPosition);

public class CustomEvents
{
    [System.Serializable] public class EventGameStateChange : UnityEvent<GameManager.State> { }
    [System.Serializable] public class EventStroke : UnityEvent { }
    [System.Serializable] public class InitializeGreen : UnityEvent { }
    [System.Serializable] public class NextGreen : UnityEvent { }
    [System.Serializable] public class PowerBarSizeChange : UnityEvent<Vector2> { }
}

public class EventManager : Singleton<EventManager>
{
    public CustomEvents.EventGameStateChange OnGameStateChange;
    public CustomEvents.EventStroke OnStroke;
    public CustomEvents.InitializeGreen OnInitializeGreen;
    public CustomEvents.NextGreen OnNextGreen;
    public CustomEvents.PowerBarSizeChange OnPowerBarSizeChange;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
