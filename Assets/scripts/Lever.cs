using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Activatable
{
    public bool status;

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
        if (connectedTo)
        {
            IEnumerator co = smoothActivate();
            StartCoroutine(co);
        }
    }

    public override void activate(GameObject activator)
    {
        toggle();
    }

    public override void setStatus(GameObject activator, bool status)
    {
        this.status = status;
    }

    IEnumerator smoothActivate()
    {
        yield return new WaitForSeconds(0.2f);
        connectedTo.activate(gameObject);
        yield return null;
    }
}
