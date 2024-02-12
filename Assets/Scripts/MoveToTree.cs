using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTree : MoveTo
{
    [SerializeField] private float minimumDistanceToTree = 2f;

    private GameObject nearestTree;

    private bool actionCompleted = false;

    public void MoveToNearestTree()
    {
        nearestTree = FindNearestTree();

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
    }
    protected void ActionCompleted()
    {
        actionCompleted = true;
    }

    private GameObject FindNearestTree()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");

        if (trees.Length == 0)
        {
            return null;
        }

        GameObject nearestTree = trees[0];
        float shortestDistance = Vector3.Distance(transform.position, nearestTree.transform.position);

        foreach (GameObject tree in trees)
        {
            float distanceToTree = Vector3.Distance(transform.position, tree.transform.position);

            if (distanceToTree < shortestDistance)
            {
                nearestTree = tree;
                shortestDistance = distanceToTree;
            }
        }
        return nearestTree;
    }

    public bool IsActionCompleted()
    {
        return actionCompleted;
    }
    
    public void setActionCompleted(bool b)
    {
        actionCompleted = b;
    }
}
