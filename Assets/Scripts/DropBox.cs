using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour,
    IContainer
{
    private GameObject currentObject = null;
    private int amountSold;

    public void AddToContainer(GameObject objectToAdd)
    {
        if(currentObject != null)
        {
            Destroy(currentObject);
        }
        currentObject = objectToAdd;
        objectToAdd.SetActive(false);
        objectToAdd.transform.SetParent(this.gameObject.transform, false);
        ISellable iSellable = objectToAdd.GetComponent<ISellable>();
        amountSold += iSellable.GetSellAmount();
        Debug.Log("amount sold: " + amountSold);
    }

    //always false because you can sell any amount you want
    public bool IsInventoryFull()
    {
        return false;
    }

    public GameObject RetrieveFromContainer()
    {
        GameObject retrievedObject = null;

        if (currentObject != null)
        {
            retrievedObject = currentObject;
            ISellable iSellable = retrievedObject.GetComponent<ISellable>();
            amountSold -= iSellable.GetSellAmount();
        }

        return retrievedObject;
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
