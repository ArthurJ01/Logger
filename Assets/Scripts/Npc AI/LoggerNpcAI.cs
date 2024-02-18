using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LoggerNpcAI : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;

    [SerializeField] private float choppingTime = 3f; // Adjust the chopping time as needed
    [SerializeField] private int maxWalkableRange = 500000;
    [SerializeField] private int maxActionRange = 5;
    [SerializeField] private int maxLogsCarried = 2;
    [SerializeField] private GameObject logDropOffStation;
 
    private NavMeshAgent navMeshAgent;
    private enum NpcState { MovingToLog, MovingToTree,  Chopping, PickUpLog, DropLog, Idle }
    private NpcState currentState = NpcState.Idle;
    private float timer = 0;

    private int logsCarried = 0;

    private string treeTag = "Tree";
    private string logTag = "Log";
    private string logDropTag = "LogDrop";


   
    private MoveTo moveTo;
    private RangeChecker rangeChecker;

    private GameObject log;
    private GameObject tree;
    private GameObject logDrop;

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
            case NpcState.MovingToLog:

                moveTo.MoveToNearestObject(logTag, rangeChecker);
                if (moveTo.IsActionCompleted())
                {
                    currentState = NpcState.Idle;
                }

                break;

            case NpcState.MovingToTree:
                moveTo.MoveToNearestObject(treeTag, rangeChecker);                               
                if(moveTo.IsActionCompleted())
                {
                    currentState = NpcState.Idle;
                }               
                break;

            case NpcState.Chopping:

                tree = rangeChecker.FindNearestObjectByTag(treeTag);
                UpdateChoppingState(tree);
               
                break;

            case NpcState.PickUpLog:
                PickUpLog();
                
                break;

            case NpcState.DropLog:
                DropLog();

                break;

            case NpcState.Idle:
               
                log = rangeChecker.FindNearestObjectByTag(logTag);
                tree = rangeChecker.FindNearestObjectByTag(treeTag);

                if(logsCarried >= maxLogsCarried)
                {
                    currentState = NpcState.DropLog;
                }
                //logs in range to pickup
                else if (rangeChecker.AreThereObjectsInRange(log, maxActionRange))
                {
                    //remove this
                    currentState = NpcState.PickUpLog;
                }
                //logs in range to walk to
                else if (rangeChecker.AreThereObjectsInRange(log, maxWalkableRange))
                {
                    //remove this
                    currentState = NpcState.MovingToLog;
                }

                //trees in range to chop
                 else if (rangeChecker.AreThereObjectsInRange(tree, maxActionRange))
                {
                    

                    currentState = NpcState.Chopping;
                }
                //trees in range to walk to
                else if (rangeChecker.AreThereObjectsInRange(tree, maxWalkableRange))
                {
                    //Debug.Log("There are trees in walkable range");
                    currentState = NpcState.MovingToTree;
                }
                else
                {
                    Debug.Log("nothing in range");
                }
                break;

                
        }
        Debug.Log(currentState);
    }

    private void DropLog()
    {
        logDrop = rangeChecker.FindNearestObjectByTag(logDropTag);
        moveTo.MoveToNearestObject(logDropTag, rangeChecker);
        if (moveTo.IsActionCompleted())
        {
            while (logsCarried > 0)
            {
                Debug.Log("log dropped");
                if(logDropOffStation != null)
                {
                    logDropOffStation.GetComponent<LogDropStationHandler>().AddLogToStation();
                    logsCarried -= 1;
                }
                
                
            }

            currentState = NpcState.Idle;


        }
    }

    private void PickUpLog()
    {
        
        log = rangeChecker.FindNearestObjectByTag(logTag);

        timer += Time.deltaTime;
        if (timer >= choppingTime)
        {
            log.SetActive(false);
            logsCarried += 1;
            Debug.Log("logs carried : " + logsCarried);
            timer = 0f;
            currentState = NpcState.Idle;
        }

        
        
    }

    private void UpdateChoppingState(GameObject t)
    {

        //Debug.Log("Choppiong");

        // Simulate chopping by waiting for a certain duration
        timer += Time.deltaTime;
        if (timer >= choppingTime)
        {
            ChopTree(t);
           
            timer = 0f;
            
        }
    }

    private void ChopTree(GameObject t)
    {    
        t.GetComponent<TreeLogic>().spawnLog();

        currentState = NpcState.Idle;
    }

}