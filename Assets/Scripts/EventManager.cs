using UnityEngine;
using UnityEngine.Events;

//** usage examples **//
// EventManager.Instance.OnObjectMatched.AddListener(HandleOnObjectMatched);
// EventManager.Instance.OnObjectMatched.Invoke(transform.name, type, _globalPosition);

public class CustomEvents
{
    [System.Serializable] public class EventGameStateChange : UnityEvent<GameManager.State> { }
    [System.Serializable] public class EventStroke : UnityEvent { }
    [System.Serializable] public class EventInitializeGreen : UnityEvent { }
    [System.Serializable] public class EventNextGreen : UnityEvent { }
    [System.Serializable] public class EventPowerBarSizeChange : UnityEvent<Vector2> { }
}

public class EventManager : Singleton<EventManager>
{
    public CustomEvents.EventGameStateChange OnGameStateChange;
    public CustomEvents.EventStroke OnStroke;
    public CustomEvents.EventInitializeGreen OnInitializeGreen;
    public CustomEvents.EventNextGreen OnNextGreen;
    public CustomEvents.EventPowerBarSizeChange OnPowerBarSizeChange;

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
