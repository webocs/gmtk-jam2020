using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activatable
{
    public bool status;
    
    GameObject openChild;
    GameObject closedChild;

    private void Awake()
    {
        playerCanActivate = false;
        openChild = transform.GetChild(0).gameObject;
        closedChild = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        openChild.SetActive(status);
        closedChild.SetActive(!status);
        CanBeTraversed = status;
    }

    public void toggle()
    {
        status = !status;
    }

    public override void activate(GameObject activator)
    {
        Debug.Log(activator.name);
        if (playerCanActivate && activator.tag == "Player")
            toggle();
        else if (activator.tag != "Player")
            toggle();
    }

    public override void setStatus(GameObject activator, bool status)
    {
        this.status = status;
    }
}
