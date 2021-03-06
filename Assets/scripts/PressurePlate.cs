﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PressurePlate : Activatable
{
    public bool status;

    GameObject pressedChild;
    GameObject releasedChild;


    private void Awake()
    {
        CanBeTraversed = true;
        playerCanActivate = false;
        pressedChild = transform.GetChild(0).gameObject;
        releasedChild = transform.GetChild(1).gameObject;
    }

    private void Update()
    {
        pressedChild.SetActive(status);
        releasedChild.SetActive(!status);
    }

    public override void activate(GameObject activator)
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
  
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "pushable")
        {
            status = true;
            if (connectedTo)
            {
                IEnumerator co = smoothActivate();
                StartCoroutine(co);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "pushable")
        {
            status = false;
            if (connectedTo)
            {
                IEnumerator co = smoothActivate();                
                StartCoroutine(co);              
            }
        }
    }

    public override void setStatus(GameObject activator, bool status)
    {
        this.status = status;
    }

    IEnumerator smoothActivate()
    {
        yield return new WaitForSeconds(0.1f);
        connectedTo.activate(gameObject);
        yield return null;      
    }
}
