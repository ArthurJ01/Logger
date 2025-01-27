using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    GameObject Interact();

    //disable stuff to disable (triggers,colliders)
    void MakePickedUpState();

    //un-disable stuff from above
    void MakeDroppedState();

    //return offset to use when stacking objects in containers (size of object basically, in the stacking direction)
    float GetObjectSizeOffset();
}
