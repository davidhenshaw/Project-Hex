using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : BoardElement
{
    [SerializeField] GameObject[] connections;
    List<TriggerResponse> responses = new List<TriggerResponse>();

    protected override void Start()
    {
        base.Start();
        foreach(GameObject obj in connections)
        {
            GetTriggerResponse(obj);
        }
    }

    private void GetTriggerResponse(GameObject obj)
    {
        if (obj.TryGetComponent(out TriggerResponse res))
        {
            responses.Add(res);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            Activate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            Deactivate();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent(out PlayerController player))
        {
            UpdateActive();
        }
    }

    private void Deactivate()
    {
        foreach (TriggerResponse response in responses)
        {
            response.OnDeactivate();
        }
    }

    private void Activate()
    {
        foreach (TriggerResponse response in responses)
        {
            response.OnActivate();
        }
    }

    private void UpdateActive()
    {
        foreach (TriggerResponse response in responses)
        {
            response.WhileActive();
        }
    }
}

public interface TriggerResponse
{
    void OnActivate();
    void OnDeactivate();
    void WhileActive();
}
