using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBehavior : MonoBehaviour
{
    public bool IsPollenated
    {
        get;
        private set;
    } = false;

    [SerializeField]
    ParticleSystem pollenParticles;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerInteract()
    {
        Collider[] collisions = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 1));

        foreach(Collider col in collisions)
        {
            if(col.TryGetComponent(out IInteractive interactable))
            {
                interactable.OnInteract(gameObject);
            }
        }
    }

    public void SetPollen(bool value)
    {
        if(!IsPollenated && value)//If off then turned on
        {
            pollenParticles.Play();
        }

        if(value == false)
        {
            pollenParticles.Stop();
        }

        IsPollenated = value;
    }
}
