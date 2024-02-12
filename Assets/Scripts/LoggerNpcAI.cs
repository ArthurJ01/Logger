using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LoggerNpcAI : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    [SerializeField] private float choppingTime = 3f; // Adjust the chopping time as needed

    

    private NavMeshAgent navMeshAgent;
    private enum NpcState { MovingToTree, Chopping, PickUpLog, Idle }
    private NpcState currentState = NpcState.MovingToTree;
    private float timer;

    //done
    private Quaternion targetRotation;
    private GameObject nearestTree;
    [SerializeField] private float minimumDistanceToTree = 2f;
    [SerializeField] private float rotationSpeed = 5f; // Adjust the rotation speed as needed

    private MoveToTree MoveToTree;

    //done
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        MoveToTree = GetComponent<MoveToTree>();


        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the NPC GameObject.");
        }
        if (MoveToTree == null)
        {
            Debug.LogError("MoveToTree script is missing on the NPC GameObject.");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case NpcState.MovingToTree:                           
                MoveToTree.MoveToNearestTree();
                if (MoveToTree.IsActionCompleted())
                {
                    currentState = NpcState.Idle;
                }
                Debug.Log("moving to tree state");
                break;

            case NpcState.Chopping:
                UpdateChoppingState(MoveToTree.FindNearestTree());
                Debug.Log("chopping state");
                break;

            case NpcState.PickUpLog:
                PickUpLog();
                Debug.Log("pickup state");
                break;

            case NpcState.Idle:
                Debug.Log("Idle state");


                if (areThereLogsInRange())
                {
                    currentState = NpcState.PickUpLog;
                }
                if (areThereTreesInRange())
                {
                    currentState = NpcState.Chopping;
                }
                break;
        }
    }

    private bool areThereLogsInRange()
    {
        if (MoveToTree.FindNearestTree() == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void PickUpLog()
    {
        Debug.Log("picking up log");
    }

    private bool areThereTreesInRange()
    {
        if (MoveToTree.FindNearestTree() == null)
        {
            return false;
        }
        else
        {
            return true;
        }

    }





    private void UpdateChoppingState(GameObject nearestTree)
    {

        // Simulate chopping by waiting for a certain duration
        timer += Time.deltaTime;
        if (timer >= choppingTime)
        {
            ChopTree(nearestTree);
           
            timer = 0f;
            
        }
    }

    private void ChopTree(GameObject nearestTree)
    {    
        nearestTree.GetComponent<TreeLogic>().spawnLog();

        currentState = NpcState.Idle;
    }


    private GameObject FindNearestlog()
    {
        GameObject[] logs = GameObject.FindGameObjectsWithTag("Log");

        if (logs.Length == 0)
        {
            return null;
        }

        GameObject nearestLog = logs[0];
        float shortestDistance = Vector3.Distance(transform.position, nearestLog.transform.position);

        foreach (GameObject log in logs)
        {
            float distanceToLog = Vector3.Distance(transform.position, log.transform.position);

            if (distanceToLog < shortestDistance)
            {
                nearestLog = log;
                shortestDistance = distanceToLog;
            }
        }
        return nearestLog;
    }
}