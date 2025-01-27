using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickupable
{
    public void MakePickupUpStat();
    public void MakeDroppedState();
    public int GetObjectSizeOffset();
}
