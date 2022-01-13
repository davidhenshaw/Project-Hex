using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventResponse : BoardElement, TriggerResponse
{
    public UnityEvent OnTriggerActivate;
    public UnityEvent OnTriggerDeactivate;

    public void OnActivate()
    {
        OnTriggerActivate.Invoke();
    }

    public void OnDeactivate()
    {
        OnTriggerDeactivate.Invoke();
    }

    public void WhileActive()
    {

    }
}
