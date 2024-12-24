using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour,
    IContainer
{
    [Tooltip("Add starting position of inventory")]
    [SerializeField] private GameObject inventoryObject;
    private float objectOffset = 0f;

    // Define an enum to select which axis to adjust
    private enum OffsetAxis { X, Y, Z }

    [Tooltip("Select the axis to adjust")]
    [SerializeField] private OffsetAxis offsetAxis = OffsetAxis.Z;

    private Stack<GameObject> inventoryStack = new();

    public void AddToContainer(GameObject objectToAdd)
    {


        inventoryStack.Push(objectToAdd);
        IInteractable interactable = objectToAdd.GetComponent<IInteractable>();

        // Apply the offset based on the selected axis
        switch (offsetAxis)
        {
            case OffsetAxis.X:
                objectToAdd.transform.position = new Vector3(objectOffset, 0, 0);
                break;
            case OffsetAxis.Y:
                objectToAdd.transform.position = new Vector3(0, objectOffset, 0);
                break;
            case OffsetAxis.Z:
                objectToAdd.transform.position = new Vector3(0, 0, objectOffset);
                break;
        }

        objectToAdd.transform.SetParent(inventoryObject.transform, false);
        float offsetValue = interactable.GetObjectSizeOffset();
        objectOffset += offsetValue;
               
    }

    public GameObject RetrieveFromContainer()
    {
        GameObject retrievedObject = null;

        if(inventoryStack.Count > 0)
        {
            retrievedObject = inventoryStack.Pop();
            IInteractable interactable = retrievedObject.GetComponent<IInteractable>();
            float offsetValue = interactable.GetObjectSizeOffset();
            objectOffset -= offsetValue;
        }

        return retrievedObject;
    }

}
