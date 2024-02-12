using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LoggerNpcAI : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float minimumDistanceToTree = 2f;
    [SerializeField] private float rotationSpeed = 5f; // Adjust the rotation speed as needed
    [SerializeField] private float choppingTime = 3f; // Adjust the chopping time as needed


    private NavMeshAgent navMeshAgent;
    private enum NpcState { MovingToTree, Chopping, PickUpTree }
    private NpcState currentState = NpcState.MovingToTree;
    private float timer;
    private Quaternion targetRotation;
    private GameObject nearestTree;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the NPC GameObject.");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case NpcState.MovingToTree:
                UpdateMovingState();
                break;

            case NpcState.Chopping:
                UpdateChoppingState(nearestTree);
                break;

            case NpcState.PickUpTree:
                PickUpTree();
                break;
        }
    }

    private void PickUpTree()
    {
        
    }

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

    IEnumerator SmoothTurn()
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            yield return null;
        }

        // Transition to the Chopping state
        currentState = NpcState.Chopping;
        timer = 0f;
    }

    Quaternion CalculateTargetRotation(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        return Quaternion.LookRotation(direction);
    }

    void UpdateChoppingState(GameObject nearestTree)
    {
        Debug.Log("Starting to chop tree");
        // Simulate chopping by waiting for a certain duration
        timer += Time.deltaTime;
        if (timer >= choppingTime)
        {
            currentState = NpcState.PickUpTree;
            ChopTree(nearestTree);
        }
    }

    void ChopTree(GameObject nearestTree)
    {
       
        nearestTree.GetComponent<TreeLogic>().spawnLog();

        //Destroy(nearestTree);



        Debug.Log("tree chopped!");
    }

    GameObject FindNearestTree()
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
}