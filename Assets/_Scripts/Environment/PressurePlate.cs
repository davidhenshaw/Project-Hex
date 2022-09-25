using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PressurePlate : BoardElement
{
    [SerializeField] GameObject[] connections;
    [SerializeField] GameObject _connectionLinePrefab;
    Dictionary<TriggerResponse, LineRenderer> connectionLines =new Dictionary<TriggerResponse, LineRenderer>();
    List<TriggerResponse> responses = new List<TriggerResponse>();

    [Space]
    [Header("Feedback")]
    [SerializeField] Color _connLineColor;
    [SerializeField] Color _connLineFlashColor;
    [SerializeField] float _flashDuration;

    protected override void Start()
    {
        base.Start();
        foreach(GameObject obj in connections)
        {
            InitTriggerResponse(obj);
        }
    }

    private void InitTriggerResponse(GameObject obj) 
    {
        if (obj.TryGetComponent(out TriggerResponse response))
        {
            responses.Add(response);
            var line = CreateNewLine(obj.transform.position);
            connectionLines.Add(response, line);
        }
    }

    private LineRenderer CreateNewLine(Vector3 endPos)
    {
        var obj = Instantiate(_connectionLinePrefab, transform);

        LineRenderer renderer = obj.GetComponent<LineRenderer>();
        renderer.SetPositions(new Vector3[] {
            transform.position,
            endPos
        });

        return renderer;
    }

    private void TriggerFeedback(TriggerResponse response)
    {
        if(connectionLines.TryGetValue(response, out LineRenderer line))
        {
            Color2 baseColor = new Color2(_connLineColor, _connLineColor);
            Color2 flashColor = new Color2(_connLineFlashColor, _connLineFlashColor);

            // Do a flash
            Sequence flashSequence = DOTween.Sequence();

            //flash in
            flashSequence.Append(
                line
                    .DOColor(baseColor, flashColor, _flashDuration)
                    .SetEase(Ease.InFlash)
            );

            //Fade out
            flashSequence.Append(
                line
                    .DOColor(flashColor, baseColor, _flashDuration*2)
                    .SetEase(Ease.Linear)
            );

            flashSequence.Play();
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
            TriggerFeedback(response);
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
