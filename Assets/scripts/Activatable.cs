using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : BlockingObject
{
    protected bool playerCanActivate;
    public abstract void activate(GameObject activator);
    public abstract void setStatus(GameObject activator, bool value);
}
