using UnityEngine;
using UnityEngine.Events;

public class CustomEvents
{
    //[System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }
    [System.Serializable] public class EventGameStateChange : UnityEvent<GameManager.State> { }
    [System.Serializable] public class EventStroke : UnityEvent { }

    //[System.Serializable] public class EventTowerAnimationStart : UnityEvent { }
    //[System.Serializable] public class EventTowerAnimationComplete : UnityEvent { }

    //[System.Serializable] public class EventLevelAnimationStart : UnityEvent { }
    //[System.Serializable] public class EventLevelAnimationComplete : UnityEvent { }

    ////[System.Serializable] public class EventObjectCountChange : UnityEvent<TowerManager.ItemType, int> { }
    //[System.Serializable] public class EventObjectAdded : UnityEvent<TowerManager.ItemType, GameObject> { }

    //[System.Serializable] public class EventObjectSelected : UnityEvent<string, TowerManager.ItemType, Vector3> { }
    //[System.Serializable] public class EventObjectMatched : UnityEvent<string, TowerManager.ItemType, Vector3> { }
    //[System.Serializable] public class EventUnselectAll : UnityEvent { }
    //[System.Serializable] public class EventObjectRemoved : UnityEvent { }


    //[System.Serializable] public class EventObjectDropStart : UnityEvent { }
    //[System.Serializable] public class EventObjectDropComplete : UnityEvent { }

    //[System.Serializable] public class EventGameLoss : UnityEvent { }
    //[System.Serializable] public class EventGameWon : UnityEvent { }

    //[System.Serializable] public class TypeCountChange : UnityEvent<int> { }
    //[System.Serializable] public class TowerNumberChange : UnityEvent<int> { }
    //[System.Serializable] public class UpdateLevelText : UnityEvent { }

}

//** usage examples **//
// EventManager.Instance.OnObjectMatched.AddListener(HandleOnObjectMatched);
// EventManager.Instance.OnObjectMatched.Invoke(transform.name, type, _globalPosition);

// INHERITANCE
public class EventManager : Singleton<EventManager>
{
    // GameManager
    public CustomEvents.EventGameStateChange OnGameStateChange;
    public CustomEvents.EventStroke OnStroke;

    //// MainMenu
    //public CustomEvents.EventFadeComplete OnMainMenuFadeComplete;

    //// Item
    //public CustomEvents.EventObjectAdded OnObjectAdded;
    ////public CustomEvents.EventObjectCountChange OnObjectCountChange;
    //public CustomEvents.EventObjectSelected OnObjectSelected;
    //public CustomEvents.EventObjectMatched OnObjectMatched;

    //// TODO
    ////public CustomEvents.EventUnselectAll OnUnselectAll;
    //public CustomEvents.EventObjectRemoved OnObjectRemoved;

    //// TowerManager
    ////public CustomEvents.EventTowerAnimationStart OnTowerAnimationStart;
    ////public CustomEvents.EventTowerAnimationComplete OnTowerAnimationComplete;

    ////public CustomEvents.EventLevelAnimationStart OnLevelAnimationStart;
    ////public CustomEvents.EventLevelAnimationComplete OnLevelAnimationComplete;

    ////public CustomEvents.EventObjectDropStart OnObjectDropStart;
    //public CustomEvents.EventObjectDropComplete OnObjectDropComplete;

    //public CustomEvents.TypeCountChange OnTypeCountChange;
    //public CustomEvents.TowerNumberChange OnTowerNumberChange;
    //public CustomEvents.UpdateLevelText OnUpdateLevelText;


    //public CustomEvents.EventGameLoss OnGameLoss;
    //public CustomEvents.EventGameLoss OnGameWin;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
