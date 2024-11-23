using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryingLogs : MonoBehaviour
{

    [SerializeField] private List<GameObject> logs = new();


    private int logsCarried = 0;




    //return true if succesful, false if not. Use to check if object was added to container or not
    public bool AddLogToCarried()
    {
        if(logsCarried < logs.Count)
        {
            logs[logsCarried].GetComponent<MeshRenderer>().enabled = true;
            logsCarried++;
            return true;
        }
        return false;
    }

    //return true if succesful, false if not. Use to check if object was removed from container or not
    public bool RemoveLogFromCarried()
    {
        if (logsCarried >= 1)
        {
            logs[logsCarried].GetComponent<MeshRenderer>().enabled = false;
            logsCarried--;
            return true;
        }

        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
