using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ShipBehavior : ScriptableObject
{
    public abstract void Behavior(ShipController shipController);
}
