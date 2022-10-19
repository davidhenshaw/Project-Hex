using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeelineController : MonoBehaviour
{
    BeeBehavior Leader;
    // Start is called before the first frame update
    void Start()
    {
        GenerateLinkedList();
    }

    void GenerateLinkedList()
    {
        BeeBehavior prev = null;
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent(out BeeBehavior currentBee))
            {
                if(Leader == null)
                    Leader = currentBee;

                if(prev != null)
                {
                    currentBee.SetHead(prev);
                    prev.SetFollower(currentBee);
                }

                prev = currentBee;
            }
            else
            {
                continue;
            }

        }
    }
}
