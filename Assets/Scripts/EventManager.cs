using UnityEngine;
using UnityEngine.Events;

//** usage examples **//
// EventManager.Instance.OnGameStateChange.AddListener(HandleOnGameStateChange);
// EventManager.Instance.OnGameStateChange.Invoke(GameManager.State.Menu);

public class CustomEvents
{
    [System.Serializable] public class EventGameStateChange : UnityEvent<GameManager.State> { }
    [System.Serializable] public class EventStroke : UnityEvent { }
    [System.Serializable] public class EventNewGreenInfo : UnityEvent<Vector3, Vector3, Vector3> { }
    [System.Serializable] public class EventOutOfBounds : UnityEvent { }
    [System.Serializable] public class EventNextGreen : UnityEvent { }
    [System.Serializable] public class EventPowerBarSizeChange : UnityEvent<Vector2> { }
    [System.Serializable] public class EventSetVCam : UnityEvent<Transform> { }
    [System.Serializable] public class EventGameRestart : UnityEvent { }
}

public class EventManager : Singleton<EventManager>
{
    public CustomEvents.EventGameStateChange OnGameStateChange;
    public CustomEvents.EventStroke OnStroke;
    public CustomEvents.EventNewGreenInfo OnNewGreenInfo;
    public CustomEvents.EventOutOfBounds OnOutOfBounds;
    public CustomEvents.EventNextGreen OnNextGreen;
    public CustomEvents.EventPowerBarSizeChange OnPowerBarSizeChange;
    public CustomEvents.EventSetVCam OnSetVCam;
    public CustomEvents.EventGameRestart OnGameRestart;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
