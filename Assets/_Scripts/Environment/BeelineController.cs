using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeelineController : MonoBehaviour
{
    public BeeBehavior Leader { get; set; }

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
        var newDonorLeader = Instantiate(BeelineManager.Instance.FollowerPrefab,
                                donor.Leader.transform.position,
                                donor.Leader.transform.rotation,
                                receiver.transform);

        yield return null;

        //Transfer the all donor bees to receiver parent
        BeeBehavior currBee = donor.Leader.followerBee;
        donor.Leader.DeferredDisable = true;
        donor.Leader.CopyState(newDonorLeader.GetComponent<BeeBehavior>());

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

    public void RemoveBee(BeeBehavior toRemove)
    {
        StartCoroutine(HandleBeelineSplit(toRemove));
    }

    public static IEnumerator HandleBeelineSplit(BeeBehavior toRemove)
    {
        BeelineController newBeeline = null;
        //Find the bee to remove
        if(toRemove.followerBee)
        {
            newBeeline = GenerateNewBeeline(toRemove.followerBee);
        }
        
        ClearNeighborReferences(toRemove);
        Destroy(toRemove.gameObject);

        yield return new WaitForEndOfFrame();

        if(newBeeline != null)
        {
            newBeeline.GenerateLinkedList();
            newBeeline.Leader.DoInteraction();
        }
    }

    private static void ClearNeighborReferences(BeeBehavior bee)
    {
        if(bee.leaderBee)
        {
            bee.leaderBee.RemoveFollower();
            bee.RemoveLeader();
        }

        if(bee.followerBee)
        {
            bee.followerBee.RemoveLeader();
            bee.RemoveFollower();
        }
    }

    private static BeelineController GenerateNewBeeline(BeeBehavior rootBee)
    {
        if (!rootBee)
            return null;

        var newController = Instantiate(BeelineManager.Instance.EmptyBeelinePrefab, Board.Instance.grid.transform);
        
        //Replace previous root bee with a leader bee. There's a chance it already was one but whatever
        var newLeaderObj = Instantiate(BeelineManager.Instance.LeaderPrefab, 
            rootBee.transform.position, 
            rootBee.transform.rotation, 
            newController.gameObject.transform);

        BeeBehavior newLeaderBee = newLeaderObj.GetComponent<BeeBehavior>();

        if (rootBee.followerBee)
            newLeaderBee.SetFollower(rootBee.followerBee);

        rootBee.CopyState(newLeaderBee);

        //Transfer rest of old beeline to the new one
        var currBee = rootBee.followerBee;
        while (currBee != null)
        {
            currBee.transform.SetParent(newController.transform, true);
            currBee = currBee.followerBee;
        }

        //dispose of old leader (19th century france be like >:))
        ClearNeighborReferences(rootBee);
        Destroy(rootBee.gameObject);

        return newController.GetComponent<BeelineController>();
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
                    currentBee.SetLeader(prev);
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
