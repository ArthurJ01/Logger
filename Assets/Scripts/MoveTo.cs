using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    
    [SerializeField] private float rotationSpeed = 5f;

    protected Quaternion targetRotation;
    protected NavMeshAgent navMeshAgent;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the NPC GameObject.");
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
