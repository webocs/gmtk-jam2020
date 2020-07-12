using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : Activatable
{
    public AudioClip pushSound;
    public override void activate(GameObject activator)
    {        

    }   

    private void Awake()
    {
        CanBeTraversed = false;
        playerCanActivate = false;
        CanBePushed = true;
    }

    public override void setStatus(GameObject activator, bool status)
    {
    }


}
