using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMgr 
{
    public static EventMgr Instance = null;

    public delegate void OnEventAction(int eventType, object udata, object param = null);
    private Dictionary<int, OnEventAction> eventActions = null;

    
    public void Init() {
        EventMgr.Instance = this;
        this.eventActions = new Dictionary<int, OnEventAction>();
    }

    public void AddListener(int eventName, OnEventAction onEvent) {
        if (this.eventActions.ContainsKey(eventName)) {
            this.eventActions[eventName] += onEvent;
        }
        else {
            this.eventActions[eventName] = onEvent;
        }
    }

    public void RemoveListener(int eventName, OnEventAction onEvent) {
        if (this.eventActions.ContainsKey(eventName)) {
            this.eventActions[eventName] -= onEvent;
        }

        /*if (this.eventActions[eventName] == null) {
            this.eventActions.Remove(eventName);
        }*/
    }

    public void Emit(int eventName, object udata, object param = null) {
        if (this.eventActions.ContainsKey(eventName)) {
            if (this.eventActions[eventName] != null) {
                this.eventActions[eventName](eventName, udata, param);
            }
        }
    }
}
