using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour,
    IInteractable
{

    [Tooltip("size of object for inventory")]
    [SerializeField] private float objectSizeOffset = .5f;
    [SerializeField] LayerMask groundLayer;
    private Rigidbody rigidbody;


    private void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            rigidbody.isKinematic = true;
        }
    }

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
        rigidbody.isKinematic = false;
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

        Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();

        // Reset velocity
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
