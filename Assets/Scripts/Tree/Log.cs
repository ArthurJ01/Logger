using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour,
    IInteractable
{

    [Tooltip("size of object for inventory")]
    [SerializeField] private float objectSizeOffset = .5f;

    public float GetObjectSizeOffset()
    {
        return objectSizeOffset;
    }

    public GameObject Interact()
    {
        return this.gameObject;
    }

    public void MakeDroppedState()
    {
        BoxCollider[] colliders = this.gameObject.GetComponents<BoxCollider>();

        // Iterate through the array and disable each one
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = true;
        }
    }

    public void MakePickedUpState()
    {
        BoxCollider[] colliders = this.gameObject.GetComponents<BoxCollider>();

        // Iterate through the array and disable each one
        foreach (BoxCollider collider in colliders)
        {
            collider.enabled = false;
        }

        this.transform.rotation = Quaternion.Euler(90, 90, 0);
    }
}
