using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTree : MoveTo
{
    
/*
    private GameObject nearestTree;

    private bool actionCompleted = false;

    private string treeTag = "Tree";

    public void MoveToNearestTree(RangeChecker rangeChecker)
    {

        actionCompleted = false;
        
        nearestTree = rangeChecker.FindNearestObjectByTag(treeTag);

        if (nearestTree != null)
        {
            float distanceToTree = Vector3.Distance(transform.position, nearestTree.transform.position);

            // If the NPC is far enough from the tree, move towards it
            if (distanceToTree > minimumDistanceToTree)
            {
                navMeshAgent.SetDestination(nearestTree.transform.position);
            }
            else
            {
                // Stop the NPC when it is close enough to the tree
                navMeshAgent.ResetPath();

                targetRotation = CalculateTargetRotation(nearestTree.transform.position);
                StartCoroutine(SmoothTurn(ActionCompleted));
            }
        }
        else
        {
            Debug.Log("No trees in range");
        }
    }
    protected void ActionCompleted()
    {
        actionCompleted = true;
    }

    public bool IsActionCompleted()
    {
        return actionCompleted;
    }
    
    public void setActionCompleted(bool b)
    {
        actionCompleted = b;
    }

    public GameObject getCurrentNearestTree()
    {
        return nearestTree;
    }
*/
}
