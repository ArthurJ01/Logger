using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeChecker
{
    private GameObject gameObject;

    public RangeChecker(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
    
    public bool areThereObjectsInRange(GameObject obj, int range)
    {
        if (obj == null) { return false; }

        float distanceToObject = Vector3.Distance(gameObject.transform.position, obj.transform.position);

        if (distanceToObject < range)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
   
    public GameObject FindNearestObjectByTag(string tag)
    {
        GameObject[] objectList = GameObject.FindGameObjectsWithTag(tag);

        if (objectList.Length == 0)
        {
            return null;
        }
        //nearest object is temporarily set to first object in array
        GameObject nearestObject = objectList[0];
        float shortestDistance = Vector3.Distance(gameObject.transform.position, nearestObject.transform.position);

        //is there a tree closer
        foreach (GameObject obj in objectList)
        {
            float distanceToObject = Vector3.Distance(gameObject.transform.position, nearestObject.transform.position);

            if (distanceToObject < shortestDistance)
            {
                nearestObject = obj;
                shortestDistance = distanceToObject;
            }
        }
        return nearestObject;
    }
}
