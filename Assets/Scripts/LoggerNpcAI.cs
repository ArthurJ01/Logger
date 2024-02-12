using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LoggerNpcAI : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    [SerializeField] private float choppingTime = 3f; // Adjust the chopping time as needed

    

    private NavMeshAgent navMeshAgent;
    private enum NpcState { MovingToTree, Chopping, PickUpLog, Idle }
    private NpcState currentState = NpcState.Idle;
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
                    currentState = NpcState.Chopping;
                }
                Debug.Log("moving to tree state");
                break;

            case NpcState.Chopping:
                UpdateChoppingState(nearestTree);
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
                    currentState = NpcState.MovingToTree;
                }
                break;
        }
    }

    private bool areThereLogsInRange()
    {
        if (FindNearestTree() == null)
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
        if (FindNearestTree() == null)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    //done
    void UpdateMovingState()
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
                StartCoroutine(SmoothTurn());
            }
        }
    }

    //done
    private IEnumerator SmoothTurn()
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        timer = 0f;

        // Transition to the Chopping state
        currentState = NpcState.Chopping;
    }

    //done
    private Quaternion CalculateTargetRotation(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        return Quaternion.LookRotation(direction);
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
    //done
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