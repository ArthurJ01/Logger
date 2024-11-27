using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContainer
{
    void AddToContainer(GameObject objectToAdd);
    GameObject RetrieveFromContainer(); 
}
