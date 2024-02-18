using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogDropStationHandler : MonoBehaviour
{


    [SerializeField] private float logDropInterval;

    private float logDropIntervalConst;
    private int logsInStation;
    // Start is called before the first frame update
    void Start()
    {
        logDropIntervalConst = logDropInterval;
    }

    // Update is called once per frame
    void Update()
    {
        logDropInterval -= Time.deltaTime;

        if(logDropInterval <= 0)
        {
            logDropInterval = logDropIntervalConst;

            DropLog();
        }
    }

    private void DropLog()
    {
        if(logsInStation > 0)
        {
            Debug.Log("dropping logs in water");

            logsInStation -= 1;
        }
    }

    public void AddLogToStation()
    {
        logsInStation += 1;
    }

}
