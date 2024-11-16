using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCutting : MonoBehaviour
{

    [SerializeField] private LayerMask treeLayer;

    private bool inCuttingRange = false;

    // Update is called once per frame
    void Update()
    {

        
        Debug.Log(inCuttingRange);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & treeLayer) != 0)
        {
            inCuttingRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & treeLayer) != 0)
        {
            inCuttingRange = false;
        }
    }
}
