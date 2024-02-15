using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] protected float minimumDistanceToTree = 2f;

    private GameObject nearestObject;

    protected Quaternion targetRotation;
    protected NavMeshAgent navMeshAgent;
    private bool actionCompleted;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the NPC GameObject.");
        }

        actionCompleted = false;
    }

    public void MoveToNearestObject(string tag, RangeChecker rangeChecker )
    {

        actionCompleted = false;

        nearestObject = rangeChecker.FindNearestObjectByTag(tag);

        if (nearestObject != null)
        {
            float distanceToTree = Vector3.Distance(transform.position, nearestObject.transform.position);

            // If the NPC is far enough from the tree, move towards it
            if (distanceToTree > minimumDistanceToTree)
            {
                navMeshAgent.SetDestination(nearestObject.transform.position);
            }
            else
            {
                // Stop the NPC when it is close enough to the tree
                navMeshAgent.ResetPath();

                targetRotation = CalculateTargetRotation(nearestObject.transform.position);
                StartCoroutine(SmoothTurn(ActionCompleted));
            }
        }
        else
        {
            Debug.Log("No objects in range");
        }      
    }
    private void ActionCompleted()
    {
        actionCompleted = true;
        
    }
    public bool isActionCompleted()
    {
        return actionCompleted;
    }
    public GameObject getNearestObject(string tag)
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
