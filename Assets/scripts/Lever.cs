using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Activatable
{
    public bool status;
    public Activatable connectedTo;

    GameObject onChild;
    GameObject offChild;


    private void Awake()
    {
        onChild =transform.GetChild(0).gameObject;
        offChild =transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        onChild.SetActive(status);
        offChild.SetActive(!status);
    }

    public void toggle()
    {
        status = !status;
        if (connectedTo) connectedTo.activate(gameObject);
    }

    public override void activate(GameObject activator)
    {
        toggle();
    }

    public override void setStatus(GameObject activator, bool status)
    {
        this.status = status;
    }
}
