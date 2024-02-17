using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    private GameObject nearestObject;
    private NavMeshAgent navMeshAgent;

    private bool actionCompleted;
    private bool actionStarted;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the NPC GameObject.");
        }

        
        actionCompleted = false;
        actionStarted = false;
    }
    public void MoveToNearestObject(string tag, RangeChecker rangeChecker )
    {
        nearestObject = rangeChecker.FindNearestObjectByTag(tag);
        Transform objWaypoint = nearestObject.transform.Find("waypoint");
        if (objWaypoint != null)
        {
            navMeshAgent.SetDestination(objWaypoint.position);           
            actionCompleted = false;
        }
        else
        {
            Debug.Log("No objects in range");
        }
        if(navMeshAgent.hasPath)
        {
            actionStarted = true;
        }
        
        if (!navMeshAgent.hasPath && !actionCompleted && actionStarted)
        {
            actionCompleted = true;
            actionStarted = false;

            Debug.Log("path done");
        }
        
    }
    public bool IsActionCompleted()
    {
        return actionCompleted;
    }
}
