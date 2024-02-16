using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LoggerNpcAI : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    [SerializeField] private float choppingTime = 3f; // Adjust the chopping time as needed
    [SerializeField] private int maxWalkableRange = 500000;
    [SerializeField] private int maxActionRange = 1;
 
    private NavMeshAgent navMeshAgent;
    private enum NpcState { MovingToTree, Chopping, PickUpLog, Idle }
    private NpcState currentState = NpcState.MovingToTree;
    private float timer = 0;

    private string treeTag = "Tree";
    private string logTag = "Log";

    private GameObject nearestTree;
   
    private MoveTo moveTo;
    private RangeChecker rangeChecker;

    GameObject log;
    GameObject tree;

    //done
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        moveTo = GetComponent<MoveTo>();

        rangeChecker = new RangeChecker(gameObject);

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the NPC GameObject.");
        }
        if (moveTo == null)
        {
            Debug.LogError("MoveTo script is missing on the NPC GameObject.");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case NpcState.MovingToTree:

               
                
                moveTo.MoveToNearestObject(treeTag, rangeChecker);
                
                
                if(moveTo.IsActionCompleted())
                {
                    currentState = NpcState.Idle;
                }
                


                break;

            case NpcState.Chopping:
                UpdateChoppingState(tree);
               
                break;

            case NpcState.PickUpLog:
                PickUpLog();
                
                break;

            case NpcState.Idle:
               
                log = rangeChecker.FindNearestObjectByTag(logTag);
                tree = rangeChecker.FindNearestObjectByTag(treeTag);
                
                //logs in range to pickup
                if (rangeChecker.areThereObjectsInRange(log, maxActionRange))
                {
                   // Debug.Log("There are logs in pickupable range");
                }
                //logs in range to walk to
                else if (rangeChecker.areThereObjectsInRange(log, maxWalkableRange))
                {
                  //  Debug.Log("There are logs in Walkable range");                                    
                }
                //trees in range to chop
                else if (rangeChecker.areThereObjectsInRange(tree, maxActionRange))
                {
                    UpdateChoppingState(nearestTree);
                  //  Debug.Log("There are trees in choppable range");
                }
                //trees in range to walk to
                else if (rangeChecker.areThereObjectsInRange(tree, maxWalkableRange))
                {
                  //  Debug.Log("There are trees in Walkable range");
                }
                else
                {
                    Debug.Log("nothing in range");
                }
                break;

                
        }
        //Debug.Log(currentState);
    }

    private void PickUpLog()
    {
        Debug.Log("picking up log");
    }

    private void UpdateChoppingState(GameObject tree)
    {
       

        if( true)
        {

        }

        // Simulate chopping by waiting for a certain duration
        timer += Time.deltaTime;
        if (timer >= choppingTime)
        {
            ChopTree(tree);
           
            timer = 0f;
            
        }
    }

    private void ChopTree(GameObject tree)
    {    
        tree.GetComponent<TreeLogic>().spawnLog();

        currentState = NpcState.Idle;
    }

}