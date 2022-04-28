using UnityEngine;
using UnityEngine.Events;

public class CustomEvents
{
    //[System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }
    [System.Serializable] public class EventGameStateChange : UnityEvent<GameManager.State> { }
    [System.Serializable] public class EventStroke : UnityEvent { }
    [System.Serializable] public class InitializeGreen : UnityEvent { }
    [System.Serializable] public class NextGreen : UnityEvent { }
    [System.Serializable] public class PowerBarSizeChange : UnityEvent<Vector2> { }

    ////[System.Serializable] public class EventObjectCountChange : UnityEvent<TowerManager.ItemType, int> { }

}

//** usage examples **//
// EventManager.Instance.OnObjectMatched.AddListener(HandleOnObjectMatched);
// EventManager.Instance.OnObjectMatched.Invoke(transform.name, type, _globalPosition);

public class EventManager : Singleton<EventManager>
{
    public CustomEvents.EventGameStateChange OnGameStateChange;
    public CustomEvents.EventStroke OnStroke;
    public CustomEvents.InitializeGreen OnInitializeGreen;
    public CustomEvents.NextGreen OnNextGreen;
    public CustomEvents.PowerBarSizeChange OnPowerBarSizeChange;


    //public CustomEvents.EventObjectCountChange OnObjectCountChange;


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
