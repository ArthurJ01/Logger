using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    
    [SerializeField] private float rotationSpeed = 5f;
    private float minimumDistanceToObject = 1f;

    private GameObject nearestObject;

    protected Quaternion targetRotation;
    protected NavMeshAgent navMeshAgent;
    private bool actionCompleted;
    private bool actionStarted;

    private float distanceToTree;
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

    private void MoveToThis(string tag, RangeChecker rangeChecker)
    {

    }

    public void MoveToNearestObject(string tag, RangeChecker rangeChecker )
    {
        nearestObject = rangeChecker.FindNearestObjectByTag(tag);
        Transform objWaypoint = nearestObject.transform.Find("waypoint");
        distanceToTree = Vector3.Distance(transform.position, objWaypoint.position);
        //Debug.Log(actionCompleted);

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



    private void SetActionCompleted(bool actionCompleted)
    {
        this.actionCompleted = actionCompleted;
        
    }
    public bool IsActionCompleted()
    {
        return actionCompleted;
    }
    public GameObject GetNearestObject(string tag)
    {
        if(nearestObject.tag == tag)
        {
            return nearestObject;
        }
        else
        {
            return null;
        }

        
    }
    protected IEnumerator SmoothTurn(System.Action callback)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        callback?.Invoke();
    }
    protected Quaternion CalculateTargetRotation(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        return Quaternion.LookRotation(direction);
    }

}
