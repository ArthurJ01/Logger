using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryingLogs : MonoBehaviour
{

    [SerializeField] private List<GameObject> logs = new();


    private int logsCarried = 0;





    public void addLogToCarried()
    {
        if(logsCarried < logs.Count)
        {
            logs[logsCarried].GetComponent<MeshRenderer>().enabled = true;
            logsCarried++;
        }
        
    }

    //return 1 if succesful, return 0 if empty
    public int removeLogFromCarried()
    {
        if (logsCarried >= 1)
        {
            logs[logsCarried].GetComponent<MeshRenderer>().enabled = false;
            logsCarried--;
            return 1;
        }

        return 0;
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
