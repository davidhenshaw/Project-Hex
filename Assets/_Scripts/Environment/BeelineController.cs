using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeelineController : MonoBehaviour
{
    public BeeBehavior Leader { get; set; }

    [SerializeField]
    GameObject leaderPrefab;

    [SerializeField]
    GameObject followerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLinkedList();
    }

    /// <summary>
    /// Transfers the bees of one controller to another. The donor is deleted after the merge.
    /// </summary>
    /// <param name="donor"></param>
    /// <param name="receiver"></param>
    /// <returns></returns>
    public static IEnumerator Merge(BeelineController donor, BeelineController receiver)
    {
        //We must "convert" the donor's leader to a follower before moving the donor's original followers
        var newDonorLeader = Instantiate(donor.followerPrefab,
                                donor.Leader.transform.position,
                                donor.Leader.transform.rotation,
                                receiver.transform);

        yield return null;

        //Transfer the all donor bees to receiver parent
        BeeBehavior currBee = donor.Leader.followerBee;
        donor.Leader.DeferredDisable = true;

        while (currBee != null)
        {
            currBee.transform.SetParent(receiver.transform, true);
            currBee = currBee.followerBee;
        }

        yield return null;

        receiver.GenerateLinkedList();


        Destroy(donor.gameObject);
    }

    BeeBehavior GetLast()
    {
        BeeBehavior currBee = Leader;
        while(currBee.followerBee != null)
        {
            currBee = currBee.followerBee;
        }

        return currBee;
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
