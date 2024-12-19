using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour,
    IContainer
{
    [Tooltip("Add starting position of inventory")]
    [SerializeField] private GameObject inventoryObject;
    private int objectOffset = 0;

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

        Debug.Log(objectToAdd);

        objectToAdd.transform.SetParent(inventoryObject.transform);
        int offsetValue = interactable.GetObjectSizeOffset();
        objectOffset += offsetValue;
               
    }

    public GameObject RetrieveFromContainer()
    {
        /*
                GameObject objectToRemove = inventoryStack.Pop();

                // Add the object offset to the selected axis of objectOffsetTransform
                Vector3 newOffset = objectOffsetTransform.position;
                float offsetValue = objectToRemove.GetComponent<IInteractable>().GetObjectSizeOffset();

                switch (offsetAxis)
                {
                    case OffsetAxis.X:
                        newOffset.x -= offsetValue;
                        break;
                    case OffsetAxis.Y:
                        newOffset.y -= offsetValue;
                        break;
                    case OffsetAxis.Z:
                        newOffset.z -= offsetValue;
                        break;
                }

                objectOffsetTransform.position = newOffset;

                return objectToRemove;
        */

        return inventoryObject;
    }

}
